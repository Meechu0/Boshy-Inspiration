using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded = true;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 //check the int value in layer manager(User Defined starts at 8) 
            && !isGrounded)
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8
            && isGrounded)
        {
            isGrounded = false;
        }
    }
}
