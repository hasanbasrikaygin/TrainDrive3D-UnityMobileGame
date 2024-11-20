using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // �sim giri�i i�in InputField

    private void Start()
    {
        // Oyun ba�lad���nda PlayerPrefs'teki ad� InputField'a y�kleyelim
        if (PlayerPrefs.HasKey("playerName"))
        {
            nameInputField.text = PlayerPrefs.GetString("playerName");
        }
        else
        {
            nameInputField.text = "Unknown"; // Varsay�lan isim
        }

        nameInputField.onValueChanged.AddListener(UpdatePlayerName); // �sim her de�i�ti�inde g�ncelle
    }

    public void UpdatePlayerName(string newName)
    {
        // �smi PlayerPrefs'e kaydet
        PlayerPrefs.SetString("playerName", newName);
        PlayerPrefs.Save();
    }
}
