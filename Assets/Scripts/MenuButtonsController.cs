using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsLevelCompletedIcon = false;

    private Button button;
    private AudioSource ButtonClickSound;

    void Awake()
    {
        button = GetComponent<Button>();
        if (!IsLevelCompletedIcon)
        {
            button.image.enabled = false;
            ButtonClickSound = GetComponent<AudioSource>();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        button.image.enabled = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!IsLevelCompletedIcon)
        {
            button.image.enabled = false;
        }
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
        Time.timeScale = 1;
        GameManager.Instance.Restart();
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
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void OnLevelCompletedRestartIconClick()
    {
        GameManager.Instance.Restart();
    }

    public void OnLevelCompletedMainMenuIconClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnLevelCompletedNextLevelIconClick()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Current Level"));
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
