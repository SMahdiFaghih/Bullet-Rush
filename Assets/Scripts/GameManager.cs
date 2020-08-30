using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject EnemySpawnZonesParent;
    public EnemiesData EnemyTypes;
    public GameObject EnemyPrefab;

    private Transform[] EnemySpwanZones;
    private int TotalSelectionChance = 0;

    private System.Random Random = new System.Random();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EnemySpwanZones = EnemySpawnZonesParent.GetComponentsInChildren<Transform>();
        foreach (EnemiesData.Enemy enemy in EnemyTypes.Enemies)
        {
            TotalSelectionChance += enemy.SelectionChance;
        }
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (Transform EnemySpwanZone in EnemySpwanZones)
        {
            if (EnemySpwanZone.gameObject == EnemySpawnZonesParent)
            {
                continue;
            }

            Mesh mesh = EnemySpwanZone.gameObject.GetComponent<MeshFilter>().mesh;
            float dimX = mesh.bounds.size.x * EnemySpwanZone.localScale.x;
            float dimZ = mesh.bounds.size.z * EnemySpwanZone.localScale.z;

            for(float i = EnemySpwanZone.position.x - dimX/2; i <= EnemySpwanZone.position.x + dimX/2; i += 1.5f)
            {
                for (float j = EnemySpwanZone.position.z - dimZ/2; j <= EnemySpwanZone.position.z + dimZ/2; j += 1.5f)
                {
                    Vector3 enemySpawnPosition = new Vector3(i, 3, j);
                    GameObject enemy = Instantiate(EnemyPrefab, enemySpawnPosition, Quaternion.identity);
                    SetEnemyProperties(enemy);
                }
            }
        }
    }

    private void SetEnemyProperties(GameObject enemy)
    {
        int enemyType = GetEnemyType();
        EnemiesData.Enemy enemyData = EnemyTypes.Enemies[enemyType];

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.Health = enemyData.Health;
        enemyController.Speed = enemyData.Speed;
        enemyController.EnemyType = enemyType;

        enemy.transform.localScale = enemyData.Scale;
        enemy.GetComponent<Renderer>().material.color = enemyData.Color;
    }

    private int GetEnemyType()
    {
        int randomNum = Random.Next(TotalSelectionChance);
        int result = 0;
        for (int i=0; i < EnemyTypes.Enemies.Count; i++)
        {
            result += EnemyTypes.Enemies[i].SelectionChance;
            if (randomNum < result)
            {
                return i;
            }
        }
        return EnemyTypes.Enemies.Count;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
