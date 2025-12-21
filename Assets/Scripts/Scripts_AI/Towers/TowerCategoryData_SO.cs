using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public string title;
    public string description;
    public int cost;
    public Sprite icon;
    public GameObject towerPrefab;
    public GameObject towerCardPrefab;
}

[CreateAssetMenu(fileName = "TowerCategoryData", menuName = "TowerShop/Tower Category")]
public class TowerCategoryData_SO : ScriptableObject
{
    public List<CardInfo> cards;
}