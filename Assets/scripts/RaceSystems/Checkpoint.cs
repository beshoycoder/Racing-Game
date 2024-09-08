using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} by the normal collider from {this.name}");

       this.gameObject.SetActive( false );
    }

}
