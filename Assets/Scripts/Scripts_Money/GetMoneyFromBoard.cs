using System;
using UnityEngine;

public class GetMoneyFromBoard : MonoBehaviour
{
    [SerializeField] private CampFunding campFunding;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()) 
            campFunding.CollectMoney();
    }
}
