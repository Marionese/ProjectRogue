using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void Resume()
    {
        TogglePause();
    }

    public void QuitToMenu()
    {
        // Speichere MetaProgress!
        SaveSystem.SaveGame(GameSession.Instance.currentSlot);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
