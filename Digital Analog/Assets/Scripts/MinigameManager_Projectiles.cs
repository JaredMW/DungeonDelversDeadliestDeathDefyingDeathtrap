using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameManager_Projectiles : Gamemanager {
    float radius = 7.15f;
    bool play = true;
    float endtimer = 5;
    //public Button button;            // Push to start
    //public Text text1, text2, text3; // Menu text

    //public MiniGame currentMinigame;

    //public List<GameObject> players;

    //public int numPlayers;

    //public List<GameObject> arenaPrefabs;   // Minigame arena prefabs
    //public GameObject arena;         // Current minigame arena + objects


    // Use this for initialization
    //void Start () {
    //arena.GetComponent<SpriteRenderer>().enabled = false;
    //foreach(GameObject player in players)
    //{
    //    player.SetActive(false);
    //    player.GetComponent<Movement>().enabled = false;
    //    player.GetComponentInChildren<SpriteRenderer>().enabled = false;
    //}
    //	}

    // Update is called once per frame
    void Update () {
        if (Timer.count == 0)
        {
            instructions.gameObject.SetActive(false);
        }
        else if (Timer.count < 5)
        {
            instructions.gameObject.SetActive(true);
        }
        else if (Timer.count > 5)
        {
            instructions.gameObject.SetActive(false);
            foreach (GameObject player in players)
            {
                player.SetActive(true);
                player.GetComponent<Movement>().enabled = true;
                player.GetComponentInChildren<SpriteRenderer>().enabled = true;

            }
        }
        if (Timer.count > 10 && play == true)
        {
            arena.gameObject.transform.localScale *= .999f;
            radius *= .999f;
        }
        for (int i=0;i<players.Count;i++)
        {
            float distance = 0;

            distance = Mathf.Sqrt((Mathf.Pow(players[i].transform.position.x- arena.transform.position.x, 2) + Mathf.Pow(players[i].transform.position.y - arena.transform.position.y, 2)));
            if (distance > radius)
            {
                players[i].SetActive(false);
                players[i].GetComponent<Movement>().enabled = false;
                players[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                players.Remove(players[i]);
                //Debug.Log("player removed");
                
            }
            //Debug.Log(distance);
         // foreach (GameObject player2 in players)
         // {
         //     player.GetComponent<Movement>().Iscolliding(player2);
         // }
        }

        if (players.Count <= 1)
        {

            play = false;
            GetComponent<Timer>().iscounting = false ;
            
            /*arena.gameObject.transform.localScale /=.999f;
            radius /= .999f;*/

            text1.gameObject.SetActive(true);

            text1.text = players[0].gameObject.tag.ToString() + " won!";
            endtimer = endtimer - .01f;
            if (endtimer <= 0)
            {
                Timer.count = 0;
                SceneManager.LoadScene("Scanning");
            }
        }
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
