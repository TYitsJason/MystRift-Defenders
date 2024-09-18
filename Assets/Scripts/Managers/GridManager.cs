using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public int width = 10;
    public int height = 3;
    public GameObject gridCellPrefab1;
    public GameObject gridCellPrefab2;
    public LayerMask GameGridLayer;

    public GridCell[,] grid;

    public void CreateGrid()
    {
        if (grid != null)
        {
            ResetGrid();
            return;
        }

        grid = new GridCell[width, height];

        GameObject gridObject = new GameObject("GameGrid");
        gridObject.layer = LayerMask.NameToLayer("GameGrid");

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellInstance;
                if ((z + x) % 2 == 0)
                    cellInstance = Instantiate(gridCellPrefab1, new Vector3(x, 0, z), Quaternion.identity);
                else
                    cellInstance = Instantiate(gridCellPrefab2, new Vector3(x, 0, z), Quaternion.identity);
                cellInstance.name = $"Cell_{x}_{z}";
                cellInstance.transform.SetParent(gridObject.transform, true);
                cellInstance.layer = LayerMask.NameToLayer("GameGrid");
                cellInstance.AddComponent<GridCell>();
                grid[x, z] = cellInstance.GetComponent<GridCell>();
                grid[x, z].Initialize(x, z);
            }
        }
    }

    public void ResetGrid()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, z].Initialize(x, z);
            }
        }
    }
}
public class GridCell : MonoBehaviour
{
    // Properties
    public bool CanPlaceTower { get;  set; }
    public bool IsOccupiedByTower { get; set; }
    public bool IsOccupiedByEnemyStructure { get;  set; }
    public bool CanActivatePower { get;  set; }

    // Occupying object
    public TowerCard tower;
    public GameObject towerObject;
    public EnemyStructure structure;

    // Position of the cell in the grid
    public int X { get; private set; }
    public int Y { get; private set; }

    // Initialize the cell with its position and default values
    public void Initialize(int x, int y)
    {
        X = x;
        Y = y;

        tower = null;
        structure = null;

        // Player's area is in the first 5 columns
        CanPlaceTower = x < 5;
        IsOccupiedByTower = false;
        IsOccupiedByEnemyStructure = false;
        DestroyPlacedTower();
    }
    // Call this method to try placing a tower in this cell
    public void TryPlaceTower(CardInstance towerToPlace)
    {
        IsOccupiedByTower = true;
        tower = towerToPlace.cardData as TowerCard;
        towerObject = Instantiate(tower.towerPrefab, transform);
        TowerObject controller = towerObject.GetComponent<TowerObject>();
        controller.Power = tower.attackPower;
        controller.Health = tower.health;
        controller.card = towerToPlace;
        towerToPlace.isPlaced = true;
        controller.Position = this;
    }

    public void DestroyPlacedTower()
    {
        IsOccupiedByTower = false;
        tower = null;
        Destroy(towerObject);
    }

    // Call this method to add an enemy structure to this cell
    public void PlaceEnemyStructure()
    {
        if (!IsOccupiedByTower && !IsOccupiedByEnemyStructure)
        {
            IsOccupiedByEnemyStructure = true;
            // Add logic here to visually place the enemy structure or trigger game mechanics
        }
    }
}