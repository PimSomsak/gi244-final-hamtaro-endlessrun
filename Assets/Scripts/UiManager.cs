using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("UI Elements")]

    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject inGameUi;


    private void Awake()
    {
        Time.timeScale = 0f;
    }
    private void Start()
    {
        titleScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        inGameUi.SetActive(false);
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        titleScreen.SetActive(false);
        inGameUi.SetActive(true);
    }
    public void OverScreen()
    {
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        inGameUi.SetActive(false);
    }
    public void Back()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
}
