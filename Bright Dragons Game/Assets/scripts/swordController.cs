using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordController : MonoBehaviour
{

    public int moveSpeed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
       

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "enemy") ;

        Destroy(other.gameObject);
    }
}
