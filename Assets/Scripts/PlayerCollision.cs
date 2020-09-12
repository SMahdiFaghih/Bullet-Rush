using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerController PlayerController;

    void Start()
    {
        PlayerController = GetComponent<PlayerController>();
    }

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
        else if(collider.tag == "Speed Boost")
        {
            Destroy(collider.gameObject);
            StartCoroutine(PlayerController.ActivateSpeedBoost());
        }
    }
}
