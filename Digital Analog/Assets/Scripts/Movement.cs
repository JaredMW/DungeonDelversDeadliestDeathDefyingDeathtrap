using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 0.0f,0.0f);
    private Vector3 inputVelocity = Vector3.zero;

    public float speed = 0.0f;
    public float speedIncrement = .75f;
    public float maxSpeed = 12.0f;
    public float slowDown = 0.97f;
    public float angle;
    public float turnspeed;
    public bool useSlowdown = true;

    private bool isDying = false;
    private float deathTimer;
    private float timeToDie;
    public float spinoutAngle = 1f;

    /// <summary>
    /// Is this player dying?
    /// </summary>
    public bool IsDying
    {
        get { return isDying; }
    }
  
    void Start ()
    {
        transform.position = position;
        angle = transform.rotation.z;
	}
	
	
	void Update () {

        // If not dying, move through input
        if (!isDying)
        {
            inputVelocity = Vector3.zero;

            if (gameObject.tag == "Player1")
            {
                // Moving upwards
                if (Input.GetKey(KeyCode.W))
                {
                    inputVelocity.y += speedIncrement;
                }

                // Moving downwards
                if (Input.GetKey(KeyCode.S))
                {
                    inputVelocity.y -= speedIncrement;
                }

                // Moving right
                if (Input.GetKey(KeyCode.D))
                {
                    inputVelocity.x += speedIncrement;
                }

                // Moving left
                if (Input.GetKey(KeyCode.A))
                {
                    inputVelocity.x -= speedIncrement;
                }

                // Clamp velocity to the maximum speed

            }

            else if (gameObject.tag == "Player2")
            {
                // Moving up
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    inputVelocity.y += speedIncrement;
                }

                // Moving down
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    inputVelocity.y -= speedIncrement;
                }

                // Moving right
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    inputVelocity.x += speedIncrement;
                }

                // Moving left
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    inputVelocity.x -= speedIncrement;
                }
            }

            else if (gameObject.tag == "Player3")
            {
                //Debug.Log(Input.GetJoystickNames());
                //Debug.Log("PLAYER3");
                float x = Mathf.Clamp(-(Input.GetAxis("Horizontal") * speedIncrement)/* * Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
                float y = Mathf.Clamp(-(Input.GetAxis("Vertical") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);

                inputVelocity.x += x;
                inputVelocity.y += y;
            }

            if (gameObject.tag == "Player4")
            {
                //Debug.Log(Input.GetJoystickNames());
                //Debug.Log("PLAYER3");
                float x = Mathf.Clamp(-(Input.GetAxis("Horizontal2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
                float y = Mathf.Clamp(-(Input.GetAxis("Vertical2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);

                inputVelocity.x += x;
                inputVelocity.y += y;
            }

            // Add input velocity to total velocity
            velocity += inputVelocity;

            // Apply friction
            if (useSlowdown)
            {
                velocity *= slowDown;
            }

            // Come to a complete halt
            if (velocity.x < .1 && velocity.x > -.1)
            {
                velocity.x = 0;
            }
            if (velocity.y < .1 && velocity.y > -.1)
            {
                velocity.y = 0;
            }
        }

        else
        {
            KillBehavior();
        }
        
        // Final translation of movement
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    public Vector3 GetDirection()
    {
        return (transform.rotation*velocity);
    }

    /// <summary>
    /// Resolve collisions
    /// </summary>
    /// <param name="coll">Other colliding object</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Contains("Player"))
        {
            coll.gameObject.GetComponent<Movement>().position += velocity * Time.deltaTime * 5;
            velocity *= -1;
            position += velocity * Time.deltaTime * 5;
            coll.gameObject.transform.position = coll.gameObject.GetComponent<Movement>().position;
            transform.position = position;
        }
    }

    /// <summary>
    /// Set the player in a dying state where they cannot move
    /// </summary>
    public void KillPlayer()
    {
        isDying = true;

        // If playing lasers, randomly determine spinout angle direction
        if (Gamemanager.currentMinigame == MiniGame.Lasers)
        {
            spinoutAngle *= (Random.Range(0, 2) == 1) ? 1 : -1;
            timeToDie = 1f;
            deathTimer = 0;
        }
    }

    /// <summary>
    /// Movement behavior for a dying player based on death type
    /// </summary>
    private void KillBehavior()
    {
        deathTimer += Time.deltaTime;

        if (deathTimer >= timeToDie)
        {
            Destroy(gameObject);
            return;
        }

        // Change death type depending on the minigame
        switch(Gamemanager.currentMinigame)
        {
            case MiniGame.Lasers:
                // Increase scale of the player
                transform.localScale *= (1.3f * Time.deltaTime);

                // Increase rotation
                angle += spinoutAngle * Time.deltaTime;

                // Increase offset from parent
                transform.GetChild(0).localPosition += new Vector3(.3f, .3f) * Time.deltaTime;
                break;

            default:
                break;
        }
    }
}
