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
  
    void Start ()
    {
        transform.position = position;
        angle = transform.rotation.z;
	}
	
	
	void Update () {
        inputVelocity = Vector3.zero;

        if (gameObject.tag == "Player1" || gameObject.tag == "Player2")
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
           
            // Moving left
            if (Input.GetKey(KeyCode.D))
            {
                inputVelocity.x += speedIncrement;
            }

            // Moving right
            if (Input.GetKey(KeyCode.A))
            {
                inputVelocity.x -= speedIncrement;
            }

            // Clamp velocity to the maximum speed

        }

        //else if (gameObject.tag == "Player2")
        //{
        //    if (Input.GetKey(KeyCode.UpArrow))
        //    {
        //       velocity.y += speedIncrement;
        //    }
        //    if (Input.GetKey(KeyCode.DownArrow))
        //    {
        //       velocity.y -= speedIncrement;
        //    }
           
        //    if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        velocity.x += speedIncrement;
        //    }
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        velocity.x -= speedIncrement;
        //    }
        //}

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
}
