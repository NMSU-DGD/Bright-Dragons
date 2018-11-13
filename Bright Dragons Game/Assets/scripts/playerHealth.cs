using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{

    public float Health = 100;
    private float currentHealth;
    // Use this for initialization
    void Start()
    {
        currentHealth = Health;
    }

    private void Update()
    {
       // print(Health);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if(other.gameObject.CompareTag("enemy"))
        {
            TakeDamage(10);
            print(currentHealth);
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
    
}