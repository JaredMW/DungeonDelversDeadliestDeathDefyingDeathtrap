using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate the background of the play area, dynamically
/// </summary>
public class BackgroundGenerator : MonoBehaviour {

    // Fields
    #region Fields & Constants
    public List<GameObject> floorTiles;

    /// <summary>Height of next map to generate</summary>
    public int mapHeight = 16;
    private int mapWidth;
    private float unitsPerSquare;
    private float screenWidthInUnits;
    private float screenHeightInUnits;

    private TileType[,] grid;  // int[Columns, Rows]
    private bool[,] gridBuffer;
    private GameObject map;

    private List<Vector3> floorScales;
    
    private const float MapDepth = .01f;
    #endregion


    // Enums
    #region Enums
    /// <summary>
    /// Identification of special level types
    /// </summary>
    public enum LevelType
    {
        Normal,
        Night,
        Tutorial
    }

    /// <summary>
    /// Representations of tile types
    /// </summary>
    public enum TileType
    {
        Floor,
        Wall,
        Door,
    }
    #endregion


    // Properties
    #region Properties


    /// <summary>
    /// The number of Unity units each grid square takes
    /// </summary>
    public float UnitsPerSquare
    {
        get { return unitsPerSquare; }
    }


    #region To-Be-Generated Width & Height Properties
    /// <summary>
    /// The height of the next-to-be-generated level. (Number of vertical grid
    /// squares.)
    /// Setting height: Value must be greater than 0. Map will be updated with
    /// new height when the next new map is generated.
    /// </summary>
    public int MapHeight
    {
        get { return mapHeight; }

        set
        {
            if (value > 0)
            {
                // Doesn't update the grid because it could cause problems if
                // done in mid-gameplay. Wait for next map to generate.
                mapHeight = value;
            }
        }
    }

    /// <summary>
    /// The width of the next-to-be-generated level. (Number of horizontal grid
    /// squares.)
    /// </summary>
    public int MapWidth
    {
        get { return mapWidth; }
    }
    #endregion

    /// <summary>
    /// The height of the currently active map grid.
    /// Getting height: If level grid has not been created yet, will return
    /// a value of 0.
    /// </summary>
    public int LeveHeight
    {
        get
        {
            // Return the current grid size if the grid exists
            if (grid != null)
            {
                return grid.GetLength(1);
            }

            // Return 0 if no grid exists yet
            else
            {
                return 0;
            }
        }
    }
    #endregion


    // Use this for initialization
    void Start()
    {
        if (mapHeight < 3)
        {
            mapHeight = 16;
        }

        // Create the physical game object representation of the map
        map = new GameObject("Background");
        map.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(
            0,
            0,
            -Camera.main.transform.position.z + MapDepth));

        UpdateScreenSize();
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Recalculate the width and height of the screen, and store it in
    /// Unity units
    /// </summary>
    public void UpdateScreenSize()
    {
        screenWidthInUnits
            = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x
            - Camera.main.transform.position.x;
        screenHeightInUnits
            = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y
            - Camera.main.transform.position.y;
    }


    /// <summary>
    /// Update the size of the grid with a new level height; recalculate the
    /// sizes of sprite objects (if necessary) to fit within the new height.
    /// </summary>
    /// <param name="height">Squares per column/vertical height of map</param>
    private void UpdateGridSize(int height)
    {
        // Resize the tiles being inserted into this level to fit the new
        // map size.
        #region Tile Resizing

        // Calculate the height & width of the grid, and unit size per tile
        if (height < 3)
        {
            height = 16;
        }

        mapHeight = height;
        unitsPerSquare = screenHeightInUnits / mapHeight;
        mapWidth = (int)Mathf.Ceil(screenWidthInUnits / unitsPerSquare);


        // Recalculate scalings for each floor sprite
        #region Resize Floor Tiles
        if (floorTiles != null && floorTiles.Count > 0)
        {
            floorScales = new List<Vector3>();

            for (int i = 0; i < floorTiles.Count; i++)
            {
                floorScales.Add(GetScaleSpriteToTileSize(
                    floorTiles[i].GetComponent<SpriteInfo>()));
            }
        }
        #endregion


        //// Recalculate scalings for each floor sprite
        //#region Resize Wall Tiles
        //if (wallTiles != null && wallTiles.Count > 0)
        //{
        //    wallScales = new List<Vector3>(wallTiles.Count);

        //    for (int i = 0; i < wallTiles.Count; i++)
        //    {
        //        wallScales.Add(GetScaleSpriteToTileSize(
        //            wallTiles[i].GetComponent<SpriteInfo>()));
        //        //wallTiles[i].GetComponent<SpriteInfo>().Radius = unitsPerSquare;
        //    }
        //}
        //#endregion

        //// Recalculate scalings for each door sprite
        //if (doorTile != null)
        //{
        //    doorScale = GetScaleSpriteToTileSize(
        //        doorTile.GetComponent<SpriteInfo>());
        //    doorTile.GetComponent<SpriteInfo>().Radius = unitsPerSquare;
        //}

        //if (openDoorTile != null)
        //{
        //    openDoorScale = GetScaleSpriteToTileSize(
        //        openDoorTile.GetComponent<SpriteInfo>());
        //    openDoorTile.GetComponent<SpriteInfo>().Radius
        //        = unitsPerSquare * .3f;
        //}
        #endregion


        // Create the new level size by making a new 2D Coordinate Grid
        // Ensure width and height are valid values
        if (mapWidth <= 0)
        {
            mapWidth = 1;
        }
        if (mapHeight <= 0)
        {
            mapHeight = 1;
        }

        grid = new TileType[mapWidth, mapHeight];
        gridBuffer = new bool[mapWidth, mapHeight];
    }


    //#region Level Advancing Methods
    ///// <summary>
    ///// Advance to the next level and generate a new map
    ///// </summary>
    //public void AdvanceLevel()
    //{
    //    // Increment level by 1
    //    level++;


    //    // Create the next level's map based on currently set map height
    //    GenerateMap(mapHeight);
    //}


    ///// <summary>
    ///// Advance to the next level, and generate a new map of a specified level
    ///// type
    ///// </summary>
    ///// <param name="levelType">Level type of next map</param>
    //public void AdvanceLevel(LevelType levelType)
    //{
    //    // Increment level by 1
    //    level++;


    //    // Create the next level's map based on specified level type and current
    //    // map height
    //    GenerateMap(levelType);
    //}


    ///// <summary>
    ///// Advance to the next level, and generate a new map
    ///// </summary>
    ///// <param name="levelType">Type of next level map</param>
    ///// <param name="mapHeight">Height of next map</param>
    //public void AdvanceLevel(LevelType levelType, int mapHeight)
    //{
    //    // Increment level by 1
    //    level++;


    //    // Create the next level's map
    //    GenerateMap(levelType, mapHeight);
    //}
    //#endregion


    #region Map Generation Methods
    /// <summary>
    /// Randomly generate a new Normal map by assigning values to the grid
    /// array, and populate the map with tiles based on Normal level type.
    /// </summary>
    private void GenerateMap()
    {
        GenerateMap(LevelType.Normal, mapHeight);
    }


    /// <summary>
    /// Randomly generate a new Normal map by assigning values to the grid
    /// array, and populate the map with tiles based on Normal level type.
    /// </summary>
    /// <param name="height">Height of Normal map to generate</param>
    private void GenerateMap(int height)
    {
        GenerateMap(LevelType.Normal, height);
    }


    /// <summary>
    /// Randomly generate a new map by assigning values to the grid
    /// array, and populate the map with tiles based on level type.
    /// </summary>
    /// <param name="levelType">Type of map level to generate</param>
    private void GenerateMap(LevelType levelType)
    {
        GenerateMap(levelType, mapHeight);
    }


    /// <summary>
    /// Randomly generate a new map by assigning values to the grid
    /// array, and populate the map with tiles based on level type.
    /// </summary>
    /// <param name="levelType">Type of map level to generate</param>
    /// <param name="height">Height of map to generate</param>
    private void GenerateMap(LevelType levelType, int height)
    {
        #region Ensure valid values
        // Reset the contents current grid, if necessary
        if (map != null)
        {
            for (int i = 0; i < map.transform.childCount; i++)
            {
                Destroy(map.transform.GetChild(i).gameObject);
            }
        }

        //// Ensure valid tile values
        //if (wallTileInfos != null)
        //{
        //    wallTileInfos.Clear();
        //}
        //else
        //{
        //    wallTileInfos = new List<SpriteInfo>();
        //}

        //if (mapWallTiles != null)
        //{
        //    mapWallTiles.Clear();
        //}
        //else
        //{
        //    mapWallTiles = new List<GameObject>();
        //}


        // Check for a valid height
        if (height < 1)
        {
            if (mapHeight > 0)
            {
                height = mapHeight;
            }
            else
            {
                height = 16;
            }
        }
        #endregion


        // Resize the grid based on values that are known to be valid
        UpdateGridSize(height);


        // Populate the map
        // Populate entire map with floor tiles
        #region Populate Floor Tiles
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = TileType.Floor;
            }
        }
        #endregion


        // Create the physical gameobjects representing the level
        #region Create Tiles
        GameObject spriteToMake;
        int tileIndex;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                switch (grid[x, y])
                {
                    // Create a floor tile
                    case TileType.Floor:
                        // Select a random floor tile sprite to place
                        tileIndex = Random.Range(0, floorTiles.Count);

                        // Instantiate the sprite
                        spriteToMake = Instantiate(
                            floorTiles[tileIndex],
                            new Vector3(
                                map.transform.position.x
                                    + (x * unitsPerSquare * 2 + unitsPerSquare),
                                map.transform.position.y
                                    + (y * unitsPerSquare * 2 + unitsPerSquare),
                                map.transform.position.z),
                            Quaternion.identity) as GameObject;

                        spriteToMake.name = "FloorTile_"
                            + floorTiles[tileIndex].name;
                        spriteToMake.transform.localScale
                            = floorScales[tileIndex];
                        spriteToMake.transform.SetParent(map.transform);

                        spriteToMake.GetComponent<SpriteInfo>().Radius
                            = unitsPerSquare;
                        break;

                    // Default case
                    default:
                        break;
                }
            }
        }

        #endregion
    }
    #endregion

    ///// <summary>
    ///// Get the wall tile at a specified index
    ///// </summary>
    ///// <param name="index">Index of wall tile to access</param>
    ///// <returns>Wall tile at an index</returns>
    //public GameObject GetWallTile(int index)
    //{
    //    if (index < mapWallTiles.Count)
    //    {
    //        return mapWallTiles[index];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    ///// <summary>
    ///// Get sprite info of a wall tile
    ///// </summary>
    ///// <param name="index">Index of wall tile to access</param>
    ///// <returns>SpriteInfo of wall tile</returns>
    //public SpriteInfo GetWallInfo(int index)
    //{
    //    if (index < wallTileInfos.Count)
    //    {
    //        return wallTileInfos[index];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}


    ///// <summary>
    ///// Get a random, valid spawn location for the player to spawn in
    ///// </summary>
    ///// <returns></returns>
    //public Vector3 GetRandomSpawnLocation()
    //{
    //    if (grid != null && mapWallTiles.Count < grid.Length)
    //    {
    //        Vector3 location = new Vector3();
    //        int minY = 1;

    //        if (level == 2 && LeveHeight > 4)
    //        {
    //            minY = 4;
    //        }

    //        do
    //        {
    //            location = new Vector3(
    //                Random.Range(1, grid.GetLength(0) - 1),
    //                Random.Range(minY, grid.GetLength(1) - 1));
    //        } while (grid[(int)location.x, (int)location.y] != TileType.Floor);

    //        location.x = map.transform.position.x
    //                + (location.x * unitsPerSquare * 2 + unitsPerSquare);
    //        location.y = map.transform.position.y
    //                + (location.y * unitsPerSquare * 2 + unitsPerSquare);
    //        location.z = map.transform.position.z;

    //        return location;
    //    }

    //    return Vector3.zero;
    //}


    /// <summary>
    /// Get the scale vector it would take to resize an object to the same
    /// size as a tile
    /// </summary>
    /// <param name="spriteInfo">Object to resize</param>
    /// <returns>Scale vector that would set the size of an object to the
    /// size of a tile</returns>
    public Vector3 GetScaleSpriteToTileSize(SpriteInfo spriteInfo)
    {
        float targetScaleX = 1;
        float targetScaleY = 1;

        float radius = spriteInfo.radius;

        if (radius > 0)
        {
            radius
                = ((1 / spriteInfo.gameObject.transform.localScale.x)
                    * (spriteInfo.SpriteRenderer.bounds.size.x)
                + (1 / spriteInfo.gameObject.transform.localScale.y)
                    * (spriteInfo.SpriteRenderer.bounds.size.y))
                / 2;
        }
        else if (radius == 0)
        {
            radius = unitsPerSquare / 2f;
        }
        else if (radius < 0)
        {
            radius = Mathf.Abs(radius);
        }

        // If still zero, set to default
        if (radius <= 0)
        {
            radius = 1;
        }


        targetScaleY = 2 * unitsPerSquare / radius;
        targetScaleX = 2 * unitsPerSquare / radius;

        return new Vector3(targetScaleX, targetScaleY, 1);
    }


    /// <summary>
    /// Get the scale vector it would take to resize an object to the same
    /// size as a tile
    /// </summary>
    /// <param name="spriteInfo">Object to resize</param>
    /// <param name="tileSize">Size of tile to match</param>
    /// <returns>Scale vector that would set the size of an object to the
    /// size of a tile</returns>
    public static Vector3 GetScaleSpriteToTileSize(
        SpriteInfo spriteInfo, float tileSize)
    {
        float targetScaleX = 1;
        float targetScaleY = 1;

        float radius = spriteInfo.radius;

        if (radius >= 0)
        {
            radius
                = ((1 / spriteInfo.gameObject.transform.localScale.x)
                    * (spriteInfo.SpriteRenderer.bounds.size.x)
                    + (1 / spriteInfo.gameObject.transform.localScale.y)
                    * (spriteInfo.SpriteRenderer.bounds.size.y))
                / 2;
        }
        else if (radius == 0)
        {
            radius = tileSize / 2f;
        }
        else if (radius < 0)
        {
            radius = Mathf.Abs(radius);
        }

        // If still zero, set to default
        if (radius <= 0)
        {
            radius = 1;
        }


        targetScaleY = 2 * tileSize / radius;
        targetScaleX = 2 * tileSize / radius;

        return new Vector3(targetScaleX, targetScaleY, 1);
    }
}
