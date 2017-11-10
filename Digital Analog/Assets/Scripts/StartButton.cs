using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	 public void StartGame()
    {
        Debug.Log("Started");
        SceneManager.LoadScene("TestScene");
    }
    public void StartGames()
    {
        Debug.Log("Started");
        SceneManager.LoadScene("Game");
    }
}
