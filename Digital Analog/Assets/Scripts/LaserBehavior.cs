using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How a laser should behave - move straight across the screen, and if it touches the player, kill them
/// </summary>
public class LaserBehavior : MonoBehaviour {

    public Vector3 velocity;
    public LaserMiniGame minigameManager;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move the laser
        transform.position += velocity * Time.deltaTime;
        
        // If off-screen, delete
        //if (transform.position.x < ScreenManager.Left - .5f
        //    || transform.position.x > ScreenManager.Right + .5f
        //    || transform.position.y < ScreenManager.Bottom - .5f
        //    || transform.position.y > ScreenManager.Top + .5f)
        //{
        //    Destroy(gameObject);
        //}
    }

    /// <summary>
    /// If colliding with a player, kill them
    /// </summary>
    /// <param name="other">Collision to check with</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kill the player, remove this from the active projectiles and destroy itself
        if (other.gameObject.tag.Contains("Player") && !other.GetComponent<Movement>().IsDying)
        {
            //other.gameObject.GetComponent<Movement>().KillPlayer();
            //Debug.Log(minigameManager.currentPlayers);
            minigameManager.KillPlayer(minigameManager.currentPlayers.IndexOf(other.gameObject));
            //other.enabled = false;
        }
    }
}
