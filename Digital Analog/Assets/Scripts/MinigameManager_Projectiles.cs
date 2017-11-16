using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameManager_Projectiles : Gamemanager {
    
    // Instances
    float radius = 7.15f;
    bool play = true;
    public float arenaShrinkRate = .9987f;


    // Update is called once per frame
    void Update () {
        if (Timer.count == 0)
        {
            //instructions.gameObject.SetActive(false);
            objectiveScreen.gameObject.SetActive(false);
        }
        else if (Timer.count < 4)
        {
            //instructions.gameObject.SetActive(true);
            objectiveScreen.gameObject.SetActive(true);
        }
        else if (Timer.count >= 4 && play)
        {
            //instructions.gameObject.SetActive(false);
            objectiveScreen.gameObject.SetActive(false);

            for (int i = 0; i < players.Count; i++)
            {
                players[i].SetActive(true);
                players[i].GetComponent<Movement>().enabled = true;
                players[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }
        if (Timer.count > 8.5f && play)
        {
            arena.gameObject.transform.localScale *= arenaShrinkRate;// Mathf.Clamp(arenaShrinkRate * Time.deltaTime, 0, .999f);
            radius *= arenaShrinkRate;// Mathf.Clamp(arenaShrinkRate * Time.deltaTime, 0, .999f);
        }
        for (int i = 0; i < players.Count; i++)
        {
            float distance = 0;

            distance = Mathf.Sqrt((Mathf.Pow(players[i].transform.position.x- arena.transform.position.x, 2)
                + Mathf.Pow(players[i].transform.position.y - arena.transform.position.y, 2)));

            if (distance > radius)
            {
                players[i].SetActive(false);
                players[i].GetComponent<Movement>().enabled = false;
                players[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                players.Remove(players[i]);
            }

            //Debug.Log(distance);
            //foreach (GameObject player2 in players)
            //{
            //    player.GetComponent<Movement>().Iscolliding(player2);
            //}
        }

        // End the minigame when 1 or less players are present
        if (players.Count <= 1 && play)
        {
            EndMinigame();
            GameOverCountdown();
        }
        // Countdown until the next minigame or until 
        else if (players.Count <= 1 && !play)
        {
            GameOverCountdown();
        }
    }

    // End the projectiles minigame
    protected override void EndMinigame()
    {
        base.EndMinigame();

        play = false;
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
        //foreach (GameObject player in players)
        //{
        //    player.SetActive(true);
        //    player.GetComponent<Movement>().enabled = true;
        //    player.GetComponentInChildren<SpriteRenderer>().enabled = true;

        //}
        // Instantiate player prefabs at their corresponding start positions


        // Instantiate the arena corresponding with this minigame

   // }
}
