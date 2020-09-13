using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    private Button button;
    private AudioSource ButtonClickSound;

    void Awake()
    {
        button = GetComponent<Button>();
        button.image.enabled = false;
        ButtonClickSound = GetComponent<AudioSource>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        button.image.enabled = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        button.image.enabled = false;
    }

    public void OnNewGameButtonClick()
    {
        PlayerPrefs.SetString("Current Level", "Level 1");
        SceneManager.LoadScene("Level 1");
    }

    public void OnMainMenuContinueButtonClick()
    {
        if (PlayerPrefs.HasKey("Current Level"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("Current Level"));
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void OnPauseMenuContinueButtonClick()
    {
        ButtonClickSound.Play();
        Time.timeScale = 1;
        Canvas pauseMenu = GetComponentInParent<Canvas>();
        pauseMenu.gameObject.SetActive(false);
    }

    public void OnPauseMenuRestartButtonClick()
    {
        ButtonClickSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void OnMainMenuExitButtonClick()
    {
        ButtonClickSound.Play();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void OnPauseMenuExitButtonClick()
    {
        if (PlayerPrefs.HasKey("Current Level"))
        {
            if (int.Parse(PlayerPrefs.GetString("Current Level").Split()[1]) < int.Parse(SceneManager.GetActiveScene().name.Split()[1]))
            {
                PlayerPrefs.SetString("Current Level", SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            PlayerPrefs.SetString("Current Level", SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void OnCreditsSceneBackButtonClick()
    {
        int creditsSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        string path = SceneUtility.GetScenePathByBuildIndex(creditsSceneBuildIndex - 1);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        string lastLevel =  name.Substring(0, dot);

        PlayerPrefs.SetString("Current Level", lastLevel);
        SceneManager.LoadScene("Main Menu");
    }
}
