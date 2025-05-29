using UnityEngine;
using TMPro;

public class VictoryText : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    void Start()
    {
        string winner = PlayerPrefs.GetString("winnerName", "Nadie");
        winnerText.text = "🏆 Ganó " + winner + " 🏆";
    }
}
