using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemiesData", menuName = "EnemiesData")]
public class EnemiesData : ScriptableObject
{
    public List<Enemy> Enemies = new List<Enemy>();

    [Serializable]
    public class Enemy
    {
        public Vector3 Scale = new Vector3();

        public float Speed;
        public int Health;

        public Color Color;

        public int SelectionChance;
    }
}
