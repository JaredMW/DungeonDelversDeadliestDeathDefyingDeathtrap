using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

/// <summary>
/// Class to represent thebehavior of a projectile
/// </summary>
public class ProjectileBehavior : MonoBehaviour {
    
    private Vector3 velocity;
    //public Vector3 target;

    /// <summary>
    /// Get or set the velocity of the Projectile
    /// </summary>
    public Vector2 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = (Vector3)value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveProjectile();

        // If this is out of the screen
		if (CheckOutOfScreen())
        {
            // Remove this from the projectile list, and delete it
            ProjectileSpawning.activeProjectiles.Remove(gameObject);
            Destroy(gameObject);
        }

        //Debug.DrawLine(gameObject.transform.position, target);
	}

    /// <summary>
    /// Check to see if the projectile is off screen using the ScreenManager
    /// </summary>
    /// <returns>True if off screen</returns>
    private bool CheckOutOfScreen()
    {
        if (transform.position.x < ScreenManager.Left - .5f
            || transform.position.x > ScreenManager.Right + .5f
            || transform.position.y < ScreenManager.Bottom - .5f
            || transform.position.y > ScreenManager.Top + .5f)
        {
            Debug.Log("Out of screen");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Move the projectile based on velocity
    /// </summary>
    private void MoveProjectile()
    {
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// Handle collision resolution
    /// </summary>
    /// <param name="collision">Other colliding object</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        // Remove this from the active projectiles and destroy itself
        if (coll.gameObject.tag.Contains("Player"))
        {
            coll.gameObject.GetComponent<Movement>().velocity += velocity * 20;
            ProjectileSpawning.activeProjectiles.Remove(gameObject);
            Destroy(gameObject);
            Debug.Log("Projectile Destroyed - Player velocity: " + coll.gameObject.GetComponent<Movement>().velocity.magnitude);
        }
        else
        {

        }
    }
}
