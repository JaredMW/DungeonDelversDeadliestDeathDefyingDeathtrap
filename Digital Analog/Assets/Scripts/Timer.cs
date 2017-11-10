using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {
private static int count;
public Text texting;
	// Use this for initialization
	void Start () {
        Debug.Log("Timer Start");
        //count = 0;
	}
	
	// Update is called once per frame
	void Update () {
        count++;
        texting.text = count.ToString();
	}

    public void clicky()
    {

    }
}
