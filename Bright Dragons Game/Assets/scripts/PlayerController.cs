using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int moveSpeed;
    public Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f));
            anim.Play("walk");
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f){
            transform.eulerAngles = new Vector3(0, 180, 0);
            transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal") * -moveSpeed * Time.deltaTime, 0f));
            anim.Play("walk");

        }
        
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
           
            transform.Translate(new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime));
            anim.Play("walk");
        } else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            
            transform.Translate(new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime));
            anim.Play("walk");
        }
    }
}
