using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputField aTeamNameInput;
    public InputField bTeamNameInput;
    public InputField passRightsInput;
    public InputField gameDurationInput;
    public InputField tabooRightsInput;
    public InputField winScoreInput;

    public Button saveButton;

    void Start()
    {
        saveButton.onClick.AddListener(SaveSettings);
    }

    void SaveSettings()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager.Instance null! L�tfen SettingsManager'�n Scene'de mevcut oldu�undan emin olun.");
            return;
        }

        // Ayarlar� g�ncelle
        SettingsManager.Instance.SetATeamName(aTeamNameInput.text);
        SettingsManager.Instance.SetBTeamName(bTeamNameInput.text);
        SettingsManager.Instance.SetPassRights(int.Parse(passRightsInput.text));
        SettingsManager.Instance.SetGameDuration(int.Parse(gameDurationInput.text));
        SettingsManager.Instance.SetTabooRights(int.Parse(tabooRightsInput.text));
        SettingsManager.Instance.SetWinScore(int.Parse(winScoreInput.text));

        // Geri d�n
        SceneManager.LoadScene(1); // veya istedi�iniz di�er sahne
    }
}
