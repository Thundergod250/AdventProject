using UnityEngine;

public class DebugTexts : MonoBehaviour
{
    public void _PrintActivated() => Debug.Log("Activated");
    public void _PrintActivated(string text) => Debug.Log("Activated: " + text);
}

