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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}