using System.Collections;
using UnityEngine;

public class Simple_FSM : FSM
{
    public enum FSMstate
    {
        none,
        patrol,
        chase,
        attack,
        dead,
    }
    public FSMstate Curstate;
    [SerializeField] private float CurSpeed;
    [SerializeField] private float CurRotSpeed;
    [SerializeField] private float Firate;
    [SerializeField] private GameObject turent;
    [SerializeField] private GameObject spwan;
    public GameObject Bullet;
    private bool ISDead = false;
    [SerializeField]private int health;


    protected override void initialize()
    {
        Curstate = FSMstate.patrol;
        CurSpeed = 50.0f;
        CurRotSpeed = 2.0f;
        ISDead = false;
        elapsedtime = 0;
        firerate = Firate;
        
        pointlist =
            GameObject.FindGameObjectsWithTag("Wanderpoints");
        FindNextPoint();

        GameObject objpalyer =
            GameObject.FindGameObjectWithTag("Player");
        playertransfrom = objpalyer.transform;
        if (!playertransfrom)
        {
            print("player doesnt exist..pls addone " +
                "with tag named 'player'");

        }
        turrent = turent.transform;
        spawnpoint = spwan.transform;
    }



    protected override void FSMupdate()
    {
        switch (Curstate)
        {
            case FSMstate.patrol: UpdatePatrolState(); break;
            case FSMstate.chase: UpdateChaseState(); break;
            case FSMstate.attack: UpdateAttackState(); break;
            case FSMstate.dead: UpdateDeadState(); break;


        }
        elapsedtime += Time.deltaTime;
        if (health <= 0)
        {
            Curstate = FSMstate.dead;
        }
    }
    protected void UpdatePatrolState()
    {
        //find another point if the current point is reached
        if (Vector3.Distance(transform.position, destpos) <= 100.0f)
        {
            print("reached to the destination point\n" +
                "calculating the next point");
            FindNextPoint();
        }
        else if (Vector3.Distance(transform.position, playertransfrom.position) <= 100.0f)
        {
            print("switching to chasemode");
            Curstate = FSMstate.chase;
        }
        //rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destpos
            - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation
            , Time.deltaTime * CurRotSpeed);
        //go forwad
        transform.Translate(Vector3.forward * Time.deltaTime *
            CurSpeed);
    }

    protected void UpdateChaseState()
    {
        destpos = playertransfrom.position;
        float distance= Vector3.Distance(transform.position, playertransfrom.position);
        if(distance <= 200.0f)
        {
            Curstate = FSMstate.attack;
        }
        else if(distance >= 300)
        {
            Curstate = FSMstate.patrol;
        }
     transform.Translate(Vector3.forward*Time.deltaTime * CurSpeed);
    }

    protected void UpdateAttackState()
    {
        destpos = playertransfrom.position;

        float dist =Vector3.Distance(transform.position,
            playertransfrom.position);
        if(dist >= 100.0f && dist < 200.0f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(destpos- transform.position);
            transform.rotation =
                Quaternion.Slerp(targetRotation,targetRotation,Time.deltaTime * CurRotSpeed);
            transform.Translate(Vector3.forward*Time.deltaTime*CurSpeed);
            Curstate = FSMstate.attack;
        }
        else if(dist >=200.0f)
        {
            Curstate= FSMstate.patrol;
        }
        //always turn the turrent toward the players 
        Quaternion turrentrotation =
            Quaternion.LookRotation(destpos - turrent.position);

        turrent.rotation =
            Quaternion.Slerp(turrent.rotation,turrentrotation,
            Time.deltaTime *CurSpeed);
        ShootBullet();

    }

    private void ShootBullet()
    {
        if(elapsedtime>= firerate)
        {
            Instantiate(Bullet, spawnpoint.position, spawnpoint.rotation);
            elapsedtime = 0;
        }
    }

    protected void UpdateDeadState()
    {
        if (!ISDead)
        {
            ISDead= true;
            Explode();
        }
        

    }

    protected void Explode()
    {
        float rndx = Random.Range(10.0f, 30.0f);
        float rndz = Random.Range(10.0f, 30.0f);
        for(int i = 0; i < 3; i++) 
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.AddExplosionForce(10000.0f,transform.position -
                new Vector3(rndx,10,rndz),40.0f,10.0f);
            rb.velocity = transform.TransformDirection(
                new Vector3(rndx,20.0f,rndz));
            
        }
        Destroy(gameObject,1.5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullets>().damage;
        }
    }

    protected void FindNextPoint()
    {
        print("getting the next point");
        int RNDindex = Random.Range(0, pointlist.Length);
        float RNRadius = 10.0f;
        Vector3 rndPostion = Vector3.zero;
        destpos = pointlist[RNDindex].transform.position+rndPostion;

        //check the range to decide the random point 
        if (IsIncurrentRange(destpos))
        {
            rndPostion = new Vector3(Random.Range(-RNRadius,RNRadius),0,
                Random.Range(-RNRadius,RNDindex));
            destpos = pointlist[RNDindex].transform.position;
        }

    }

    protected bool IsIncurrentRange(Vector3 destpos)
    {
        float xPos = Mathf.Abs(destpos.x - transform.position.x);
        float zPos = Mathf.Abs(destpos.z - transform.position.z);

        if (xPos <= 50 && zPos <= 50)
        {
            return true;
        }
        return false;
    }
}
