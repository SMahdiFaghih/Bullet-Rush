using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public FloatVariable NumOfDeadEnemies;
    public Text PercentageOfDeadEnemies;
    private Image FillBar;

    void Start()
    {
        FillBar = GetComponent<Image>();
    }

    void Update()
    {
        float percentage = NumOfDeadEnemies.Value / GameManager.Instance.NumOfEnemies;
        PercentageOfDeadEnemies.text = Mathf.Round(percentage * 100).ToString() + "%";
        FillBar.fillAmount = percentage;
    }
}
