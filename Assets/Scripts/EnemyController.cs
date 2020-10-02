using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public static bool IsLevelCompleted = false;
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

    void Start()
    {
        IsLevelCompleted = false;
    }

    void Update()
    {
        transform.LookAt(Player.transform);
        if (!IsLevelCompleted)
        {
            Move();
        }
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
            GameManager.Instance.NumOfDeadEnemies.Value++;
            Destroy(gameObject);
        }
        else
        {
            SetEnemyNewProperties(EnemyType - 1);
        }
    }

    public void SetEnemyNewProperties(int enemyType)
    {
        EnemiesData.Enemy enemyData = GameManager.Instance.EnemyTypes.Enemies[enemyType];

        Speed = enemyData.Speed;
        Health = enemyData.Health;
        EnemyType = enemyType;

        transform.localScale = enemyData.Scale;
        GetComponent<Renderer>().material.color = enemyData.Color;
    }
}
