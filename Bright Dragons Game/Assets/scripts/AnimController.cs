using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public GameObject gameobject;

    public Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            anim.Play("attack");

            gameobject.gameObject.SetActive(true);
            StartCoroutine(wait());

        }
        if (Input.GetKeyDown("2"))
        {
            anim.Play("block");
        }
        if (Input.GetKeyDown("3"))
        {
            anim.Play("death");
        }
        
        
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
       
        gameobject.gameObject.SetActive(false);
    }

}

