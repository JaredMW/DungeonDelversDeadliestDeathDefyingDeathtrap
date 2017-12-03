using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

/// <summary>
/// Behavior for a safety square - flash a few times, then become invisible
/// </summary>
public class SafetySquare : MonoBehaviour {
    
    // Instance fields
    public float lifetimeLength = 2;
    public int numFlashes = 2;
    private float timePerFlash;

    private float timer;
    private SpriteRenderer spriteRenderer;

    public float maxOpacity = 190f;
    private float alpha = 0f;
    private int flashesCompleted = 0;


    // Properties
    /// <summary>
    /// The overall length of this safety square over its lifetime
    /// </summary>
    public float LifetimeLength
    {
        get { return lifetimeLength; }
        set
        {
            if (value <= 0)
            {
                value = 1f;
            }

            // Validate the timer
            while (timer >= value)
            {
                timer -= value;
            }

            timePerFlash = lifetimeLength / numFlashes;
        }
    }

    /// <summary>
    /// The number of flashes over the lifetime of the square
    /// </summary>
    public int NumFlashes
    {
        get { return numFlashes; }
        set
        {
            if (numFlashes < 0)
            {
                numFlashes = 2;
            }

            timePerFlash = lifetimeLength / numFlashes;
        }
    }


    // METHODS
	// Use this for initialization
	void Start ()
    {
		if (lifetimeLength <= 0)
        {
            lifetimeLength = 1f;
        }

        if (numFlashes <= 0)
        {
            numFlashes = 2;
        }

        timePerFlash = lifetimeLength / numFlashes;

        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Update timer
        timer += Time.deltaTime;

        if (timer >= timePerFlash)
        {
            timer -= timePerFlash;
            if (timer > timePerFlash)
            {
                timer = 0;
            }

            flashesCompleted++;

            // Make invisible as long as it's done with its cycle
            if (flashesCompleted >= numFlashes)
            {
                alpha = 0;
                
                spriteRenderer.color = new Color(
                    spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                    0);
            }
        }

        // Keep flashing while it's not finished
        if (flashesCompleted < numFlashes)
        {
            // Set the opacity of the sprite as a function of time
            alpha = (-Mathf.Abs(((4 * maxOpacity * timer) - (timePerFlash * timePerFlash)) / (2 * timePerFlash)) + maxOpacity) / 255f;

            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                alpha);
        }
    }

    /// <summary>
    /// Draw the safety square
    /// </summary>
    private void OnDrawGizmos()
    {
        if (gameObject != null && spriteRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, spriteRenderer.bounds.size);
        }
    }
}
