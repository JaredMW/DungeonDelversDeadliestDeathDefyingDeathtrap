using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	public void StartGame()
    {
        Debug.Log("Started");
        SceneManager.LoadScene("MainMenu");
    }
    public void ScanScreen()
    {
        Debug.Log("Started");
        SceneManager.LoadScene("Scanning");
    }
    public void StartMinigame_Projectiles()
    {
        //Gamemanager.currentMinigame = MiniGame.Projectiles;
        Debug.Log("Started");
        SceneManager.LoadScene("Minigame_Projectiles");
    }
    public void StartMinigame_Lasers()
    {
        //Gamemanager.currentMinigame = MiniGame.Projectiles;
        Debug.Log("Started");
        SceneManager.LoadScene("LaserGame");
    }
    public void StartMinigame_Seeker()
    {
        //Gamemanager.currentMinigame = MiniGame.Projectiles;
        Debug.Log("Started");
        SceneManager.LoadScene("Minigame_Saw");
    }
}
