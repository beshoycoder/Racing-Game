using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Player_controls : MonoBehaviour
{
    public GameObject Bullet;

    [SerializeField]private Transform turrent;
    [SerializeField]private Transform bulletSpawn;
     private float Curspeed, targetspeed, Rotspeed;
    [SerializeField] private float turrentRotSpeed= 10.0f;
    [SerializeField] private float MaxforwardSpeed=300.0f;
    [SerializeField] private float maxBackwardSpeed= -300.0f;

    [SerializeField] protected float Shootrate = 0.25f;
    protected float elapsedTime;

    void Start()
    {
        Rotspeed = 150.0f;
    }

    void Update()
    {
        UpdateWeapon();
        UpdateControl();
    }

    private void UpdateWeapon()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            if( elapsedTime >= Shootrate)
            {
                elapsedTime = 0;
                Instantiate(Bullet,bulletSpawn.transform.position,bulletSpawn.transform.rotation);
            }
        }
    }

    private void UpdateControl()
    {
        Plane playerPlane = new Plane(Vector3.up,
            transform.position + new Vector3(0, 0, 0));
        Ray RayCast =
            Camera.main.ScreenPointToRay(Input.mousePosition);
        float HitDit = 0;
        if(playerPlane.Raycast(RayCast, out HitDit))
        {
            Vector3 RayHitPoint = RayCast.GetPoint(HitDit);
            Quaternion targetRotation =
                Quaternion.LookRotation(RayHitPoint - transform.position);
            turrent.transform.rotation =
                Quaternion.Slerp(turrent.transform.rotation, targetRotation, Time.deltaTime * turrentRotSpeed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            targetspeed = MaxforwardSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetspeed = maxBackwardSpeed;
        }
        else
        {
            targetspeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -Rotspeed * Time.deltaTime,
            0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, Rotspeed * Time.deltaTime,
            0.0f);
        }
        //Determine current speed
        Curspeed = Mathf.Lerp(Curspeed, targetspeed, 7.0f *
        Time.deltaTime);

        transform.Translate(Vector3.forward * Time.deltaTime *
        Curspeed);
    }
}

