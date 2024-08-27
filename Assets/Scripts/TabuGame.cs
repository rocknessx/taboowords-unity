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

    public GameObject summaryPanel; // �zet paneli
    public Text summaryText; // �zet metni
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

    // Kartlar� saklayan liste
    public List<TabuCard> cards; // Burada TabuCard nesneleri listesi olmal�

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
    private bool isATurn = true; // �lk ba�ta A tak�m� oynar

    private int correctCountThisRound = 0;
    private int tabooCountThisRound = 0;
    private int totalPassCountA = 0; // Toplam pas say�s� A tak�m� i�in
    private int totalPassCountB = 0; // Toplam pas say�s� B tak�m� i�in
    private int totalCorrectCountA = 0; // Toplam do�ru say�s� A tak�m� i�in
    private int totalCorrectCountB = 0; // Toplam do�ru say�s� B tak�m� i�in
    private int totalTabooCountA = 0; // Toplam tabu say�s� A tak�m� i�in
    private int totalTabooCountB = 0; // Toplam tabu say�s� B tak�m� i�in

    void Start()
    {
        // Referanslar�n atan�p atanmad���n� kontrol edin
        if (timerText == null || scoreAText == null || scoreBText == null || passRightsText == null || tabooRightsText == null || pauseMenuPanel == null || summaryPanel == null || summaryText == null || nextRoundButton == null)
        {
            Debug.LogError("Referans eksik! L�tfen t�m UI elementlerini atad���n�zdan emin olun.");
            return;
        }

        if (cards == null || cards.Count == 0)
        {
            Debug.LogError("Kartlar listesi eksik veya bo�!");
            return;
        }

        // Butonlara i�levsellik ekleyin
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
        // SettingsManager.Instance null kontrol� yap�n
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager.Instance null! L�tfen SettingsManager'�n Scene'de mevcut oldu�undan emin olun.");
            return;
        }

        // Ayarlardan verileri al
        passRights = SettingsManager.Instance.passRights;
        tabooRights = SettingsManager.Instance.tabooRights;
        winScore = SettingsManager.Instance.winScore;
        roundDuration = SettingsManager.Instance.gameDuration; // gameDuration olarak de�i�tirildi

        // De�erlerin ge�erlili�ini kontrol et
        ValidateSettings();

        // UI G�ncellemeleri
        UpdateScoreText();
        UpdatePassRightsText();
        UpdateTabooRightsText();
        UpdateTeamNames();

        // Kart indekslerini ba�lat
        availableCardIndices = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            availableCardIndices.Add(i);
        }

        // Oyunu ba�lat
        isGameActive = true;
        isPaused = false;
        timeRemaining = roundDuration; // �lk turun s�resini ayarla
        ShowNextCard();
    }

    void Update()
    {
        if (isGameActive && !isPaused)
        {
            // Zamanlay�c�y� g�ncelle
            timeRemaining -= Time.deltaTime;

            // Timer de�erini g�ncelle
            timerText.text = Mathf.Max(Mathf.Ceil(timeRemaining), 0).ToString();

            // S�re doldu�unda tur bitir
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
        // Game Duration'�n ge�erlili�ini kontrol et
        if (roundDuration < 30 || roundDuration > 180)
        {
            Debug.LogError("Game Duration ge�ersiz! (30 ile 180 saniye aras�nda olmal�.)");
            return;
        }

        // Pass Rights'�n ge�erlili�ini kontrol et
        if (passRights < 0 || passRights > 5)
        {
            Debug.LogError("Pass Rights ge�ersiz! (0 ile 5 aras�nda olmal�.)");
            return;
        }

        // Taboo Rights'�n ge�erlili�ini kontrol et
        if (tabooRights < 0 || tabooRights > 3)
        {
            Debug.LogError("Taboo Rights ge�ersiz! (0 ile 3 aras�nda olmal�.)");
            return;
        }

        // Win Score'�n ge�erlili�ini kontrol et
        if (winScore < 25 || winScore > 250)
        {
            Debug.LogError("Win Score ge�ersiz! (25 ile 250 aras�nda olmal�.)");
            return;
        }
    }

    public void ShowNextCard()
    {
        if (availableCardIndices.Count > 0)
        {
            // Rastgele bir kart se�
            int randomIndex = Random.Range(0, availableCardIndices.Count);
            int cardIndex = availableCardIndices[randomIndex];
            availableCardIndices.RemoveAt(randomIndex); // Se�ilen kart� listeden ��kar

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

            // E�er t�m kartlar g�sterildiyse, kart listesini tekrar doldur
            if (availableCardIndices.Count == 0)
            {
                availableCardIndices = new List<int>(); // Kart indekslerini ba�lat
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

            if (isATurn) // A Tak�m�
            {
                scoreA++; // A Tak�m� skorunu 1 art�r
            }
            else // B Tak�m�
            {
                scoreB++; // B Tak�m� skorunu 1 art�r
            }
            UpdateScoreText();
            ShowNextCard(); // Do�ru butonuna bas�ld���nda bir sonraki karta ge�
        }
    }

    public void OnPassButtonClicked()
    {
        if (isGameActive)
        {
            if (passRights > 0)
            {
                ShowNextCard(); // Bir sonraki karta ge�
                passRights--; // Pas hakk�n� azalt
                UpdatePassRightsText(); // Pas haklar�n� g�ncelle

                if (passRights <= 0)
                {
                    passButton.interactable = false; // Pas hakk� bitti�inde butonu devre d��� b�rak
                }
            }
        }
    }

    public void OnTabooButtonClicked()
    {
        if (isGameActive)
        {
            tabooCountThisRound++;

            if (isATurn) // A Tak�m�
            {
                scoreA--; // A Tak�m� skorunu 1 azalt
            }
            else // B Tak�m�
            {
                scoreB--; // B Tak�m� skorunu 1 azalt
            }
            UpdateScoreText();
            ShowNextCard(); // Tabu butonuna bas�ld���nda bir sonraki karta ge�
        }
    }

    void EndRound()
    {
        if (isGameActive)
        {
            // Oyunu duraklat
            isPaused = true;

            // �zet panelini a�
            ShowSummary();
        }
    }

    void EndGame()
    {
        isGameActive = false;
        pauseMenuPanel.SetActive(false);
        ShowSummary(); // Oyunu bitirdi�inde �zet panelini g�ster
    }

    void UpdateScoreText()
    {
        if (scoreAText != null && scoreBText != null)
        {
            scoreAText.text = "A Tak�m�: " + scoreA;
            scoreBText.text = "B Tak�m�: " + scoreB;
        }
    }

    void UpdatePassRightsText()
    {
        if (passRightsText != null)
        {
            passRightsText.text = "Pas Hakk�: " + passRights;
        }
    }

    void UpdateTabooRightsText()
    {
        if (tabooRightsText != null)
        {
            tabooRightsText.text = "Tabu Hakk�: " + tabooRights;
        }
    }

    void UpdateTeamNames()
    {
        if (teamAText != null && teamBText != null)
        {
            teamAText.text = "A Tak�m�: " + SettingsManager.Instance.GetATeamName();
            teamBText.text = "B Tak�m�: " + SettingsManager.Instance.GetBTeamName();
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
        SceneManager.LoadScene(0); // Ana men� sahnesine geri d�n
    }

    // �zet Panelini G�ster
    void ShowSummary()
    {
        // �zet panelini a� ve bilgileri g�ster
        summaryPanel.SetActive(true);

        // �zeti g�ncelle
        summaryText.text = "Oyun �zeti\n";
        summaryText1.text = SettingsManager.Instance.GetATeamName() + " Skoru: " + scoreA + "\n";
        summaryText2.text = SettingsManager.Instance.GetBTeamName() + " Skoru: " + scoreB + "\n";
        summaryText3.text = currentRound + ". Tur Bitti\n";
        summaryText4.text = "Do�ru Say�s�: " + correctCountThisRound + "\n";
        summaryText5.text = "Tabu Say�s�: " + tabooCountThisRound + "\n";
        summaryText6.text = "Tur Skoru: " + (isATurn ? scoreA : scoreB) + "\n";

        //buradan ald�mkodu
    }

    void OnNextRoundButtonClicked()
    {
        // �zet panelini kapat ve yeni tura ba�la
        summaryPanel.SetActive(false);
        currentRound++;
        isPaused = false;
        correctCountThisRound = 0;
        tabooCountThisRound = 0;

        // Tak�m de�i�tir
        isATurn = !isATurn;
        timeRemaining = roundDuration; // Yeni tur s�resini ayarla
        passRights = SettingsManager.Instance.passRights; // Pas haklar�n� yenile
        passButton.interactable = true; // Pas butonunu yeniden etkinle�tir
        UpdatePassRightsText(); // Pas haklar�n� g�ncelle
        ShowNextCard(); // Yeni tura ge�meden �nce bir sonraki kart� g�ster
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);

        // Kazanan tak�m� belirle
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

        // Paneldeki bilgileri g�ncelle
        winnerTeamText.text = $"Kazanan: {winnerTeam}";
        winnerScoreText.text = $"Skor: {winnerScore}";
        teamAStatsText.text = $"A Tak�m� - Skor: {scoreA}, Do�ru: {totalCorrectCountA}, Yanl��: {totalTabooCountA}, Pas: {totalPassCountA}";
        teamBStatsText.text = $"B Tak�m� - Skor: {scoreB}, Do�ru: {totalCorrectCountB}, Yanl��: {totalTabooCountB}, Pas: {totalPassCountB}";
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
