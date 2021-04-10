using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour {
    [System.Serializable]
    public struct TileUV {
        public TileGrid.TileObject.TileSprite sprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private TileUV[] tileUVs;
    private Grid<TileGrid.TileObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<TileGrid.TileObject.TileSprite, UVCoords> uvCoordsDict;


    private void Awake() {
        mesh = new Mesh();
        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float tWidth = texture.width;
        float tHeight = texture.height;
        uvCoordsDict = new Dictionary<TileGrid.TileObject.TileSprite, UVCoords>();
        GetComponent<MeshFilter>().mesh = mesh;

        foreach(TileUV cv in tileUVs)
        {
            uvCoordsDict[cv.sprite] = new UVCoords
            {
                uv00 = new Vector2(cv.uv00Pixels.x/tWidth, cv.uv00Pixels.y / tHeight),
                uv11 = new Vector2(cv.uv11Pixels.x / tWidth, cv.uv11Pixels.y / tHeight),
            };
        }
    }


    public void SetGrid(TileGrid c, Grid<TileGrid.TileObject> grid) {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        c.OnLoaded += Tilemap_OnLoaded;
    }

    private void Tilemap_OnLoaded(object sender, System.EventArgs e)
    {
        updateMesh = true;

    }

    private void Grid_OnGridValueChanged(object sender, Grid<TileGrid.TileObject>.OnGridObjectChangedEventArgs e) {
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                TileGrid.TileObject gridObject = grid.GetGridObject(x, y);
                TileGrid.TileObject.TileSprite sprite = gridObject.GetTileSprite();
                Vector2 gridValueUV00, gridValueUV11;
               
                UVCoords uVCoords = uvCoordsDict[sprite];
                gridValueUV00 = uVCoords.uv00;
                gridValueUV11 = uVCoords.uv11;
                
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}


public class HeatMapGridObject {

    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue) {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized() {
        return (float)value / MAX;
    }

    public override string ToString() {
        return value.ToString();
    }

}

