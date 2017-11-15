using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {
    public static float count=0;
  
    public bool iscounting = false;
public Text texting;
	// Use this for initialization
	void Start () {
        Debug.Log("Timer Start");
        //count = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (iscounting)
        {
            count += Time.deltaTime;
      
        }
        texting.text = count.ToString();
	}
}
