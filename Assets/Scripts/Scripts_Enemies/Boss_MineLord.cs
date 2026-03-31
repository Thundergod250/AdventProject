using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class Boss_MineLord : MonoBehaviour
{
    public enum BossState { Idle, Attacking, Jumping, Staggered, Defeated }
    private BossState currentState = BossState.Idle;

    [Header("Target Reference")]
    public Transform Player;

    [Header("Boss Settings")]
    public Transform bossBody; // child holding appearance
    public GameObject bulletPrefab;
    public List<GameObject> JumpPoints;
    public float health = 100f;
    public float bobbingHeight = 0.5f;
    public float bobbingSpeed = 3f;
    public GameObject turretObject;

    private bool isAttacking = false;
    [SerializeField] private bool canShoot;
    private Vector3 initialBodyPos;
    public float attackInterval = 2f;

    void Start()
    {
        initialBodyPos = bossBody.localPosition;
        StartCoroutine(BossRoutine());
    }

    void Update()
    {
        if (currentState != BossState.Defeated)
            FacePlayer();
        //if(currentState == BossState.Attacking || isAttacking == true)
        //    Shooting();
    }

    private void FacePlayer()
    {
        Vector3 direction = Player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }

    private IEnumerator BossRoutine()
    {
        while (health > 0)
        {
            if (currentState == BossState.Idle)
            {
                yield return StartCoroutine(Attack());
                yield return StartCoroutine(Jump());
            }
            yield return null;
        }
        yield return StartCoroutine(Death());
    }

    private IEnumerator Attack()
    {
        currentState = BossState.Attacking;
        isAttacking = true;

        float interval = health <= 50 ? attackInterval / 2f : attackInterval;
        float timer = 0f;

        // Swing forward (0 → 180)
        float duration = interval / 2f;
        float t = 0f;
        while (t < 1f)
        {
            float angle = Mathf.Lerp(-90f, 90f, t);
            bossBody.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            t += Time.deltaTime / duration;
            timer += Time.deltaTime;

            Shooting();

            yield return null;
        }

        // Swing back (180 → 0)
        t = 0f;
        while (t < 1f)
        {
            float angle = Mathf.Lerp(90f, -90f, t);
            bossBody.transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            Shooting();

            t += Time.deltaTime / duration;
            timer += Time.deltaTime;
            yield return null;
        }

        bossBody.localPosition = initialBodyPos;
        isAttacking = false;
        currentState = BossState.Idle;
    }

    private void Shooting()
    {
        if (canShoot)
        {
            // Fire bullets forward
            GameObject bullet = Instantiate(bulletPrefab, turretObject.transform.position, turretObject.transform.rotation);
            // Initialize projectile direction
            ProjectileBase pb = bullet.GetComponent<ProjectileBase>();
            if (pb != null)
            {
                pb.SetDirection(turretObject.transform.forward); // Pass the enemy's facing direction
            }

            StartCoroutine(ShootingInterval());
        }
        //float intervalStart = 0f;
        //float intervalEnd = 1.5f;

        //if (intervalStart < intervalEnd)
        //{
        //    // Fire bullets forward
        //    GameObject bullet = Instantiate(bulletPrefab, turretObject.transform.position, turretObject.transform.rotation);
        //    // Initialize projectile direction
        //    ProjectileBase pb = bullet.GetComponent<ProjectileBase>();
        //    if (pb != null)
        //    {
        //        pb.SetDirection(turretObject.transform.forward); // Pass the enemy's facing direction
        //    }
        //    intervalStart++;
        //}
        //else
        //{
        //    intervalStart = 0;
        //}
        
    }

    private IEnumerator ShootingInterval()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }

    private IEnumerator Jump()
    {
        currentState = BossState.Jumping;
        int jumpPoints = Random.Range(0, 3);
        GameObject targetPoint = JumpPoints[Random.Range(0, jumpPoints)];
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPoint.transform.position;

        float t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        currentState = BossState.Idle;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            currentState = BossState.Defeated;
        }
        else if (health <= 50 && currentState != BossState.Staggered)
        {
            StartCoroutine(Staggered());
        }
    }

    private IEnumerator Staggered()
    {
        currentState = BossState.Staggered;
        float staggerTime = 3f;
        float timer = 0f;

        while (timer < staggerTime)
        {
            bossBody.Rotate(Vector3.forward * 200f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        bossBody.rotation = Quaternion.identity;
        currentState = BossState.Idle;
    }

    private IEnumerator Death()
    {
        currentState = BossState.Defeated;

        // Turn over
        float timer = 0f;
        while (timer < 2f)
        {
            bossBody.Rotate(Vector3.right * 100f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Explosion
        Destroy(gameObject, 2f);
    }
}
