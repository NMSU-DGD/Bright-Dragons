using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyControl : MonoBehaviour
{
    public Animator anim;
    public int chaseRange;
    public float speed;
    private Transform target;
    private float distance;
    public int hp;
    // Use this for initialization
    void Start()
    {
        // set the target to be the player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the distance between the enemy and the target
        distance = Vector2.Distance(transform.position, target.position);

        //if the player is close enough to the enemy chase
        if (distance <= chaseRange)
            //  Debug.Log(distance);
            if (distance > 0.5)
            {
                anim.Play("enemWalk");
            }
        Debug.Log(target.position - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
   
}
