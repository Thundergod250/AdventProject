using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class IntEvent : UnityEvent<int> { }

public class TowerPlasticCollector : TowerUtilityBase
{
    [Header("Events")]
    public UnityEvent EvtOnGarbageObjectCollide;
    public UnityEvent<GameObject> EvtPassObjectReference;
    public IntEvent EvtOnAddMoney;

    [Header("Damage Settings")]
    public int TowerDamageLevel = 1;
    public int StartingDamageValue = 20;
    public int DamageIncreasePerLevel = 10;

    [Header("Garbage Filter Settings")]
    public bool checkGroupOnly = false;
    public bool checkSubtypeOnly = false;
    public bool checkBothGroupAndSubtype = false;

    /*
    public GarbageObject.GarbageGroup requiredGroup;
    public GarbageObject.GarbageSubtype requiredSubtype;
    */

    private void OnTriggerEnter(Collider other)
    {
        GarbageObject garbage = other.GetComponent<GarbageObject>();
        if (garbage == null) return;

        if (!IsValidGarbage(garbage)) return;

        // ✅ Passed filter, trigger events
        EvtOnGarbageObjectCollide?.Invoke();
        EvtPassObjectReference?.Invoke(other.gameObject);

        int moneyToAdd = CalculateMoneyValue();
        EvtOnAddMoney?.Invoke(moneyToAdd);
    }

    private bool IsValidGarbage(GarbageObject garbage)
    {
        // Accept all garbage if no checks are enabled
        if (!checkGroupOnly && !checkSubtypeOnly && !checkBothGroupAndSubtype)
            return true;

        /*if (checkGroupOnly)
            return garbage.Group == requiredGroup;

        if (checkSubtypeOnly)
            return garbage.Subtype == requiredSubtype;

        if (checkBothGroupAndSubtype)
            return garbage.Group == requiredGroup && garbage.Subtype == requiredSubtype;*/

        return false;
    }

    public void _IncreaseTowerDamageLevel()
    {
        TowerDamageLevel++;
        int newValue = CalculateMoneyValue();
        Debug.Log($"[TowerPlasticCollector] TowerDamageLevel increased to {TowerDamageLevel}. New money value: {newValue}");
    }

    public void _DisableGarbageObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public int CalculateMoneyValue()
    {
        return StartingDamageValue + (TowerDamageLevel - 1) * DamageIncreasePerLevel;
    }
}
