using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrap : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;

    public float speed = 1.0F;

    private float startTime;

    private float journeyLength;


    public GameObject Trap;
    public Transform target;
    public float duration;

    float distance;
    float t;
    private void Start()
    {
        startTime = Time.time;

        journeyLength = Vector3.Distance(Trap.transform.position, endMarker.position);
    }
    private void Update()
    {

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        Trap.transform.position = Vector3.Lerp(Trap.transform.position, endMarker.position, fractionOfJourney);


    }
}