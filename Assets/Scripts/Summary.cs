using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    public Text aTeamNameText;
    public Text bTeamNameText;
    public Text scoreText;
    public Text currentRoundText;
    public Text correctCountText;
    public Text tabooCountText;
    public Text totalScoreText;

    void Start()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager.Instance null! Lütfen SettingsManager'ýn Scene'de mevcut olduðundan emin olun.");
            return;
        }

        aTeamNameText.text = "A Takýmý: " + SettingsManager.Instance.GetATeamName();
        bTeamNameText.text = "B Takýmý: " + SettingsManager.Instance.GetBTeamName();

        // Diðer verileri de buradan çekebilir ve güncelleyebilirsiniz
    }
}
