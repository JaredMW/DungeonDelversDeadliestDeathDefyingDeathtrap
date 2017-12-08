using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BackgroundGenerator))]

/// <summary>
/// A seeker minigame, where an entity will chase marked player(s) and
/// attempt to kill them
/// </summary>
public class MiniGameManager_Seeker : Gamemanager
{
    // Instances
    bool play = false;
    bool gameOver = false;
    public float arenaShrinkRate = .9987f;
    float radius = 7.15f;

    public float Radius
    {
        get { return radius; }
    }


    protected override void Start()
    {
        //Debug.Log("Movement DISABLED");
        base.Start();
        seeker.GetComponent<SeekerMovement_Seeker>().enabled = false;
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
            seeker.GetComponent<SeekerMovement_Seeker>().enabled = true;

            for (int i = 0; i < currentPlayers.Count; i++)
            {
                currentPlayers[i].SetActive(true);
                if (currentPlayers[i].GetComponent<Movement_Seeker>())
                {
                    currentPlayers[i].GetComponent<Movement_Seeker>().enabled = true;
                }
                currentPlayers[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
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
                seeker.gameObject.GetComponent<SeekerMovement_Seeker>().enabled = false;
                seeker.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
    /// Procedures to follow when starting the seeker minigame
    /// </summary>
    public override void StartMinigame()
    {
        base.StartMinigame();

        play = true;
        gameOver = false;
        int rand = Random.Range(0,currentPlayers.Count);
        seeker.gameObject.GetComponent<SeekerMovement_Seeker>().target = currentPlayers[rand];
        currentPlayers[rand].GetComponent<Movement_Seeker>().marked = true;
        Behaviour h = (Behaviour)currentPlayers[rand].GetComponent("Halo");
        h.enabled = true;

    }

    /// <summary>
    /// Procedures to follow when ending the seeker minigame
    /// </summary>
    protected override void EndMinigame()
    {
        base.EndMinigame();

        play = false;
        gameOver = true;
        GetComponent<Timer>().iscounting = false;
    }
}

