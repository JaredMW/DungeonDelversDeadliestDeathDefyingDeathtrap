using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMovement_Seeker : MonoBehaviour {

    public GameObject target;
    public Vector3 seek;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tarpos = target.transform.position;
        Vector3 pos = transform.position;
        seek = tarpos - pos;
        if (seek.x > 0.05)
        {
            seek.x = 0.05f;
        }
        if (seek.x < -0.05)
        {
            seek.x = -0.05f;
        }
        if (seek.y > 0.05)
        {
            seek.y = 0.05f;
        }
        if (seek.y < -0.05)
        {
            seek.y = -0.05f;
        }
        transform.position += seek;
	}

    void OnCollisionEnter2D(Collision2D coll) { 
        if (coll.gameObject.tag.Contains("Player")) { 
            GameObject.FindGameObjectWithTag("Saw").GetComponent<MiniGameManager_Seeker>().currentPlayers.Remove(coll.gameObject);
            coll.gameObject.GetComponent<Movement_Seeker>().enabled = false;
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            coll.gameObject.SetActive(false);
        } 
    }
}
