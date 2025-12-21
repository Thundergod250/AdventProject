using System;
using UnityEngine;

public class BillBoardToCamera : MonoBehaviour
{
    private void Update()
    {
        GameManager.Instance.CameraManager.BillboardToCamera(gameObject);
    }
}
