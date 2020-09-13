using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject EnemySpawnZonesParent;
    public EnemiesData EnemyTypes;
    public GameObject EnemyPrefab;

    private Transform[] EnemySpwanZones;
    private int TotalSelectionChance = 0;

    [HideInInspector]
    public int NumOfEnemies = 0;
    public FloatVariable NumOfDeadEnemies;

    public Canvas PauseMenu;
    private Button[] PauseMenuButtons;

    private System.Random Random = new System.Random();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        NumOfDeadEnemies.Value = 0;
        EnemySpwanZones = EnemySpawnZonesParent.GetComponentsInChildren<Transform>();
        foreach (EnemiesData.Enemy enemy in EnemyTypes.Enemies)
        {
            TotalSelectionChance += enemy.SelectionChance;
        }
        SpawnEnemies();
        PauseMenu.gameObject.SetActive(false);
        PauseMenuButtons = PauseMenu.GetComponentsInChildren<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                PauseMenu.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.gameObject.SetActive(false);
                foreach (Button button in PauseMenuButtons)
                {
                    button.image.enabled = false;
                }
            }
        }
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
                    int enemType = GetEnemyType();
                    Vector3 enemySpawnPosition = new Vector3(i, EnemyTypes.Enemies[enemType].Scale.y + 0.25f, j);
                    GameObject enemy = Instantiate(EnemyPrefab, enemySpawnPosition, Quaternion.identity);
                    enemy.GetComponent<EnemyController>().SetEnemyNewProperties(enemType);
                    NumOfEnemies++;
                }
            }
        }
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

    public void GotoNextLevel()
    {
        Debug.Log("Win");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
