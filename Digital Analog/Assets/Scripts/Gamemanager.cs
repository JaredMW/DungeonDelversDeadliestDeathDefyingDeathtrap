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

    public MiniGame currentMinigame;

    public List<GameObject> playerPrefabs;
    public List<GameObject> startPositions;
    public int numPlayers;

    public List<GameObject> arenaPrefabs;   // Minigame arena prefabs
    public GameObject arena;         // Current minigame arena + objects


    // Use this for initialization
    void Start () {
        arena.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startgame()
    {
        // Show the arena and hide menu text
        arena.GetComponent<SpriteRenderer>().enabled = true;
        button.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);

        // Instantiate player prefabs at their corresponding start positions


        // Instantiate the arena corresponding with this minigame
        arena = Instantiate(arenaPrefabs[(int)currentMinigame]);
    }
}
