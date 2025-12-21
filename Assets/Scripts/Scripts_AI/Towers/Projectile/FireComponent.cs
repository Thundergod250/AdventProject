using System.Threading.Tasks;
using UnityEngine;

public class FireComponent : MonoBehaviour
{
    [Header("Settings")]
    public int damagePerTick = 10;
    public float intervalDelay = 1f;
    public int totalTicks = 5;

    [SerializeField] protected EnemyBase enemyBase;

}

