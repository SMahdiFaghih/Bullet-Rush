using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public int Health;
    [HideInInspector]
    public int EnemyType;

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
        if (Health == 1)
        {
            Destroy(gameObject);
        }
        else
        {
            SetEnemyNewProperties();
        }
    }

    private void SetEnemyNewProperties()
    {
        EnemyType -= 1;
        EnemiesData.Enemy enemyData = GameManager.Instance.EnemyTypes.Enemies[EnemyType];

        Speed = enemyData.Speed;
        Health = enemyData.Health;

        transform.localScale = enemyData.Scale;
        GetComponent<Renderer>().material.color = enemyData.Color;
    }
}
