using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Type of minigame
/// </summary>
public enum MiniGame
{
    Projectiles,
    Lasers,
    Seeker
}

/// <summary>
/// Base class for minigame management; also manages global game data
/// </summary>
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

    public float endTimer = 3.5f;


    // Use this for initialization
    protected virtual void Start () {
        startScreen.gameObject.SetActive(true);
        objectiveScreen.gameObject.SetActive(false);
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

    /// <summary>
    /// Set of procedures to call for all minigames when they begin
    /// </summary>
    public virtual void StartMinigame()
    {
        // Hide menu text and show the objective screen
        startScreen.SetActive(false);
        objectiveScreen.SetActive(true);

        // Start the timer
        GetComponent<Timer>().iscounting = true;

        // Recalculate the screen
        ScreenManager.CalculateScreen();
    }

    /// <summary>
    /// Set of procedures to follow for minigames when they end
    /// </summary>
    protected virtual void EndMinigame()
    {
        // If playing single player, show the time they survived for
        if (playercountstart == 1)
        {
            gameOverScreen.SetActive(true);
            gameOverText.text = "Score:\n"+ string.Format("{0:0.00}", Timer.count) + " seconds";
        }

        // If multiplayer, the last one standing wins
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
    /// Do a QUICK countdown during the game over screen.
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

    /// <summary>
    /// Player #1 is playing this minigame
    /// </summary>
    public void AddPlayer1()
    {
        currentPlayers.Add(players[0]);
        playercountstart++;
        Addplayer1.interactable = false;
    }

    /// <summary>
    /// Player #2 is playing this minigame
    /// </summary>
    public void AddPlayer2()
    {
        currentPlayers.Add(players[1]);
        playercountstart++;
        Addplayer2.interactable = false;
    }

    /// <summary>
    /// Player #3 is playing this minigame
    /// </summary>
    public void AddPlayer3()
    {
        currentPlayers.Add(players[2]);
        playercountstart++;
        Addplayer3.interactable = false;
    }

    /// <summary>
    /// Player #4 is playing this minigame
    /// </summary>
    public void AddPlayer4()
    {
        currentPlayers.Add(players[3]);
        playercountstart++;
        Addplayer4.interactable = false;
    }
}
