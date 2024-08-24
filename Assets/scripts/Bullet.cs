using System.Collections;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public GameObject Eploison;
    public float speed = 600.0f;
    public float lifetime = 3.0f;
    public int damage = 50;

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    
    void Update()
    {
        transform.position +=
            transform.forward * speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        Instantiate(Eploison, contactPoint.point,
            Quaternion.identity);
        Destroy(gameObject);
    }
}
