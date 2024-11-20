using UnityEngine;

[CreateAssetMenu(fileName = "GoldValues", menuName = "Game/GoldValues")]
public class GoldValues : ScriptableObject
{
    [Header("Basic Mod")]
    public int passengerGold ;
    public int dropWagonsGold ;
    
    [Header("2048 Mod")]
    public int wagonGold ;

    [Header("GameOver Cost")]
    public int directionChangeCost;       // Tren yönünü deðiþtirme maliyeti
    public int cleanStartCost;             // Temiz baþlangýç maliyeti

    [Header("Ads Rewards")]
    public int adsGold;
}
