using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public int Speed;
    [HideInInspector]
    public int Health;

    private GameObject Player;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.LookAt(Player.transform);
        Move();
    }

    private void Move()
    {
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
    }

    public void ReduceHealth()
    {
        Health -= 1;
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }
}
