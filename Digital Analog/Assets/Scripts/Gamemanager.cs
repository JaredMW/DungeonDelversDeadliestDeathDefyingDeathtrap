using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MiniGame
{
    Projectiles
}

public class Gamemanager : MonoBehaviour {
    
    public Button button;            // Push to start
    public Text text1, text2, text3; // Menu text
    public Image instructions;
    public MiniGame currentMinigame;
    
    public List<GameObject> players;
    
    public int numPlayers;

    public List<GameObject> arenaPrefabs;   // Minigame arena prefabs
    public GameObject arena;         // Current minigame arena + objects


    // Use this for initialization
    void Start () {
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);
        arena.GetComponent<SpriteRenderer>().enabled = false;
        foreach(GameObject player in players)
        {
            player.SetActive(false);
            player.GetComponent<Movement>().enabled = false;
            player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //foreach (GameObject player in players)
        //{
        //    if (Vector3.Distance(player.transform.position, arena.transform.position) > 7.15f)
        //    {
        //        player.SetActive(false);
        //        player.GetComponent<Movement>().enabled = false;
        //        player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //        players.Remove(player);
        //    }
        //}


        }

    public void StartMinigame()
    {
        // Show the arena and hide menu text
        arena.GetComponent<SpriteRenderer>().enabled = true;
        button.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(true);
        this.GetComponent<Timer>().iscounting = true;
       
        // Instantiate player prefabs at their corresponding start positions


        // Instantiate the arena corresponding with this minigame

    }
}
