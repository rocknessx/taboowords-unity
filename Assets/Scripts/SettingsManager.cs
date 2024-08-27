using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public string aTeamName;
    public string bTeamName;
    public int passRights;
    public int gameDuration;
    public int tabooRights;
    public int winScore;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Bu, sahneler arasýnda geçiþ yaparken nesneyi korur
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Getter ve setter metodlarý
    public string GetATeamName()
    {
        return aTeamName;
    }

    public void SetATeamName(string name)
    {
        aTeamName = name;
    }

    public string GetBTeamName()
    {
        return bTeamName;
    }

    public void SetBTeamName(string name)
    {
        bTeamName = name;
    }

    public int GetPassRights()
    {
        return passRights;
    }

    public void SetPassRights(int value)
    {
        passRights = value;
    }

    public int GetGameDuration()
    {
        return gameDuration;
    }

    public void SetGameDuration(int value)
    {
        gameDuration = value;
    }

    public int GetTabooRights()
    {
        return tabooRights;
    }

    public void SetTabooRights(int value)
    {
        tabooRights = value;
    }

    public int GetWinScore()
    {
        return winScore;
    }

    public void SetWinScore(int value)
    {
        winScore = value;
    }
}
