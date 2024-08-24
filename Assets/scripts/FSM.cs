using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    //playertransform
    protected Transform playertransfrom;
    //next pos to AI tank
    protected Vector3 destpos;
    //list of wandepoints
    protected GameObject[] pointlist;
    protected float firerate;
    protected float elapsedtime;

    //tank turrent
    protected Transform turrent { get; set; }
    protected Transform spawnpoint {  get; set; }
    protected virtual void initialize() { }
    protected virtual void FSMupdate() { }
    protected virtual void FSMFixedupdate() { }
    void Start()
    {
        initialize();
    }

    
    void Update()
    {
        FSMupdate();

    }
    void FixedUpdate() 
    {
        FSMFixedupdate();
    }
}
