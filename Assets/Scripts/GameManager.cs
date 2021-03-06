﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player;

    private float CelebrateWinJumpForce = 250;
    private bool IsLevelCompleted;

    public GameObject EnemySpawnZonesParent;
    public EnemiesData EnemyTypes;
    public GameObject EnemyPrefab;

    private Transform[] EnemySpwanZones;
    private int TotalSelectionChance = 0;

    [HideInInspector]
    public int NumOfEnemies = 0;
    public FloatVariable NumOfDeadEnemies;

    public GameObject PauseMenu;
    private Button[] PauseMenuButtons;

    public GameObject LevelCompletedUI;

    private System.Random Random = new System.Random();

    [Header("Stars")]
    private AudioSource LevelCompletedSound;
    public AudioClip ZeroStarWinSound;
    public AudioClip OneStarWinSound;
    public AudioClip TwoStarWinSound;
    public AudioClip ThreeStarWinSound;
    public FloatVariable NumOfGainedStars;

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
        IsLevelCompleted = false;
        LevelCompletedSound = GetComponent<AudioSource>();
        LevelCompletedUI.SetActive(false);
    }

    void Update()
    {
        if (!IsLevelCompleted)
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

    public void StartLevelCompletedProcesses()
    {
        SetNextLevel();
        Player.transform.eulerAngles = new Vector3(0, 0, 0);
        StartCoroutine(CelebrateWin());
        PlayLevelCompletedSound();
        IsLevelCompleted = true;
        PlayerController.IsLevelCompleted = true;
        EnemyController.IsLevelCompleted = true;
        CameraController.IsLevelCompleted = true;
        LevelCompletedUI.SetActive(true);
    }

    private void SetNextLevel()
    {
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        string path = SceneUtility.GetScenePathByBuildIndex(currentSceneBuildIndex + 1);
        int slash = path.LastIndexOf('/');
        string nextSceneName = path.Substring(slash + 1);
        int dot = nextSceneName.LastIndexOf('.');
        nextSceneName = nextSceneName.Substring(0, dot);
        PlayerPrefs.SetString("Current Level", nextSceneName);
    }

    private void PlayLevelCompletedSound()
    {
        switch (NumOfGainedStars.Value)
        {
            case 0:
                LevelCompletedSound.clip = ZeroStarWinSound;
                break;
            case 1:
                LevelCompletedSound.clip = OneStarWinSound;
                break;
            case 2:
                LevelCompletedSound.clip = TwoStarWinSound;
                break;
            case 3:
                LevelCompletedSound.clip = ThreeStarWinSound;
                break;
        }
        LevelCompletedSound.Play();
    }

    private IEnumerator CelebrateWin()
    {
        Player.GetComponent<Animator>().enabled = true;
        Rigidbody rigidbody = Player.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;

        while (true)
        {
            rigidbody.AddForce(CelebrateWinJumpForce * transform.up);

            yield return new WaitForSeconds(2);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
