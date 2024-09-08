using System.Collections;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    /// <summary>
    /// this script will handle:
    /// race data
    /// position the reacers in the start area
    /// the checkpoints of the race 
    /// keep track of the positions of each racer
    /// how many player finished the race and how many required to start the end race count down
    /// </summary>
    private void Awake()
    {
        
    }

    void Start()
    {
        StartCoroutine(StartRace());
    }
    IEnumerator StartRace()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("3");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("2");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("1");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("GO");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
