using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MiniGame
{
    Projectiles
}

public class Gamemanager : MonoBehaviour {
    
    public Button button;            // Push to start
    public GameObject startScreen;   // Information/components of the menu
    public GameObject objectiveScreen;
    public GameObject gameOverScreen;
    public Text gameOverText;
    public Button Addplayer1, Addplayer2, Addplayer3, Addplayer4, startbutton;
    //public Image instructions;

    
    private static MiniGame currentMinigame;
    public List<GameObject> players; // The players in the game
    public List<GameObject> currentPlayers;
    public int playercountstart = 0;
    //public int numPlayers;

    public List<GameObject> arenaPrefabs;   // Minigame arena prefabs
    public GameObject arena;         // Current minigame arena + objects

    public float endTimer = 3.5f;


    // Use this for initialization
    protected virtual void Start () {
        startScreen.gameObject.SetActive(true);
        objectiveScreen.gameObject.SetActive(false);
        arena.GetComponent<SpriteRenderer>().enabled = false;
        startbutton.interactable = false;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(false);
            players[i].GetComponent<Movement>().enabled = false;
            players[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
       

    }

    // Setup the minigame
    protected virtual void StartMinigame()
    {
        // Show the arena and hide menu text
        arena.GetComponent<SpriteRenderer>().enabled = true;
        startScreen.SetActive(false);
        objectiveScreen.SetActive(true);
        GetComponent<Timer>().iscounting = true;

        // Instantiate player prefabs at their corresponding start positions
       


            // Instantiate the arena corresponding with this minigame

        }

    // End the minigame and declare a winner
    protected virtual void EndMinigame()
    {
        if (playercountstart == 1)
        {
            gameOverScreen.SetActive(true);
            gameOverText.text = "Score: "+ Timer.count;
        }
        else if (playercountstart > 1)
        {
            // Display game over information
            gameOverScreen.SetActive(true);
            gameOverText.text = currentPlayers[0].name + " won!";
        }
        // Disable movement for all players and immediately stop any current movement
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<Movement>().enabled = false;
            players[i].GetComponent<Movement>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Do a QUICK countdown at the game over screen.
    /// Then, transition to either the scanning screen or the next game in the minigame sequence.
    /// </summary>
    protected void GameOverCountdown()
    {
        // Return to the scanning screen or head to the next minigame
        endTimer -= 1 * Time.deltaTime;
        if (endTimer <= 0)
        {
            Timer.count = 0;
            SceneManager.LoadScene("Scanning");
        }
    }

    public void AddPlayer1()
    {
        currentPlayers.Add(players[0]);
        playercountstart++;
        Addplayer1.interactable = false;
    }
    public void AddPlayer2()
    {
        currentPlayers.Add(players[1]);
        playercountstart++;
        Addplayer2.interactable = false;
    }
    public void AddPlayer3()
    {
        currentPlayers.Add(players[2]);
        playercountstart++;
        Addplayer3.interactable = false;
    }
    public void AddPlayer4()
    {
        currentPlayers.Add(players[3]);
        playercountstart++;
        Addplayer4.interactable = false;
    }
}
