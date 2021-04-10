using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    [Header("Left click to add a node. Right click to remove")]

    public List<Shape> shapes = new List<Shape>();

    public float handleRadius = .5f;
}

[System.Serializable]
public class Shape
{
    public List<Vector3> points = new List<Vector3>();
}