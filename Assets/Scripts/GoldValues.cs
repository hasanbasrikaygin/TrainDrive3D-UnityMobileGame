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
    public int directionChangeCost;       // Tren y�n�n� de�i�tirme maliyeti
    public int cleanStartCost;             // Temiz ba�lang�� maliyeti

    [Header("Ads Rewards")]
    public int adsGold;
}
