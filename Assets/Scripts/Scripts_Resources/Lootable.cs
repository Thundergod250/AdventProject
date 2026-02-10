using UnityEngine;
using UnityEngine.Events;

public class Lootable : MonoBehaviour
{
    [Header("Loot Settings")]
    [SerializeField] private int maxLoots = 1;  
    private int currentLoots = 0;

    [Header("Events")]
    public UnityEvent EvtOnLooted; 
    
    public void _OnLooted()
    {
        currentLoots++;
        EvtOnLooted?.Invoke();
        if (currentLoots >= maxLoots) 
            Destroy(gameObject);
    }
}