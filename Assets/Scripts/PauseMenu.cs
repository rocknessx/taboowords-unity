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
        pauseMenuPanel.SetActive(false); // Ba�lang��ta paneli gizle
    }

    void Update()
    {
        // ESC tu�una bas�ld���nda pause men�y� a�/kapat
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
        pauseMenuPanel.SetActive(true); // Paneli g�ster
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
        Time.timeScale = 1f; // Oyunu devam ettir (ana men�ye d�nerken)
        SceneManager.LoadScene("MainMenu"); // Ana men�ye ge�i�
    }
}
