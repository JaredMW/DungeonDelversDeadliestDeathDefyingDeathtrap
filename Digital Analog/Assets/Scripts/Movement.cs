using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 0.0f,0.0f);

    public float speed = 0.0f;
    public float speedIncrement = .75f;
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
                velocity.y += speedIncrement;
               // position.y += .1f;
               // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity.y -= speedIncrement;
               // position.y -= .1f;
                // speed += speedIncrement * Time.deltaTime;
            }
           
            if (Input.GetKey(KeyCode.D))
            {
                velocity.x += speedIncrement;
               // position.x += .1f;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x -= speedIncrement;
               // position.x -= .1f;
                //angle += turnspeed * Time.deltaTime;
            }
          

        }
        if (gameObject.tag == "Player2")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
               velocity.y += speedIncrement;
                // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
               velocity.y -= speedIncrement;
                // speed += speedIncrement * Time.deltaTime;
            }
           
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity.x += speedIncrement;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity.x -= speedIncrement;
                //angle += turnspeed * Time.deltaTime;
            }
            

        }
        if (gameObject.tag == "Player3")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal") * speedIncrement)/* * Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            //float x = (Input.GetAxis("Hotizontal") != 0 ? Time.deltaTime * 1 : 0);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            //gameObject.transform.Translate(x, y, 0);
            velocity.x += x;
            velocity.y += y;
        }
        if (gameObject.tag == "Player4")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical2") * speedIncrement) /*Time.deltaTime * 150.0f) / 3.25f*/, -speedIncrement, speedIncrement);
            //gameObject.transform.Translate(x, y, 0);
            velocity.x += x;
            velocity.y += y;
        }
        if (useSlowdown)
        {
            velocity *= slowDown;
        }

        if (velocity.x < .1 && velocity.x > -.1)
        {
            velocity.x = 0;
        }
        if (velocity.y < .1 && velocity.y > -.1)
        {
            velocity.y = 0;
        }
        
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    public Vector3 GetDirection()
    {
        return (transform.rotation*velocity);
    }

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
