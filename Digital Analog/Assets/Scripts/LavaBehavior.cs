using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simulates a moving liquid texture
public class LavaBehavior : MonoBehaviour {

    private Material material;
    public Vector2 textureOffset;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;

        // Generate a random offset in randomly the positive or negative x and y directions
        textureOffset.x = Random.Range(0.01f, 0.04f) * ((Random.Range(1, 3) * 2) - 3);
        textureOffset.y = Random.Range(0.01f, 0.04f) * ((Random.Range(1, 3) * 2) - 3);
    }
	
	// Update is called once per frame
	void Update ()
    {
        material.mainTextureOffset += textureOffset * Time.deltaTime;
	}
}
