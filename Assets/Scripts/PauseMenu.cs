using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button continueButton;
    public Button mainMenuButton;

    private bool isPaused = false;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        pauseMenuPanel.SetActive(false); // Baþlangýçta paneli gizle
    }

    void Update()
    {
        // ESC tuþuna basýldýðýnda pause menüyü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // Paneli göster
        Time.timeScale = 0f; // Oyunu duraklat
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // Paneli gizle
        Time.timeScale = 1f; // Oyunu devam ettir
        isPaused = false;
    }

    public void OnContinueButtonClicked()
    {
        ResumeGame(); // Oyuna devam et
    }

    public void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1f; // Oyunu devam ettir (ana menüye dönerken)
        SceneManager.LoadScene("MainMenu"); // Ana menüye geçiþ
    }
}
