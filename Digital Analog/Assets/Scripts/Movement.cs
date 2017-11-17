using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity = new Vector3(0.0f, 0.0f,0.0f);

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
                velocity.y += .75f;
               // position.y += .1f;
               // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity.y -= .75f;
               // position.y -= .1f;
                // speed += speedIncrement * Time.deltaTime;
            }
           
            if (Input.GetKey(KeyCode.D))
            {
                velocity.x += .75f;
               // position.x += .1f;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x -= .75f;
               // position.x -= .1f;
                //angle += turnspeed * Time.deltaTime;
            }
          

        }
        if (gameObject.tag == "Player2")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
               velocity.y += .75f;
                // speed += speedIncrement * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
               velocity.y -= .75f;
                // speed += speedIncrement * Time.deltaTime;
            }
           
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity.x += .75f;
                // angle -= turnspeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity.x -= .75f;
                //angle += turnspeed * Time.deltaTime;
            }
            

        }
        if (gameObject.tag == "Player3")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f) / 3.25f, -.75f, .75f);
            //float x = (Input.GetAxis("Hotizontal") != 0 ? Time.deltaTime * 1 : 0);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical") * Time.deltaTime * 150.0f) / 3.25f, -.75f, .75f);
            //gameObject.transform.Translate(x, y, 0);
            velocity.x += x;
            velocity.y += y;
        }
        if (gameObject.tag == "Player4")
        {
            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log("PLAYER3");
            float x = Mathf.Clamp(-(Input.GetAxis("Horizontal2") * Time.deltaTime * 150.0f) / 3.25f, -.75f, .75f);
            float y = Mathf.Clamp(-(Input.GetAxis("Vertical2") * Time.deltaTime * 150.0f) / 3.25f, -.75f, .75f);
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
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            position += velocity * Time.deltaTime;
           
            transform.position = position;

     


    }
    public Vector3 GetDirection()
    {
        return (transform.rotation*velocity);
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "enemy")
        {
            velocity += (Vector3)coll.gameObject.GetComponent<ProjectileBehavior>().Velocity*15;
            position += velocity * Time.deltaTime * 5;
            transform.position = position;
        }
        else
        {
            coll.gameObject.GetComponent<Movement>().position += velocity * Time.deltaTime * 5;
            velocity *= -1;
            position += velocity * Time.deltaTime * 5;
            coll.gameObject.transform.position = coll.gameObject.GetComponent<Movement>().position;
            transform.position = position;
        }
        
      
         
        
    }
}
