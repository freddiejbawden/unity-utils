using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBuilder : MonoBehaviour
{

    [SerializeField] private TileVisual tileVisual;
    TileGrid.TileObject.TileSprite tileSprite;
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        if (!worldCamera.orthographic)
        {
            Debug.LogError(System.String.Format("{0} is not orthagraphic, this will cause errors with getting the mouse position", worldCamera.name));
        }
        Vector3 p = screenPosition;
        p.z = 20;
        Vector3 pos = worldCamera.ScreenToWorldPoint(p);
        return pos;
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    private void Start()
    {
        GridManager.Instance.tileGrid.Load();
        GridManager.Instance.tileGrid.SetTileVisual(tileVisual);
    }


    public void SetGrid(List<TileUpdate> updates)
    {
        foreach (TileUpdate update in updates)
        {
            GridManager.Instance.tileGrid.SetTileSprite(update.pos, update.sprite);
        }
    }
    public Grid<TileGrid.TileObject> GetGrid()
    {
        return GridManager.Instance.tileGrid.GetGrid();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            GridManager.Instance.tileGrid.SetTileSprite(mouseWorldPosition, tileSprite);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            tileSprite = TileGrid.TileObject.TileSprite.TILE_A;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            tileSprite = TileGrid.TileObject.TileSprite.TILE_B;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            tileSprite = TileGrid.TileObject.TileSprite.TILE_C;
        }
    }
}
