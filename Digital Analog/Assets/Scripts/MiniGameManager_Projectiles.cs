using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ProjectileSpawning))]

/// <summary>
/// Manager of the projectiles minigame, where players must fight to
/// stay on a shrinking stage while avoiding fireballs that are coming
/// at them
/// </summary>
public class MiniGameManager_Projectiles : Gamemanager {

    // Instances
    #region Fields
    //private bool play = false;
    //private bool gameOver = false;
    public GameObject arena;
    public float arenaShrinkRate = .9987f;
    private float radius = 7.15f;

    private ProjectileSpawning spawnManager;
    #endregion

    /// <summary>
    /// Get the radius of the arena
    /// </summary>
    public float Radius
    {
        get { return radius; }
    }


    protected override void Start()
    {
        base.Start();

        // Show the arena & setup the spawn manager
        arena.GetComponent<SpriteRenderer>().enabled = false;
        spawnManager = GetComponent<ProjectileSpawning>();
    }


    // Update is called once per frame
    void Update () {
        // After some players are selected to play, you can now select to start
        if (!play && !gameOver && playercountstart >= 1)
        {
            startbutton.interactable = true;
        }
        if (Timer.count == 0)
        {
            //instructions.gameObject.SetActive(false);
            objectiveScreen.gameObject.SetActive(false);
        }

        // Show instructions
        else if (Timer.count < 4)
        {
            //instructions.gameObject.SetActive(true);
            objectiveScreen.gameObject.SetActive(true);
        }

        // Time to start the game
        else if (Timer.count >= 4 && play)
        {
            //instructions.gameObject.SetActive(false);
            objectiveScreen.gameObject.SetActive(false);
            spawnManager.enabled = true;

            for (int i = 0; i < currentPlayers.Count; i++)
            {
                currentPlayers[i].SetActive(true);
                currentPlayers[i].GetComponent<Movement>().enabled = true;
                currentPlayers[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }

        // Start shrinking arena
        if (Timer.count > 10f && play)
        {
            arena.gameObject.transform.localScale *= arenaShrinkRate;// Mathf.Clamp(arenaShrinkRate * Time.deltaTime, 0, .999f);
            radius *= arenaShrinkRate;// Mathf.Clamp(arenaShrinkRate * Time.deltaTime, 0, .999f);
        }

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            float distance = 0;

            distance = Mathf.Sqrt((Mathf.Pow(currentPlayers[i].transform.position.x- arena.transform.position.x, 2)
                + Mathf.Pow(currentPlayers[i].transform.position.y - arena.transform.position.y, 2)));

            // Kill the player if they run off the arena
            if (distance > radius)
            {
                KillPlayer(i);
            }
        }
        
        // If playing singleplayer...
        if (playercountstart <= 1)
        {
            // If there is nobody left standing, end the minigame
            if (currentPlayers.Count < 1 && play)
            {
                EndMinigame();
                GameOverCountdown();
            }

            // Countdown until the next minigame
            else if (currentPlayers.Count < 1 && !play && gameOver)
            {
                GameOverCountdown();
            }
        }

        // If playing multiplayer...
        else if (playercountstart > 1)
        {
            // End the minigame when 1 or less currentplayers are present
            if (currentPlayers.Count <= 1 && play)
            {
                EndMinigame();
                GameOverCountdown();
            }

            // Countdown until the next minigame
            else if (currentPlayers.Count <= 1 && !play && gameOver)
            {
                GameOverCountdown();
            }
        }
    }

    /// <summary>
    /// Start the Projectiles minigame
    /// </summary>
    public override void StartMinigame()
    {
        base.StartMinigame();
        currentMinigame = MiniGame.Projectiles;

        arena.GetComponent<SpriteRenderer>().enabled = true;

        if (ProjectileSpawning.activeProjectiles != null)
        {
            for (int i = 0; i < ProjectileSpawning.activeProjectiles.Count; i++)
            {
                Destroy(ProjectileSpawning.activeProjectiles[ProjectileSpawning.activeProjectiles.Count - 1]);
                ProjectileSpawning.activeProjectiles.RemoveAt(ProjectileSpawning.activeProjectiles.Count - 1);
                i--;
            }
        }
    }

    /// <summary>
    /// End the Projectiles minigame
    /// </summary>
    protected override void EndMinigame()
    {
        base.EndMinigame();

        spawnManager.enabled = false;
    }
}
