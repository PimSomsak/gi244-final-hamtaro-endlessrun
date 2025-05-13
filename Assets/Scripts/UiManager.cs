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

    public TextMeshProUGUI distanceText;


    private PlayerController playerController;

    private void Awake()
    {
        Time.timeScale = 0f;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Start()
    {
        titleScreen.SetActive(true);
        inGameUi.SetActive(false);
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);  
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
        distanceText.text = "" + playerController.distanceText.text.ToString();
    }
    public void Back()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
}
