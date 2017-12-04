using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the spawning of laser objects for the laser minigame
/// </summary>
public class LaserSpawner : MonoBehaviour {

    // Fields
    public GameObject laserPrefab;
    public GameObject safetySquarePrefab;
    public float laserSpawnSpeed = 1f;  // Relative to the vertical laser
    public float laserSpeedIncreaseRate = 1.1f;
    public float borderBuffer = .2f;

    private List<GameObject> activeLasers;
    private List<GameObject> activeSquares;
    private List<float> activeSquareTimers;
    private List<float> squareTimerLengths;
    
    /// <summary>
    /// How often the lasers should spawn, in seconds
    /// </summary>
    public float laserSpawnTime = 3f;
    public float laserSpawnDecayRate = .95f;
    private float spawnTimer = 0f;

    public float timeUntilFirstSpawn = .5f;
    private bool firstSpawned = false;

    public float safetySquareWidth = 2f;
    public float safetySquareShrinkRate = .95f;
    public float initialSquareLifetime = 1.5f;
    public float squareLifetimeDecayRate = .97f;
    private float currentSquareLifetime;
    private float minimumSquareSize;

    private Vector3 lastSpawnLocation;
    public float maxSafetySquareDistance = 15f;

    private LaserMiniGame minigameManager;


	// Use this for initialization
	void Start ()
    {
        // Make sure spawn rate is not negative or 0
		if (laserSpawnTime <= 0)
        {
            laserSpawnTime = 3f;
        }
        if (laserSpeedIncreaseRate <= 1)
        {
            laserSpeedIncreaseRate = 1.1f;
        }
        if (laserSpawnDecayRate > 1 || laserSpawnDecayRate <= 0)
        {
            laserSpawnDecayRate = .97f;
        }

        minimumSquareSize = (Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y
            - Camera.main.transform.position.y) / GetComponent<BackgroundGenerator>().mapHeight;

        //activeLasers = new List<GameObject>();
        activeSquares = new List<GameObject>();
        activeSquareTimers = new List<float>();
        squareTimerLengths = new List<float>();

        currentSquareLifetime = initialSquareLifetime;

        minigameManager = GetComponent<LaserMiniGame>();


        // Start by spawning a laser
        //SpawnSafetySquares();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update the spawn timer
        spawnTimer += Time.deltaTime;

        if (!firstSpawned && spawnTimer >= timeUntilFirstSpawn)
        {
            spawnTimer -= timeUntilFirstSpawn;
            while (spawnTimer >= timeUntilFirstSpawn)
            {
                spawnTimer -= timeUntilFirstSpawn;
            }

            SpawnSafetySquares();
            firstSpawned = true;
        }

        // Update all safety square timers; if their time is up, spawn lasers
        for (int i = 0; i < activeSquareTimers.Count; i++)
        {
            activeSquareTimers[i] += Time.deltaTime;

            // Time to spawn the laser
            if (activeSquareTimers[i] > squareTimerLengths[i])
            {
                // Remove the timers
                activeSquareTimers.RemoveAt(i);
                squareTimerLengths.RemoveAt(i);

                // Spawn a laser at the square's location with the square's size
                SpawnLasers(activeSquares[i].transform.position,
                    activeSquares[i].GetComponent<SpriteRenderer>().bounds.size.x);

                // Remove and destroy the square
                Destroy(activeSquares[i]);
                activeSquares.RemoveAt(i);

                // Adjust the indexer
                i--;
            }
        }

        // If it's time to spawn a laser, reset the timer and spawn a safety square
        if (spawnTimer >= laserSpawnTime)
        {
            spawnTimer -= laserSpawnTime;

            // Backup - make sure that the spawn timer is less than the spawn rate
            if (spawnTimer > laserSpawnTime)
            {
                spawnTimer = 0;
            }

            // Spawn safety square
            SpawnSafetySquares();

            // Reduce the size of the safety square as long as it's greater than the minimum size (1 tile)
            if (safetySquareWidth > minimumSquareSize)
            {
                safetySquareWidth *= .95f;
                if (safetySquareWidth < minimumSquareSize)
                {
                    safetySquareWidth = minimumSquareSize;
                }

                // Shorten the lifespan of the safety square
                currentSquareLifetime *= squareLifetimeDecayRate;
            }
        }
	}

    /// <summary>
    /// Choose a safe zone square location and show it
    /// </summary>
    private void SpawnSafetySquares()
    {
        Vector3 squareSpawnScale = BackgroundGenerator.GetScaleSpriteToTileSize(safetySquarePrefab.GetComponent<SpriteInfo>(), safetySquareWidth);

        // Create a timer for this square
        squareTimerLengths.Add(currentSquareLifetime);
        activeSquareTimers.Add(0f);

        // Choose a random spawn location within the buffer of the screen
        float spawnX = Random.Range(ScreenManager.Left + (safetySquareWidth / 2) + borderBuffer, ScreenManager.Right - (safetySquareWidth / 2) - borderBuffer);
        float spawnY = Random.Range(ScreenManager.Bottom + (safetySquareWidth / 2) + borderBuffer, ScreenManager.Top - (safetySquareWidth / 2) - borderBuffer);

        // Make sure the players can reach the next square in time
        if (lastSpawnLocation != null)
        {
            while ((new Vector3(spawnX, spawnY) - lastSpawnLocation).sqrMagnitude > maxSafetySquareDistance * maxSafetySquareDistance)
            {
                spawnX = Random.Range(ScreenManager.Left + (safetySquareWidth / 2) + borderBuffer, ScreenManager.Right - (safetySquareWidth / 2) - borderBuffer);
                spawnY = Random.Range(ScreenManager.Bottom + (safetySquareWidth / 2) + borderBuffer, ScreenManager.Top - (safetySquareWidth / 2) - borderBuffer);
            }
        }

        // Instantiate the square
        activeSquares.Add(
            Instantiate(
                safetySquarePrefab,
                new Vector3(spawnX, spawnY, -.5f),
                Quaternion.identity));

        // Resize the square & configure the lifetime of it
        activeSquares[activeSquares.Count - 1].transform.localScale = squareSpawnScale;
        activeSquares[activeSquares.Count - 1].GetComponent<SafetySquare>().lifetimeLength = currentSquareLifetime;

        maxSafetySquareDistance *= laserSpawnDecayRate; //1f - ((laserSpeedIncreaseRate - 1) % 1);
    }

    /// <summary>
    /// Spawn 2 sets of lasers about a location
    /// </summary>
    /// <param name="location">Location where to spawn lasers</param>
    /// <param name="gapLength">Size of gap between lasers</param>
    private void SpawnLasers(Vector3 location, float gapLength)
    {
        //Debug.Log(gapLength);
        //Debug.Log(location);
        //GameObject newHorizLaser = new GameObject();
        //newHorizLaser.name = "Horizontally-Moving Laser Parent";

        //GameObject newVertLaser = new GameObject();
        //newVertLaser.name = "Vertically-Moving Laser Parent";


        // Randomly determine whether to spawn the laser on the top or bottom of the screen
        bool topOrBottom = Random.Range(0, 2) == 0 ? true : false;  // True = top; false = bottom

        // Randomly determine whether to spawn the second laser on the left or right of the screen
        bool leftOrRight = Random.Range(0, 2) == 0 ? true : false;  // True = left; false = right

        // Spawn the horizontally-moving laser just off screen
        float x_HorizontalSpawn = leftOrRight ? ScreenManager.Left - .3f : ScreenManager.Right + .3f;
        float y_HorizontalSpawn = location.y;

        // Spawn the vertically-moving laser just off screen
        float x_VerticalSpawn = location.x;
        float y_VerticalSpawn = topOrBottom ? ScreenManager.Top + .3f : ScreenManager.Bottom - .3f;


        float width = laserPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float height = laserPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float worldScreenHeight = (Camera.main.orthographicSize * 2f) - (borderBuffer * 2);
        float worldScreenWidth = worldScreenHeight / (ScreenManager.Top - ScreenManager.Bottom - borderBuffer * 2) * (ScreenManager.Right - ScreenManager.Left - borderBuffer * 2)/*Screen.height * Screen.width*/;

        float topHeight = (ScreenManager.Top - borderBuffer) - (location.y + gapLength / 2);
        float bottomHeight = (location.y - gapLength / 2) - (ScreenManager.Bottom + borderBuffer);
        float leftWidth = (location.x - gapLength / 2) - (ScreenManager.Left + borderBuffer);
        float rightWidth = (ScreenManager.Right - borderBuffer) - (location.x + gapLength / 2);

        //float xScreenScale = worldScreenWidth / width;
        //float yScreenScale = worldScreenHeight / height;

        float topScale = topHeight / height;
        float bottomScale = bottomHeight / height;
        float leftScale = leftWidth / width;
        float rightScale = rightWidth / width;


        #region Horizontally-Moving Lasers
        // Get the scales for the bottom and top of the horizontally-moving laser
        //Vector3 firstLaserSpawnScale = BackgroundGenerator.GetScaleSpriteToTileSize(safetySquarePrefab.GetComponent<SpriteInfo>(),
        //    (location.y - (gapLength / 2)) - (ScreenManager.Bottom + borderBuffer)) / 2;
        //Vector3 secondLaserSpawnScale = BackgroundGenerator.GetScaleSpriteToTileSize(safetySquarePrefab.GetComponent<SpriteInfo>(),
        //    (ScreenManager.Top - borderBuffer) - (location.y + (gapLength / 2))) / 2;

        // Instantiate the bottom of the horizontally-moving laser
        GameObject bottomHorizontalLaser = Instantiate(
            laserPrefab,
            new Vector3(
                x_HorizontalSpawn,
                y_HorizontalSpawn - (((location.y - (gapLength / 2)) - (ScreenManager.Bottom + borderBuffer)) / 2) - gapLength / 2,
                -.5f),
            Quaternion.identity);

        //Debug.Log((((location.y - (gapLength)) - (ScreenManager.Bottom + borderBuffer)) / 2));

        // Scale it
        //bottomHorizontalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, firstLaserSpawnScale.y, 1);
        bottomHorizontalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, bottomScale, 1);

        //bottomHorizontalLaser.transform.parent = newHorizLaser.transform;

        // Instantiate the top of the horizontally-moving laser
        GameObject topHorizontalLaser = Instantiate(
            laserPrefab,
            new Vector3(
                x_HorizontalSpawn,
                y_HorizontalSpawn + (((ScreenManager.Top - borderBuffer) - (location.y + (gapLength / 2))) / 2) + gapLength / 2,
                -.5f),
            Quaternion.identity);

        // Scale it
        //topHorizontalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, secondLaserSpawnScale.y, 1);
        topHorizontalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, topScale, 1);

        //topHorizontalLaser.transform.parent = newHorizLaser.transform;
        #endregion


        #region Vertically-Moving Lasers
        // Get the scales for the left and right of the vertically-moving laser
        //firstLaserSpawnScale = BackgroundGenerator.GetScaleSpriteToTileSize(safetySquarePrefab.GetComponent<SpriteInfo>(),
        //    (location.x - (gapLength / 2)) - (ScreenManager.Left + borderBuffer)) / 2;
        //secondLaserSpawnScale = BackgroundGenerator.GetScaleSpriteToTileSize(safetySquarePrefab.GetComponent<SpriteInfo>(),
        //    (ScreenManager.Right - borderBuffer) - (location.x + (gapLength / 2))) / 2;

        // Instantiate the left of the vertically-moving laser
        GameObject leftVerticalLaser = Instantiate(
            laserPrefab,
            new Vector3(
                x_VerticalSpawn - (((location.x - (gapLength / 2)) - (ScreenManager.Left + borderBuffer)) / 2) - gapLength / 2,
                y_VerticalSpawn,
                -.5f),
            Quaternion.Euler(new Vector3(0, 0, 90)));

        // Scale it
        //leftVerticalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, firstLaserSpawnScale.y, 1);
        leftVerticalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, leftScale, 1);

        //leftVerticalLaser.transform.parent = newVertLaser.transform;

        // Instantiate the right of the vertically-moving laser
        GameObject rightVerticalLaser = Instantiate(
            laserPrefab,
            new Vector3(
                x_VerticalSpawn + (((ScreenManager.Right - borderBuffer) - (location.x + (gapLength / 2))) / 2) + gapLength / 2,
                y_VerticalSpawn,
                -.5f),
            Quaternion.Euler(new Vector3(0, 0, 90)));

        // Scale it
        rightVerticalLaser.transform.localScale = new Vector3(laserPrefab.transform.localScale.x, rightScale, 1);
        //rightVerticalLaser.transform.parent = newVertLaser.transform;
        #endregion


        // Setup laser movement
        LaserBehavior topHorizontalLaserBehavior = topHorizontalLaser.AddComponent<LaserBehavior>();
        LaserBehavior bottomHorizontalLaserBehavior = bottomHorizontalLaser.AddComponent<LaserBehavior>();
        LaserBehavior leftVerticalLaserBehavior = leftVerticalLaser.AddComponent<LaserBehavior>();
        LaserBehavior rightVerticalLaserBehavior = rightVerticalLaser.AddComponent<LaserBehavior>();

        topHorizontalLaserBehavior.minigameManager = minigameManager;
        bottomHorizontalLaserBehavior.minigameManager = minigameManager;
        leftVerticalLaserBehavior.minigameManager = minigameManager;
        rightVerticalLaserBehavior.minigameManager = minigameManager;

        leftVerticalLaserBehavior.velocity =
            new Vector3(
                0,
                topOrBottom ? -laserSpawnSpeed : laserSpawnSpeed);
        rightVerticalLaserBehavior.velocity = leftVerticalLaserBehavior.velocity;

        float horizontalSpeed = (ScreenManager.Right - ScreenManager.Left) * laserSpawnSpeed / (ScreenManager.Top - ScreenManager.Bottom);
        topHorizontalLaserBehavior.velocity =
            new Vector3(
                leftOrRight ? horizontalSpeed : -horizontalSpeed,
                0);
        bottomHorizontalLaserBehavior.velocity = topHorizontalLaserBehavior.velocity;


        // Increase the speed for the next laser spawn set
        laserSpawnSpeed *= laserSpeedIncreaseRate;
        laserSpawnTime *= laserSpawnDecayRate;
    }

    private void OnDrawGizmos()
    {
        if (activeSquares != null)
        {
            for (int i = 0; i < activeSquares.Count; i++)
            {
                Gizmos.DrawWireSphere(activeSquares[i].transform.position, maxSafetySquareDistance);
            }
        }
    }
}
