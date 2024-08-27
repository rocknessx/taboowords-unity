using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public InputField teamANameInput;
    public InputField teamBNameInput;
    public InputField passRightsInput;
    public InputField gameDurationInput;
    public InputField tabooRightsInput;
    public InputField winScoreInput;

    public void SaveSettings()
    {
        int passRights;
        int.TryParse(passRightsInput.text, out passRights);

        SettingsManager.Instance.SetATeamName(teamANameInput.text);
        SettingsManager.Instance.SetBTeamName(teamBNameInput.text);
        SettingsManager.Instance.SetPassRights(passRights);
        SettingsManager.Instance.SetGameDuration(int.Parse(gameDurationInput.text));
        SettingsManager.Instance.SetTabooRights(int.Parse(tabooRightsInput.text));
        SettingsManager.Instance.SetWinScore(int.Parse(winScoreInput.text));

        // SettingsManager'dan aldýðý verilerle TabuGame sahnesine geçiþ yapýn
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
