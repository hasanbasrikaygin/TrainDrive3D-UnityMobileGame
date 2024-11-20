using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{
    public List<ScoreEntry> scoreList = new List<ScoreEntry>(); // Skor listesi
    public GameObject scoreEntryPrefab; // Prefab referans�
    public Transform scoreBoardParent;  // Skorlar�n g�r�nece�i panel veya scroll view
    public static ScoreBoardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadScores(); // Skorlar� ba�lat�rken y�kle
        AddScoreStart(); // �lk skoru listeye ekle
        DisplayScores(); // Skorlar� ekranda g�ster
    }

    public void AddScore(int score)
    {
        // Yeni skoru listeye ekle
        string playerName = PlayerPrefs.GetString("playerName", "Unknown");
        ScoreEntry newEntry = new ScoreEntry { playerName = playerName, score = score };
        scoreList.Add(newEntry);

        // Skorlar� s�ralay�p en y�ksek 10 skoru tut
        scoreList.Sort((a, b) => b.score.CompareTo(a.score)); // B�y�kten k����e s�ralama
        if (scoreList.Count > 10)
        {
            scoreList.RemoveAt(scoreList.Count - 1); // En d���k skoru sil
        }

        SaveScores(); // G�ncellenen skoru kaydet
        DisplayScores(); // Skorlar� ekranda g�ncelle
    }

    public void SaveScores()
    {
        // Skorlar� PlayerPrefs�e kaydet
        PlayerPrefs.SetInt("scoreCount", scoreList.Count); // Skor say�s�n� sakla
        for (int i = 0; i < scoreList.Count; i++)
        {
            PlayerPrefs.SetString($"scorePlayerName{i}", scoreList[i].playerName);
            PlayerPrefs.SetInt($"scoreValue{i}", scoreList[i].score);
        }
        PlayerPrefs.Save();
    }

    public void LoadScores()
    {
        
        scoreList.Clear();
        int scoreCount = PlayerPrefs.GetInt("scoreCount", 0); // Kaydedilen skor say�s�n� al

        for (int i = 0; i < scoreCount; i++)
        {
            string playerName = PlayerPrefs.GetString($"scorePlayerName{i}", "Unknown");
            int score = PlayerPrefs.GetInt($"scoreValue{i}", 0);
            scoreList.Add(new ScoreEntry { playerName = playerName, score = score });
        }
    }

    public void AddScoreStart()
    {
        int lastScore = PlayerPrefs.GetInt("lastScore",-1);
        if(lastScore > 0) 
        {
            AddScore(lastScore);
            PlayerPrefs.SetInt("lastScore", -1);
            SaveScores();
            PlayerPrefs.Save();
        }
    }

    //public void ScoreTest()
    //{
    //    AddScore(20);
    //}

    public void DisplayScores()
    {
        if (scoreBoardParent == null) return;

        // Mevcut listeyi temizle
        foreach (Transform child in scoreBoardParent)
        {
            Destroy(child.gameObject);
        }

        // �lk 10 skoru g�ster
        for (int i = 0; i < scoreList.Count && i < 10; i++)
        {
            // Prefab'� instantiate et
            GameObject newEntry = Instantiate(scoreEntryPrefab, scoreBoardParent);

            // Prefab'taki TMP_Text bile�enlerini bul
            TextMeshProUGUI[] texts = newEntry.GetComponentsInChildren<TextMeshProUGUI>();

            // texts[0] -> Rank i�in TMP_Text (Rank s�ralamas�n� ekliyoruz)
            // texts[1] -> Oyuncu ismi i�in TMP_Text
            // texts[2] -> Skor i�in TMP_Text

            texts[0].text = (i + 1).ToString(); // Rank g�sterimi (1'den 10'a)
            texts[1].text = scoreList[i].playerName; // Oyuncu ismi g�sterimi
            texts[2].text = scoreList[i].score.ToString(); // Skor g�sterimi
        }
    }

}

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class ScoreList
{
    public List<ScoreEntry> scores;
}
