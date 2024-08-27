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
            Debug.LogError("SettingsManager.Instance null! L�tfen SettingsManager'�n Scene'de mevcut oldu�undan emin olun.");
            return;
        }

        aTeamNameText.text = "A Tak�m�: " + SettingsManager.Instance.GetATeamName();
        bTeamNameText.text = "B Tak�m�: " + SettingsManager.Instance.GetBTeamName();

        // Di�er verileri de buradan �ekebilir ve g�ncelleyebilirsiniz
    }
}
