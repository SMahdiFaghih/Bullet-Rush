using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            GameManager.Instance.Restart();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Helicopter Landing Pad")
        {
            GameManager.Instance.GotoNextLevel();
        }
    }
}
