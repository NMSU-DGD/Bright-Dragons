using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordController : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
      
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Destroy(other.gameObject);
        }

    }
   
    


}
