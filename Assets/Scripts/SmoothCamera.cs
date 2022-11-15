using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField]private float smoothTimer;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform Player;

    private void Start()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
    }

    private void Update()
    {
        if (Player)
        {
            Vector3 PlayerPos = Player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, PlayerPos, ref velocity, smoothTimer);
        }

    }


}
