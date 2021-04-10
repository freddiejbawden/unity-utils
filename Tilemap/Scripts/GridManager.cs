
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public int width;
    public int height;
    public float size = 10f;
    public static GridManager Instance { private set; get; }

    public TileGrid tileGrid;

    private void Awake()
    {
        Instance = this;
        tileGrid = new TileGrid(width, height, size, Vector3.zero);
    }

    private void Start()
    {
    }

}
