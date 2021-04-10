using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{

	static bool IsPointInPolygon(Vector2 point, List<Vector3> polygon)
	{
		int polygonLength = polygon.Count, i = 0;
		bool inside = false;
		// x, y for tested point.
		float pointX = point.x, pointY = point.y;
		// start / end point for the current polygon segment.
		float startX, startY, endX, endY;
		Vector2 endPoint = polygon[polygonLength - 1].ToXZ();
		endX = endPoint.x;
		endY = endPoint.y;
		while (i < polygonLength)
		{
			startX = endX; startY = endY;
			endPoint = polygon[i++].ToXZ();
			endX = endPoint.x; endY = endPoint.y;
			//
			inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
					  && /* if so, test if it is under the segment */
					  ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
		}
		return inside;
	}
	public static List<Vector2> GeneratePoints(float radius, Vector2 sampleStart, Vector2 sampleEnd, Shape shape, int numSamplesBeforeRejection = 30)
	{
		foreach(Vector3 p in shape.points)
        {
			Debug.Log(p);
        }
		float cellSize = radius / Mathf.Sqrt(2);
		Vector2 sampleRegionSize = sampleStart - sampleEnd;
		int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
		List<Vector2>points = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		Vector2 midPoint = sampleEnd + sampleRegionSize / 2;
		spawnPoints.Add(midPoint);
		while (spawnPoints.Count > 0)
		{
			
			int spawnIndex = Random.Range(0, spawnPoints.Count);
			Vector2 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				float angle = Random.value * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
				Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
				if (IsValid(candidate, sampleStart, sampleEnd, cellSize, radius, points, grid, shape))
				{
					points.Add(candidate);
					spawnPoints.Add(candidate);

					grid[(int)((candidate.x - sampleEnd.x) / cellSize), (int)((candidate.y - sampleEnd.y) / cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}
		return points;
	}

	static bool IsValid(Vector2 candidate, Vector2 sampleStart, Vector2 sampleEnd, float cellSize, float radius, List<Vector2> points, int[,] grid, Shape shape)
	{
		if (IsPointInPolygon(candidate, shape.points))
        {
			if (candidate.x >= sampleEnd.x && candidate.x < sampleStart.x && candidate.y >= sampleEnd.y && candidate.y < sampleStart.y)
			{
				int cellX = (int)((candidate.x - sampleEnd.x) / cellSize);
				int cellY = (int)((candidate.y - sampleEnd.y) / cellSize);
				int searchStartX = Mathf.Max(0, cellX - 2);
				int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
				int searchStartY = Mathf.Max(0, cellY - 2);
				int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

				for (int x = searchStartX; x <= searchEndX; x++)
				{
					for (int y = searchStartY; y <= searchEndY; y++)
					{
						int pointIndex = grid[x, y] - 1;
						if (pointIndex != -1)
						{
							float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
							if (sqrDst < radius * radius)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
        }
		return false;
	}
}
