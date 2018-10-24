using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int moveSpeed;
    private Rigidbody2D myrigidbody;
	// Use this for initialization
	void Start () {
        myrigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxisRaw("Horizontal")>0.5f || Input.GetAxisRaw("Horizontal")<-0.5f)
        {
           // myrigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*moveSpeed, myrigidbody.velocity.y);
            transform.Translate(new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f));
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
          // myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, Input.GetAxisRaw("Vertical")*moveSpeed);

            transform.Translate(new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime));
        }
  if (Input.GetAxisRaw("Horizontal") <0.5f && Input.GetAxisRaw("Horizontal")>-0.5f)
        {
           // myrigidbody.velocity = new Vector2(0f, myrigidbody.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
           // myrigidbody.velocity = new Vector2(myrigidbody.velocity.x,0f);
        }


    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "enemy") ;

        Destroy(this.gameObject);
    }
}
