using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 0.0f,0.0f);
    private Vector3 inputVelocity = Vector3.zero;
    private Vector3 forward;

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
    private bool dead = false;

    public bool canMove = true;

    public float spinoutAngle = 1f;
    private float spinoutX = 1f;
    private float spinoutY = 1f;
    private float spinoutSpeedVariation = .7f;

    /// <summary>
    /// Is this player dying?
    /// </summary>
    public bool IsDying
    {
        get { return isDying; }
    }

    /// <summary>
    /// Are they dead?
    /// </summary>
    public bool IsDead
    {
        get { return dead; }
    }
  
    void Start ()
    {
        transform.position = position;
        angle = transform.rotation.z;
	}

	void Update () {

        // If not dying, move through input
        if (canMove && !isDying)
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

            if (inputVelocity.sqrMagnitude != 0)
            {
                forward = inputVelocity.normalized;
                Debug.Log(forward);
                transform.up = forward;
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

        if(transform.position.x > ScreenManager.Right)
        {
            transform.position = new Vector3(ScreenManager.Right,transform.position.y,transform.position.z);
        }
        if (transform.position.x < ScreenManager.Left)
        {
            transform.position = new Vector3(ScreenManager.Left, transform.position.y, transform.position.z);
        }
        if (transform.position.y > ScreenManager.Top)
        {
            transform.position = new Vector3(transform.position.x, ScreenManager.Top, transform.position.z);
        }
        if (transform.position.y < ScreenManager.Bottom)
        {
            transform.position = new Vector3(transform.position.x, ScreenManager.Bottom, transform.position.z);
        }
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
        if(Gamemanager.currentMinigame != MiniGame.Lasers)
        {
            if (coll.gameObject.tag.Contains("Player"))
            {
                coll.gameObject.GetComponent<Movement>().position += velocity * Time.deltaTime * 5;
                //coll.gameObject.GetComponent<Movement>().velocity += velocity * Time.deltaTime * 20;
                velocity *= -1;
                position += velocity * Time.deltaTime * 5;
                coll.gameObject.transform.position = coll.gameObject.GetComponent<Movement>().position;
                transform.position = position;
            }
        }
    }

    /// <summary>
    /// Set the player in a dying state where they cannot move
    /// </summary>
    public void KillPlayer()
    {
        if (!isDying || !dead)
        {
            isDying = true;
            canMove = false;

            // If playing lasers, randomly determine spinout angle direction
            if (Gamemanager.currentMinigame == MiniGame.Lasers)
            {
                spinoutAngle *= (Random.Range(0, 2) == 1) ? 1 : -1;
                spinoutAngle += Random.Range(-10f, 100f);

                spinoutX = Random.Range(spinoutX - spinoutSpeedVariation, spinoutX + spinoutSpeedVariation);
                spinoutY = Random.Range(spinoutY - spinoutSpeedVariation, spinoutY + spinoutSpeedVariation);

                spinoutX *= Random.Range(0, 2) == 0 ? 1 : -1;
                spinoutY *= Random.Range(0, 2) == 0 ? 1 : -1;

                timeToDie = .25f;
                deathTimer = 0;
            }

            else
            {
                gameObject.SetActive(false);
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                enabled = false;
                velocity = Vector3.zero;
            }
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
            dead = true;
            gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        GetComponent<BoxCollider2D>().enabled = false;

        // Change death type depending on the minigame
        switch(Gamemanager.currentMinigame)
        {
            case MiniGame.Lasers:
                // Increase scale of the player
                transform.localScale *= (1.2f);

                // Increase rotation
                angle += spinoutAngle * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                // Increase offset from parent
                transform.GetChild(0).localPosition += new Vector3(spinoutX, spinoutY) * Time.deltaTime;
                break;

            default:
                break;
        }
    }
}
