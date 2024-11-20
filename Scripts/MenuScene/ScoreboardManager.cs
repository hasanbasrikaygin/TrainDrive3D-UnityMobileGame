using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{
    public List<ScoreEntry> scoreList = new List<ScoreEntry>(); // Skor listesi
    public GameObject scoreEntryPrefab; // Prefab referansý
    public Transform scoreBoardParent;  // Skorlarýn görüneceði panel veya scroll view
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
        LoadScores(); // Skorlarý baþlatýrken yükle
        AddScoreStart(); // Ýlk skoru listeye ekle
        DisplayScores(); // Skorlarý ekranda göster
    }

    public void AddScore(int score)
    {
        // Yeni skoru listeye ekle
        string playerName = PlayerPrefs.GetString("playerName", "Unknown");
        ScoreEntry newEntry = new ScoreEntry { playerName = playerName, score = score };
        scoreList.Add(newEntry);

        // Skorlarý sýralayýp en yüksek 10 skoru tut
        scoreList.Sort((a, b) => b.score.CompareTo(a.score)); // Büyükten küçüðe sýralama
        if (scoreList.Count > 10)
        {
            scoreList.RemoveAt(scoreList.Count - 1); // En düþük skoru sil
        }

        SaveScores(); // Güncellenen skoru kaydet
        DisplayScores(); // Skorlarý ekranda güncelle
    }

    public void SaveScores()
    {
        // Skorlarý PlayerPrefs’e kaydet
        PlayerPrefs.SetInt("scoreCount", scoreList.Count); // Skor sayýsýný sakla
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
        int scoreCount = PlayerPrefs.GetInt("scoreCount", 0); // Kaydedilen skor sayýsýný al

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

        // Ýlk 10 skoru göster
        for (int i = 0; i < scoreList.Count && i < 10; i++)
        {
            // Prefab'ý instantiate et
            GameObject newEntry = Instantiate(scoreEntryPrefab, scoreBoardParent);

            // Prefab'taki TMP_Text bileþenlerini bul
            TextMeshProUGUI[] texts = newEntry.GetComponentsInChildren<TextMeshProUGUI>();

            // texts[0] -> Rank için TMP_Text (Rank sýralamasýný ekliyoruz)
            // texts[1] -> Oyuncu ismi için TMP_Text
            // texts[2] -> Skor için TMP_Text

            texts[0].text = (i + 1).ToString(); // Rank gösterimi (1'den 10'a)
            texts[1].text = scoreList[i].playerName; // Oyuncu ismi gösterimi
            texts[2].text = scoreList[i].score.ToString(); // Skor gösterimi
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
