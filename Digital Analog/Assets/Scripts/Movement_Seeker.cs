using UnityEngine;
using System.Collections;

public class Movement_Seeker : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 inputVelocity = Vector3.zero;

    public float speed = 0.0f;
    public float speedIncrement = .75f;
    public float maxSpeed = 12.0f;
    public float slowDown = 0.97f;
    public float angle;
    public float turnspeed;
    public bool useSlowdown = true;

    void Start()
    {
        transform.position = position;
        angle = transform.rotation.z;
    }


    void Update()
    {
        inputVelocity = Vector3.zero;

        if (gameObject.tag == "Player1")
        {
            // Moving upwards
            if (Input.GetKey(KeyCode.W))
            {
                if (transform.position.y <= -3.3)
                {
                    inputVelocity.y += speedIncrement;
                }
            }

            // Moving downwards
            if (Input.GetKey(KeyCode.S))
            {
                if (transform.position.y >= -18.68)
                {
                    inputVelocity.y -= speedIncrement;
                }
            }

            // Moving right
            if (Input.GetKey(KeyCode.D))
            {
                if (transform.position.x <= 15.15)
                {
                    inputVelocity.x += speedIncrement;
                }
            }

            // Moving left
            if (Input.GetKey(KeyCode.A))
            {
                if (transform.position.x >= -15.1)
                {
                    inputVelocity.x -= speedIncrement;
                }
            }
            if (transform.position.x <= -15.1)
            {
                position.x = -15f;
            }

            if (transform.position.y <= -18.68)
            {
                position.y = -18.58f;
            }

            if (transform.position.x >= 15.15)
            {
                position.x = 15.05f;
            }

            if (transform.position.y >= -3.3)
            {
                position.y = -3.2f;
            }

            // Clamp velocity to the maximum speed

        }

        else if (gameObject.tag == "Player2")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.position.y <= -3.3)
                {
                    inputVelocity.y += speedIncrement;
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (transform.position.y >= -18.68)
                {
                    inputVelocity.y -= speedIncrement;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x <= 15.15)
                {
                    inputVelocity.x += speedIncrement;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x >= -15.1)
                {
                    inputVelocity.x -= speedIncrement;
                }
            }
            if (transform.position.x <= -15.1)
            {
                position.x = -15;
            }

            if (transform.position.y <= -18.68)
            {
                position.y = -18.58f;
            }

            if (transform.position.x >= 15.15)
            {
                position.x = 15.05f;
            }

            if (transform.position.y >= -3.3)
            {
                position.y = -3.2f;
            }
        }

        else if (gameObject.tag == "Player3")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal") * speedIncrement)/* * Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);

            if (transform.position.x >= -15.1 && transform.position.x <= 15.15)
            {
                inputVelocity.x += x;
            }

            if (transform.position.y <= -3.3 && transform.position.y >= -18.68)
            {
                inputVelocity.y += y;
            }

            if (transform.position.x <= -15.1)
            {
                position.x = -15f;
            }

            if (transform.position.y <= -18.68)
            {
                position.y = -18.58f;
            }

            if (transform.position.x >= 15.15)
            {
                position.x = 15.05f;
            }

            if (transform.position.y >= -3.3)
            {
                position.y = -3.4f;
            }
        }

        if (gameObject.tag == "Player4")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);

            if (transform.position.x >= -15.1 && transform.position.x <= 15.15)
            {
                inputVelocity.x += x;
            }

            if (transform.position.y <= -3.3 && transform.position.y >= -18.68)
            {
                inputVelocity.y += y;
            }

            if (transform.position.x <= -15.1)
            {
                position.x = -15f;
            }

            if (transform.position.y <= -18.68)
            {
                position.y = -18.58f;
            }

            if (transform.position.x >= 15.15)
            {
                position.x = 15.05f;
            }

            if (transform.position.y >= -3.3)
            {
                position.y = -3.4f;
            }
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

        // Final translation of movement
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    public Vector3 GetDirection()
    {
        return (transform.rotation * velocity);
    }

    /// <summary>
    /// Resolve collisions
    /// </summary>
    /// <param name="coll">Other colliding object</param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Contains("Player"))
        {
            coll.gameObject.GetComponent<Movement_Seeker>().position += velocity * Time.deltaTime * 5;
            velocity *= -1;
            position += velocity * Time.deltaTime * 5;
            coll.gameObject.transform.position = coll.gameObject.GetComponent<Movement_Seeker>().position;
            transform.position = position;
        }
    }
}

