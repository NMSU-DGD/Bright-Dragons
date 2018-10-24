using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchWeapons : MonoBehaviour {
    public int currentWeapon = 0;
    public Transform[] weapons;
	// Use this for initialization
	void Start () {
        SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire3") > 0f)
            if (currentWeapon >= transform.childCount - 1)
                currentWeapon = 0;
        else
            currentWeapon++;
	}
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
   
  }
