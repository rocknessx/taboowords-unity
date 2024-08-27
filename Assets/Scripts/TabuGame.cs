using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabuGame : MonoBehaviour
{
    // UI Elementleri
    public Text mainWordText;
    public List<Text> tabooWordsTexts;
    public Text timerText;
    public Text scoreAText;
    public Text scoreBText;
    public Text passRightsText;
    public Text tabooRightsText;
    public Text teamAText;
    public Text teamBText;

    public Button correctButton;
    public Button passButton;
    public Button tabooButton;
    public Button pauseButton;

    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button mainMenuButton;

    public GameObject summaryPanel; // Özet paneli
    public Text summaryText; // Özet metni
    public Text summaryText1;
    public Text summaryText2;
    public Text summaryText3;
    public Text summaryText4;
    public Text summaryText5;
    public Text summaryText6;
    public Button nextRoundButton; // Sonraki tur butonu
    public GameObject gameOverPanel; // Oyun Sonu Paneli
    public Text winnerTeamText;
    public Text winnerScoreText;
    public Text teamAStatsText;
    public Text teamBStatsText;
    public Button restartButton;
    public Button mainMenuButtonGameOver;

    // Kartlarý saklayan liste
    public List<TabuCard> cards; // Burada TabuCard nesneleri listesi olmalý

    private List<int> availableCardIndices;
    private float timeRemaining;
    private bool isGameActive = false;
    private bool isPaused = false;
    private int scoreA = 0;
    private int scoreB = 0;
    private int passRights;
    private int tabooRights;
    private int winScore;
    private float roundDuration;
    private int currentRound = 1;
    private bool isATurn = true; // Ýlk baþta A takýmý oynar

    private int correctCountThisRound = 0;
    private int tabooCountThisRound = 0;
    private int totalPassCountA = 0; // Toplam pas sayýsý A takýmý için
    private int totalPassCountB = 0; // Toplam pas sayýsý B takýmý için
    private int totalCorrectCountA = 0; // Toplam doðru sayýsý A takýmý için
    private int totalCorrectCountB = 0; // Toplam doðru sayýsý B takýmý için
    private int totalTabooCountA = 0; // Toplam tabu sayýsý A takýmý için
    private int totalTabooCountB = 0; // Toplam tabu sayýsý B takýmý için

    void Start()
    {
        // Referanslarýn atanýp atanmadýðýný kontrol edin
        if (timerText == null || scoreAText == null || scoreBText == null || passRightsText == null || tabooRightsText == null || pauseMenuPanel == null || summaryPanel == null || summaryText == null || nextRoundButton == null)
        {
            Debug.LogError("Referans eksik! Lütfen tüm UI elementlerini atadýðýnýzdan emin olun.");
            return;
        }

        if (cards == null || cards.Count == 0)
        {
            Debug.LogError("Kartlar listesi eksik veya boþ!");
            return;
        }

        // Butonlara iþlevsellik ekleyin
        correctButton.onClick.RemoveAllListeners();
        correctButton.onClick.AddListener(OnCorrectButtonClicked);

        passButton.onClick.RemoveAllListeners();
        passButton.onClick.AddListener(OnPassButtonClicked);

        tabooButton.onClick.RemoveAllListeners();
        tabooButton.onClick.AddListener(OnTabooButtonClicked);

        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(OnResumeButtonClicked);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

        nextRoundButton.onClick.RemoveAllListeners();
        nextRoundButton.onClick.AddListener(OnNextRoundButtonClicked);

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartButtonClicked);

        mainMenuButtonGameOver.onClick.RemoveAllListeners();
        mainMenuButtonGameOver.onClick.AddListener(OnMainMenuButtonGameOverClicked);

        pauseMenuPanel.SetActive(false);
        summaryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        StartGame();
    }

    public void StartGame()
    {
        // SettingsManager.Instance null kontrolü yapýn
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager.Instance null! Lütfen SettingsManager'ýn Scene'de mevcut olduðundan emin olun.");
            return;
        }

        // Ayarlardan verileri al
        passRights = SettingsManager.Instance.passRights;
        tabooRights = SettingsManager.Instance.tabooRights;
        winScore = SettingsManager.Instance.winScore;
        roundDuration = SettingsManager.Instance.gameDuration; // gameDuration olarak deðiþtirildi

        // Deðerlerin geçerliliðini kontrol et
        ValidateSettings();

        // UI Güncellemeleri
        UpdateScoreText();
        UpdatePassRightsText();
        UpdateTabooRightsText();
        UpdateTeamNames();

        // Kart indekslerini baþlat
        availableCardIndices = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            availableCardIndices.Add(i);
        }

        // Oyunu baþlat
        isGameActive = true;
        isPaused = false;
        timeRemaining = roundDuration; // Ýlk turun süresini ayarla
        ShowNextCard();
    }

    void Update()
    {
        if (isGameActive && !isPaused)
        {
            // Zamanlayýcýyý güncelle
            timeRemaining -= Time.deltaTime;

            // Timer deðerini güncelle
            timerText.text = Mathf.Max(Mathf.Ceil(timeRemaining), 0).ToString();

            // Süre dolduðunda tur bitir
            if (timeRemaining <= 0)
            {
                EndRound();
            }
            if (scoreA >= winScore || scoreB >= winScore)
            {
                ShowGameOverPanel();
                print("Geldiniz");
                isPaused = true;
            }
        }
    }

    void ValidateSettings()
    {
        // Game Duration'ýn geçerliliðini kontrol et
        if (roundDuration < 30 || roundDuration > 180)
        {
            Debug.LogError("Game Duration geçersiz! (30 ile 180 saniye arasýnda olmalý.)");
            return;
        }

        // Pass Rights'ýn geçerliliðini kontrol et
        if (passRights < 0 || passRights > 5)
        {
            Debug.LogError("Pass Rights geçersiz! (0 ile 5 arasýnda olmalý.)");
            return;
        }

        // Taboo Rights'ýn geçerliliðini kontrol et
        if (tabooRights < 0 || tabooRights > 3)
        {
            Debug.LogError("Taboo Rights geçersiz! (0 ile 3 arasýnda olmalý.)");
            return;
        }

        // Win Score'ýn geçerliliðini kontrol et
        if (winScore < 25 || winScore > 250)
        {
            Debug.LogError("Win Score geçersiz! (25 ile 250 arasýnda olmalý.)");
            return;
        }
    }

    public void ShowNextCard()
    {
        if (availableCardIndices.Count > 0)
        {
            // Rastgele bir kart seç
            int randomIndex = Random.Range(0, availableCardIndices.Count);
            int cardIndex = availableCardIndices[randomIndex];
            availableCardIndices.RemoveAt(randomIndex); // Seçilen kartý listeden çýkar

            TabuCard card = cards[cardIndex];
            mainWordText.text = card.mainWord;
            for (int i = 0; i < tabooWordsTexts.Count; i++)
            {
                if (i < card.tabooWords.Count)
                {
                    tabooWordsTexts[i].text = card.tabooWords[i];
                }
                else
                {
                    tabooWordsTexts[i].text = "";
                }
            }

            // Eðer tüm kartlar gösterildiyse, kart listesini tekrar doldur
            if (availableCardIndices.Count == 0)
            {
                availableCardIndices = new List<int>(); // Kart indekslerini baþlat
                for (int i = 0; i < cards.Count; i++)
                {
                    availableCardIndices.Add(i);
                }
            }
        }
    }

    public void OnCorrectButtonClicked()
    {
        if (isGameActive)
        {
            correctCountThisRound++;

            if (isATurn) // A Takýmý
            {
                scoreA++; // A Takýmý skorunu 1 artýr
            }
            else // B Takýmý
            {
                scoreB++; // B Takýmý skorunu 1 artýr
            }
            UpdateScoreText();
            ShowNextCard(); // Doðru butonuna basýldýðýnda bir sonraki karta geç
        }
    }

    public void OnPassButtonClicked()
    {
        if (isGameActive)
        {
            if (passRights > 0)
            {
                ShowNextCard(); // Bir sonraki karta geç
                passRights--; // Pas hakkýný azalt
                UpdatePassRightsText(); // Pas haklarýný güncelle

                if (passRights <= 0)
                {
                    passButton.interactable = false; // Pas hakký bittiðinde butonu devre dýþý býrak
                }
            }
        }
    }

    public void OnTabooButtonClicked()
    {
        if (isGameActive)
        {
            tabooCountThisRound++;

            if (isATurn) // A Takýmý
            {
                scoreA--; // A Takýmý skorunu 1 azalt
            }
            else // B Takýmý
            {
                scoreB--; // B Takýmý skorunu 1 azalt
            }
            UpdateScoreText();
            ShowNextCard(); // Tabu butonuna basýldýðýnda bir sonraki karta geç
        }
    }

    void EndRound()
    {
        if (isGameActive)
        {
            // Oyunu duraklat
            isPaused = true;

            // Özet panelini aç
            ShowSummary();
        }
    }

    void EndGame()
    {
        isGameActive = false;
        pauseMenuPanel.SetActive(false);
        ShowSummary(); // Oyunu bitirdiðinde özet panelini göster
    }

    void UpdateScoreText()
    {
        if (scoreAText != null && scoreBText != null)
        {
            scoreAText.text = "A Takýmý: " + scoreA;
            scoreBText.text = "B Takýmý: " + scoreB;
        }
    }

    void UpdatePassRightsText()
    {
        if (passRightsText != null)
        {
            passRightsText.text = "Pas Hakký: " + passRights;
        }
    }

    void UpdateTabooRightsText()
    {
        if (tabooRightsText != null)
        {
            tabooRightsText.text = "Tabu Hakký: " + tabooRights;
        }
    }

    void UpdateTeamNames()
    {
        if (teamAText != null && teamBText != null)
        {
            teamAText.text = "A Takýmý: " + SettingsManager.Instance.GetATeamName();
            teamBText.text = "B Takýmý: " + SettingsManager.Instance.GetBTeamName();
        }
    }

    public void OnPauseButtonClicked()
    {
        if (isGameActive)
        {
            isPaused = true;
            pauseMenuPanel.SetActive(true);
        }
    }

    public void OnResumeButtonClicked()
    {
        if (isGameActive)
        {
            isPaused = false;
            pauseMenuPanel.SetActive(false);
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0); // Ana menü sahnesine geri dön
    }

    // Özet Panelini Göster
    void ShowSummary()
    {
        // Özet panelini aç ve bilgileri göster
        summaryPanel.SetActive(true);

        // Özeti güncelle
        summaryText.text = "Oyun Özeti\n";
        summaryText1.text = SettingsManager.Instance.GetATeamName() + " Skoru: " + scoreA + "\n";
        summaryText2.text = SettingsManager.Instance.GetBTeamName() + " Skoru: " + scoreB + "\n";
        summaryText3.text = currentRound + ". Tur Bitti\n";
        summaryText4.text = "Doðru Sayýsý: " + correctCountThisRound + "\n";
        summaryText5.text = "Tabu Sayýsý: " + tabooCountThisRound + "\n";
        summaryText6.text = "Tur Skoru: " + (isATurn ? scoreA : scoreB) + "\n";

        //buradan aldýmkodu
    }

    void OnNextRoundButtonClicked()
    {
        // Özet panelini kapat ve yeni tura baþla
        summaryPanel.SetActive(false);
        currentRound++;
        isPaused = false;
        correctCountThisRound = 0;
        tabooCountThisRound = 0;

        // Takým deðiþtir
        isATurn = !isATurn;
        timeRemaining = roundDuration; // Yeni tur süresini ayarla
        passRights = SettingsManager.Instance.passRights; // Pas haklarýný yenile
        passButton.interactable = true; // Pas butonunu yeniden etkinleþtir
        UpdatePassRightsText(); // Pas haklarýný güncelle
        ShowNextCard(); // Yeni tura geçmeden önce bir sonraki kartý göster
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);

        // Kazanan takýmý belirle
        string winnerTeam = "";
        int winnerScore = 0;

        if (scoreA >= winScore)
        {
            winnerTeam = SettingsManager.Instance.aTeamName;
            winnerScore = scoreA;
        }
        else if (scoreB >= winScore)
        {
            winnerTeam = SettingsManager.Instance.bTeamName;
            winnerScore = scoreB;
        }

        // Paneldeki bilgileri güncelle
        winnerTeamText.text = $"Kazanan: {winnerTeam}";
        winnerScoreText.text = $"Skor: {winnerScore}";
        teamAStatsText.text = $"A Takýmý - Skor: {scoreA}, Doðru: {totalCorrectCountA}, Yanlýþ: {totalTabooCountA}, Pas: {totalPassCountA}";
        teamBStatsText.text = $"B Takýmý - Skor: {scoreB}, Doðru: {totalCorrectCountB}, Yanlýþ: {totalTabooCountB}, Pas: {totalPassCountB}";
    }

  

    public void OnRestartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void OnMainMenuButtonGameOverClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }



}
