using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choos_ur_weapon : MonoBehaviour
{
    public BoxCollider BoxCollider;
    public GameObject head_target;
    public GameObject Left_target;
    public GameObject right_target;
    public GameObject knife_target;
    public GameObject pistol_target;
    public GameObject shotgun_target;
    public float offset;
  [SerializeField] private bool isfocused;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();  
    }
    private void Update()
    {
        foucus();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag=="Player") 
        {
            foucus();
        }
    }

    private void foucus()
    {
        if (Input.GetKeyDown(KeyCode.E) && isfocused==false)
        {
            head_target.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + offset, this.gameObject.transform.position.z);
            isfocused= true;
        }else if (Input.GetKeyDown(KeyCode.E) && isfocused == true)
        {
            head_target.transform.position = Vector3.zero; 
            isfocused= false;
        }
        
       
    }
}


