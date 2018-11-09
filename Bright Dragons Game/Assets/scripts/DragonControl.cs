using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonControl : MonoBehaviour {

    public GameObject player;
    private Rigidbody2D body;
    public float speed;
    public int dragonHealth = 3;
    public Transform enemy;
    public int chaseRange;
    private Func<Rigidbody2D> DragBod;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > player.transform.position.x + 1.5f)
        {
            Vector2 toTarget = player.transform.position - transform.position;


            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.x < player.transform.position.x - 1.5f)
        {
            Vector2 toTarget = player.transform.position - transform.position;
   

            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.y < player.transform.position.y - 1.5f)
        {
            Vector2 toTarget = player.transform.position - transform.position;


            transform.Translate(toTarget * speed * Time.deltaTime);
        }
        else if (transform.position.y > player.transform.position.y + 1.5f)
        {
            Vector2 toTarget = player.transform.position - transform.position;


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
        float distance = Vector2.Distance(transform.position, enemy.position);

        //if the player is close enough to the enemy chase
        if (distance <= chaseRange)
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);

        if (dragonHealth <= 0 && distance <= chaseRange)
        {
            //DragBod = this.GetComponent<Rigidbody2D>;
            this.GetComponent<Rigidbody2D>().isKinematic = false;
            speed = 0;
        }


    }

}

