using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1Manager : Gamemanager {
    float radius = 7.15f;
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
        if (Timer.count > 5)
        {
            arena.gameObject.transform.localScale *= .99999999f;
            radius *= .99999999f;
        }
        for (int i=0;i<players.Count;i++)
        {
            float distance = 0;

            distance = Mathf.Sqrt((Mathf.Pow(players[i].transform.position.x- arena.transform.position.x, 2) + Mathf.Pow(players[i].transform.position.y - arena.transform.position.y, 2)));
            if (distance >radius)
            {
                players[i].SetActive(false);
                players[i].GetComponent<Movement>().enabled = false;
                players[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                players.Remove(players[i]);
                Debug.Log("player removed");
                
            }
            //Debug.Log(distance);
         // foreach (GameObject player2 in players)
         // {
         //     player.GetComponent<Movement>().Iscolliding(player2);
         // }
        }

        if (players.Count == 0)
        {
            GetComponent<Timer>().iscounting = false ;
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
