using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonControl : MonoBehaviour {

    public GameObject target;
    public GameObject player;
    private Rigidbody2D body;
    public float speed;
    public int dragonHealth = 3;
    
    public int chaseRange;
    private Func<Rigidbody2D> DragBod;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > target.transform.position.x + 1.5f)
        {
            Vector2 toTarget = target.transform.position - transform.position;


            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.x < target.transform.position.x - 1.5f)
        {
            Vector2 toTarget = target.transform.position - transform.position;
   

            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.y < target.transform.position.y - 1.5f)
        {
            Vector2 toTarget = target.transform.position - transform.position;


            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.y > target.transform.position.y + 1.5f)
        {
            Vector2 toTarget = target.transform.position - transform.position;


            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else
        {
            body.velocity = new Vector2(0, 0);
        }
    }

    void Update()
    {
        // calculate the distance between the enemy and the target
        //float distance = Vector2.Distance(transform.position, enemy.position);

        //if the player is close enough to the enemy chase
        //if (distance <= chaseRange)
          //  transform.position = Vector2.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);

        if (dragonHealth <= 0 && target != player)
        {
            //DragBod = this.GetComponent<Rigidbody2D>;
            this.GetComponent<Rigidbody2D>().isKinematic = false;
            speed = 0;
        }

        if (target == null)
        {
            target = player;
            dragonHealth = 3;
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {



        if (other.gameObject.tag == "Enemy")
        {
            if (target == null || target == player)
                target = other.gameObject;


        } 


    }

}

