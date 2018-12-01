using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyControl : MonoBehaviour
{

    public int chaseRange;
    public float speed;
    private Transform target;
    private float distance;
   
    

	// Use this for initialization
	void Start ()
    {
        // set the target to be the player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        // calculate the distance between the enemy and the target
        distance = Vector2.Distance(transform.position, target.position);

        //if the player is close enough to the enemy, chase
        if(distance <= chaseRange)
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        speed = 3;

    }

}
