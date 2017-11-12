using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 1.0f,0.0f);

    public float speed = 0.0f;
    public float speedIncrement = 2.0f;
    public float maxSpeed = 12.0f;
    public float slowDown = 0.97f;
    public float angle;
    public float turnspeed;
    public bool useSlowdown = true;
  
    void Start () {
        
        transform.position = position;
        angle = transform.rotation.z;
	}
	
	
	void Update () {
        
        if (gameObject.tag == "Player1")
        {
            if (Input.GetKey(KeyCode.W))
            {
                position.y += .1f;
               // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position.y -= .1f;
                // speed += speedIncrement * Time.deltaTime;
            }
            else if (useSlowdown)
            {
                speed *= slowDown;
            }

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            else if (speed < 0.01f)
            {
                speed = 0.0f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                position.x += .1f;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                position.x -= .1f;
                //angle += turnspeed * Time.deltaTime;
            }

        }
        if (gameObject.tag == "Player2")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                position.y += .1f;
                // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                position.y -= .1f;
                // speed += speedIncrement * Time.deltaTime;
            }
            else if (useSlowdown)
            {
                speed *= slowDown;
            }

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            else if (speed < 0.01f)
            {
                speed = 0.0f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                position.x += .1f;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                position.x -= .1f;
                //angle += turnspeed * Time.deltaTime;
            }

        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            position += transform.rotation *velocity * speed * Time.deltaTime;
           
            transform.position = position;

        
      
    }
    public Vector3 GetDirection()
    {
        return (transform.rotation*velocity);
    }
}
