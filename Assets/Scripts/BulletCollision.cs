using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Contains("Gun"))
        {
            return;
        }
        Destroy(gameObject);
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider);
        }
    }
}
