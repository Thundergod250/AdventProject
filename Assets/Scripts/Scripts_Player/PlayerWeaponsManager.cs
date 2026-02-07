using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Assign your weapons here")] 
    public List<GameObject> weapons = new List<GameObject>(); 
    private int currentWeaponIndex = 0; 
    
    // Input Action reference (set this up in your Input Actions asset)
    //public InputActionAsset inputActions; 
    //private InputActionMap actionMap; 
    //private InputAction weapon1Action; 
    //private InputAction weapon2Action; 
    //private InputAction weapon3Action; 

    //private void Awake() { 
    //    // Get your action map (replace "Player" with the name of your action map)
    //    actionMap = inputActions.FindActionMap("Player"); // Bind actions (replace with your actual action names in the Input Actions asset)
    //    weapon1Action = actionMap.FindAction("Weapon1"); 
    //    weapon2Action = actionMap.FindAction("Weapon2"); 
    //    weapon3Action = actionMap.FindAction("Weapon3"); 
    //} 
    //private void OnEnable() 
    //{ weapon1Action.performed += ctx => SwitchWeapon(0); 
    //    weapon2Action.performed += ctx => SwitchWeapon(1); 
    //    weapon3Action.performed += ctx => SwitchWeapon(2); 
    //    weapon1Action.Enable(); weapon2Action.Enable(); 
    //    weapon3Action.Enable(); 
    //} 
    //private void OnDisable() { 
    //    weapon1Action.Disable(); 
    //    weapon2Action.Disable();
    //    weapon3Action.Disable(); } 
    private void SwitchWeapon(int index) { 
        if (index < 0 || index >= weapons.Count) return; // Disable all weapons
       
        foreach (var weapon in weapons) {
            weapon.SetActive(false);
        
        } // Enable selected weapon
        
        weapons[index].SetActive(true); 
        currentWeaponIndex = index; 
    }
}
