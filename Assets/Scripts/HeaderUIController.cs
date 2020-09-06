using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int StarToGainId = 0;

    void Update()
    {
        float percentage = NumOfDeadEnemies.Value / GameManager.Instance.NumOfEnemies;
        FillBar.fillAmount = percentage;
        percentage *= 100;
        PercentageOfDeadEnemies.text = Mathf.Floor(percentage).ToString() + "%";
        if (StarToGainId < 3)
        {
            CheckGainStar(percentage);
        }
    }

    private void CheckGainStar(float deadEnemiesPercentage)
    {
        if (deadEnemiesPercentage >= StarsPercentage[StarToGainId].Value)
        {
            GainStar(StarToGainId);
            StarToGainId++;
        }
    }

    private void GainStar(int id)
    {
        EmptyStars[id].SetActive(false);
        GainedStars[id].SetActive(true);
    }
}
