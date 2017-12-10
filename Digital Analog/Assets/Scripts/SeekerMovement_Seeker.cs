using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMovement_Seeker : MonoBehaviour {

    public GameObject target;
    public Vector3 seek;
    public Sprite blood;
    public float speed;
    public int count;
    public bool single;
    public float speedX;
    public float speedY;

    // Use this for initialization
    void Start () {
        speed = 0.05f;
        
        speedX = Random.Range(-1f, 1f);
        speedY = Random.Range(-1f, 1f);
        while(speedX == 0)
        {
            speedX = Random.Range(-1f, 1f);
        }
        while (speedY == 0)
        {
            speedY = Random.Range(-1f, 1f);
        }
        seek.x += speedX;
        seek.y += speedY;
        seek.Normalize();
        seek *= speed;
    }
	
	// Update is called once per frame
	void Update () {
        count++;
        if(count > 120)
        {
            speed += 0.03f;
            count = 0;
        }
        if (single == false)
        {
            Vector3 tarpos = target.transform.position;
            Vector3 pos = transform.position;
            seek = tarpos - pos;
            if (seek.x > speed)
            {
                seek.x = speed;
            }
            if (seek.x < -speed)
            {
                seek.x = -speed;
            }
            if (seek.y > speed)
            {
                seek.y = speed;
            }
            if (seek.y < -speed)
            {
                seek.y = -speed;
            }
            transform.position += seek;
        }
        else
        {
            if (transform.position.x <= -15.1)
            {
                seek.x = -seek.x;
            }

            if (transform.position.y <= -18.68)
            {
                seek.y = -seek.y;
            }

            if (transform.position.x >= 15.15)
            {
                seek.x = -seek.x;
            }

            if (transform.position.y >= -3.3)
            {
                seek.y = -seek.y;
            }
            seek.Normalize();
            seek = seek * (speed*4);
            transform.position += seek;
        }
        transform.Rotate(Vector3.forward * 730 * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D coll) { 
        if (coll.gameObject.tag.Contains("Player")) { 
            GameObject.FindGameObjectWithTag("Saw").GetComponent<MiniGameManager_Seeker>().currentPlayers.Remove(coll.gameObject);
            coll.gameObject.GetComponent<Movement_Seeker>().enabled = false;
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            coll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = blood;
            //coll.gameObject.SetActive(false);
        } 
    }
}
