using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinigameManager_Projectiles))]

/// <summary>
/// Manager class for creating projectiles
/// </summary>
public class ProjectileSpawning : MonoBehaviour {
    
    public GameObject projectilePrefab;
    public static List<GameObject> activeProjectiles;
    public float projectileSpeed = .5f;
    public float speedVariation = 2.5f;

    public float arenaTargetPercentage = .25f;
    private MinigameManager_Projectiles minigameManager;

    ///<summary>
    ///How often the projectiles should spawn
    ///</summary>
    public float spawnTimer = 1.0f;
    private float timer = 0.0f;

	// Use this for initialization
	void Start () {
        minigameManager = gameObject.GetComponent<MinigameManager_Projectiles>();
        activeProjectiles = new List<GameObject>();
        ScreenManager.CalculateScreen();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTimer)
        {
            timer -= spawnTimer;

            // If lagging, don't let bug out pls
            if (timer >= spawnTimer)
            {
                timer = 0;
            }

            SpawnProjectile();

            spawnTimer *= .997f;

            projectileSpeed += 1f;
        }
	}

    /// <summary>
    /// Instantiate an instance of the projectiles and set it flying somewhere
    /// </summary>
    void SpawnProjectile()
    {
        bool spawnTopOrBottom = Random.Range(0, 2) == 0 ? true : false; // If false, spawning on left or right

        float spawnX = spawnTopOrBottom ?
            Random.Range(ScreenManager.Left, ScreenManager.Right)   // If spawning on top, get a point between left and right
            : (Random.Range(0, 2) == 0 ? ScreenManager.Left - .2f : ScreenManager.Right + .2f); // If not, randomly pick whether spawning on left or right

        float spawnY = spawnTopOrBottom ?
            (Random.Range(0, 2) == 0 ? ScreenManager.Bottom - .2f : ScreenManager.Top + .2f) // If so, randomly pick whether spawning on top or bottom
            : Random.Range(ScreenManager.Bottom, ScreenManager.Top);    // If not, get a point between bottom and top

        Vector3 spawnLocation = new Vector3(spawnX, spawnY, -1);

        float targetX = minigameManager.arena.transform.position.x + ((minigameManager.Radius * arenaTargetPercentage) * (Random.Range(1, 3) * 2 - 3));
        float targetY = minigameManager.arena.transform.position.y + ((minigameManager.Radius * arenaTargetPercentage) * (Random.Range(1, 3) * 2 - 3));

        Vector3 target = new Vector3(targetX, targetY, -1);


        activeProjectiles.Add(
            Instantiate(
                projectilePrefab,
                spawnLocation,
                Quaternion.Euler(target - spawnLocation)));

        activeProjectiles[activeProjectiles.Count - 1].GetComponent<ProjectileBehavior>().Velocity
            = (target-spawnLocation).normalized * (projectileSpeed + Random.Range(0f, speedVariation) * (Random.Range(1, 3) * 2 - 3));
        //activeProjectiles[activeProjectiles.Count - 1].GetComponent<ProjectileBehavior>().target = target;
    }
}