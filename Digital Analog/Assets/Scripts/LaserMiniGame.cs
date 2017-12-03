using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BackgroundGenerator))]

/// <summary>
/// Minigame manager: Manages a minigame where lasers cross the screen,
/// and players must fit into a small gap to survive
/// </summary>
public class LaserMiniGame : Gamemanager {

    // Instances
    //bool play = false;
    //bool gameOver = false;
    LaserSpawner laserManager;

    protected override void Start()
    {
        base.Start();
        laserManager = GetComponent<LaserSpawner>();
        laserManager.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // After some players are selected to play, you can now select to start
        if (!play && !gameOver && playercountstart >= 1)
        {
            startbutton.interactable = true;
        }
        if (Timer.count == 0)
        {
            objectiveScreen.gameObject.SetActive(false);
        }

        // Show instructions
        else if (Timer.count < 4)
        {
            objectiveScreen.gameObject.SetActive(true);
        }

        // Time to start the game
        else if (Timer.count >= 4 && play)
        {
            objectiveScreen.gameObject.SetActive(false);

            for (int i = 0; i < currentPlayers.Count; i++)
            {
                currentPlayers[i].SetActive(true);
                currentPlayers[i].GetComponent<Movement>().enabled = true;
                currentPlayers[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
            }

            // Begin laser spawning
            laserManager.enabled = true;
        }
        
        // If playing singleplayer...
        if (playercountstart <= 1)
        {
            // If everyone is dead, end the minigame
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
    /// Procedures to follow when starting the Lasers minigame
    /// </summary>
    public override void StartMinigame()
    {
        base.StartMinigame();
        currentMinigame = MiniGame.Lasers;
    }

    /// <summary>
    /// Procedures to follow when ending the Lasers minigame
    /// </summary>
    protected override void EndMinigame()
    {
        base.EndMinigame();

        // Stop spawning lasers
        laserManager.enabled = false;
    }
}
