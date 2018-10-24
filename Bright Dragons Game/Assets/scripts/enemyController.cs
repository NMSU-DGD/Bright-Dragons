using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyControl : MonoBehaviour
{
    public float speed;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {


        print("position 1" + target.position);
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        print("position 2" + target.position);
    }

}