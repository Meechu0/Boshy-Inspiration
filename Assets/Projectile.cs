using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private float speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 7f);
    }
    private void FixedUpdate()
    {
       // transform.position += transform.right * Time.deltaTime * speed;  
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame

}
