using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeaderUIController : MonoBehaviour
{
    [Header("Bar")]
    public FloatVariable NumOfDeadEnemies;
    public Text PercentageOfDeadEnemies;
    public Image FillBar;

    [Header("Stars")]
    public List<FloatVariable> StarsPercentage;
    public List<GameObject> EmptyStars;
    public List<GameObject> GainedStars;
    private AudioSource CollectStarSound;
    public FloatVariable NumOfGainedStars;

    [Header("Level")]
    public Text Level;

    void Start()
    {
        for (int i=0;i < EmptyStars.Count;i++)
        {
            EmptyStars[i].GetComponentInChildren<Text>().text = StarsPercentage[i].Value.ToString();
        }
        Level.text = SceneManager.GetActiveScene().name;
        CollectStarSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        float percentage = NumOfDeadEnemies.Value / GameManager.Instance.NumOfEnemies;
        FillBar.fillAmount = percentage;
        percentage *= 100;
        PercentageOfDeadEnemies.text = Mathf.Floor(percentage).ToString() + "%";
        if (NumOfGainedStars.Value < 3)
        {
            CheckGainStar(percentage);
        }
    }

    private void CheckGainStar(float deadEnemiesPercentage)
    {
        if (deadEnemiesPercentage >= StarsPercentage[(int) NumOfGainedStars.Value].Value)
        {
            GainStar((int) NumOfGainedStars.Value);
            NumOfGainedStars.Value ++;
        }
    }

    private void GainStar(int id)
    {
        EmptyStars[id].SetActive(false);
        GainedStars[id].SetActive(true);
        CollectStarSound.Play();
    }
}
