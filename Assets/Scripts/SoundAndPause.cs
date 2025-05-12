using UnityEngine;

public class SoundAndPause : MonoBehaviour
{
    //SoundAndPause in MainCameraComponant!

    private AudioSource audioSource;
    private bool isMuted = false;
    private bool isPaused = false;

    UiManager uiManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            uiManager.pauseScreen.SetActive(true);
            uiManager.inGameUi.SetActive(false);

        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            uiManager.pauseScreen.SetActive(false);
            uiManager.inGameUi.SetActive(true);
        }
    }
}
