using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ProjectileSpawning))]

public class MiniGameManager_Projectiles : Gamemanager {
    
    // Instances
    float radius = 7.15f;
    bool play = false;
    bool gameOver = false;
    public float arenaShrinkRate = .9987f;

    private ProjectileSpawning spawnManager;

    public float Radius
    {
        get { return radius; }
    }


    protected override void Start()
    {
        base.Start();
        ScreenManager.CalculateScreen();
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

            if (distance > radius)
            {
                currentPlayers[i].SetActive(false);
                currentPlayers[i].GetComponent<Movement>().enabled = false;
                currentPlayers[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                currentPlayers.Remove(currentPlayers[i]);
            }

            //Debug.Log(distance);
            //foreach (GameObject player2 in currentplayers)
            //{
            //    player.GetComponent<Movement>().Iscolliding(player2);
            //}
        }
        
        if (playercountstart <= 1)
        {
            if (currentPlayers.Count < 1 && play)
            {
                EndMinigame();
                GameOverCountdown();
            }
            // Countdown until the next minigame or until 
            else if (currentPlayers.Count < 1 && !play && gameOver)
            {
                GameOverCountdown();
            }
        }
        else if (playercountstart > 1)
        {
            // End the minigame when 1 or less currentplayers are present
            if (currentPlayers.Count <= 1 && play)
            {
                EndMinigame();
                GameOverCountdown();
            }
            // Countdown until the next minigame or until 
            else if (currentPlayers.Count <= 1 && !play && gameOver)
            {
                GameOverCountdown();
            }
        }
    }

    protected override void StartMinigame()
    {
        base.StartMinigame();

        play = true;
        gameOver = false;


    }

    // End the projectiles minigame
    protected override void EndMinigame()
    {
        base.EndMinigame();

        play = false;
        gameOver = true;
        GetComponent<Timer>().iscounting = false;
        
        //endTimer = endTimer - .01f;
        //if (endTimer <= 0)
        //{
        //    Timer.count = 0;
        //    SceneManager.LoadScene("Scanning");
        //}
    }
  //  public void StartMinigame()
   // {
        // Show the arena and hide menu text
        //arena.GetComponent<SpriteRenderer>().enabled = true;
        //button.gameObject.SetActive(false);
        //text1.gameObject.SetActive(false);
        //text2.gameObject.SetActive(false);
        //text3.gameObject.SetActive(false);
        //foreach (GameObject player in currentplayers)
        //{
        //    player.SetActive(true);
        //    player.GetComponent<Movement>().enabled = true;
        //    player.GetComponentInChildren<SpriteRenderer>().enabled = true;

        //}
        // Instantiate player prefabs at their corresponding start positions


        // Instantiate the arena corresponding with this minigame

   // }
}
