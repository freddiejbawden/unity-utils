using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileUpdate
{
    public Vector2 pos;
    public TileGrid.TileObject.TileSprite sprite;
}

public class TileGrid
{
    public event System.EventHandler OnLoaded;

    private Grid<TileObject> grid;
    public TileGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new Grid<TileObject>(width, height, cellSize, originPosition, (Grid<TileObject> g, int x, int y) => new TileObject(g, x, y));
    }

    public Grid<TileObject> GetGrid()
    {
        return grid;
    }

    public void SetGrid(List<TileUpdate> updates)
    {
        foreach (TileUpdate update in updates)
        {
            SetTileSprite(update.pos, update.sprite);
        }
    }
    public void SetTileSprite(Vector3 worldPos, TileObject.TileSprite sprite)
    {
        TileObject tileObject = grid.GetGridObject(worldPos);
        if (tileObject != null)
        {
            tileObject.SetTileSprite(sprite);
        }
    }
    public void SetTileSprite(Vector2 gridPos, TileObject.TileSprite sprite)
    {
        TileObject tileObject = grid.GetGridObject((int)gridPos.x, (int)gridPos.y);
        if (tileObject != null)
        {
            tileObject.SetTileSprite(sprite);
        }
    }


    public void SetTileVisual(TileVisual tileVisual)
    {
        tileVisual.SetGrid(GridManager.Instance.tileGrid, grid);
    }


    public void Save()
    {
        List<TileObject.SaveObject> save = new List<TileObject.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                TileObject tileObject = grid.GetGridObject(x, y);
                save.Add(tileObject.Save());
            }
        }
        SaveObject saveObject = new SaveObject { saveObjectArray = save.ToArray() };
        SaveSystem.SaveObject(saveObject);
    }

    public void Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        if (saveObject != null)
        {
            foreach (TileObject.SaveObject savedTile in saveObject.saveObjectArray)
            {
                TileObject co = grid.GetGridObject(savedTile.x, savedTile.y);
                co.Load(savedTile);
            }
            OnLoaded?.Invoke(this, System.EventArgs.Empty);
        }
       
    }
    public class SaveObject
    {
        public TileObject.SaveObject[] saveObjectArray;
    }

    public class TileObject
    {
        public enum TileSprite
        {
            // Put grid options here
            TILE_A,
            TILE_B,
            TILE_C,
            TILE_D
        }

        private Grid<TileObject> grid;
        int x;
        int y;
        private TileSprite tileSprite;

        public TileObject(Grid<TileObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

        }

        public void SetTileSprite(TileSprite tileSprite)
        {
            this.tileSprite = tileSprite;
            grid.TriggerGridObjectChanged(x, y);
        }

        public TileSprite GetTileSprite()
        {
            return tileSprite;
        }
        public override string ToString()
        {
            return tileSprite.ToString();
        }

        [System.Serializable]
        public class SaveObject
        {
            public TileSprite sprite;
            public int x;
            public int y;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                sprite = tileSprite,
                x = x,
                y = y,
            };
        }
        public void Load(SaveObject save)
        {
            tileSprite = save.sprite;
        }
    }


}

