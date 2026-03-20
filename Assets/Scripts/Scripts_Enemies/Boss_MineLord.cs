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
    public float attackInterval = 2f;
    public float bobbingHeight = 0.5f;
    public float bobbingSpeed = 3f;

    private bool isAttacking = false;
    private Vector3 initialBodyPos;

    void Start()
    {
        initialBodyPos = bossBody.localPosition;
        StartCoroutine(BossRoutine());
    }

    void Update()
    {
        if (currentState != BossState.Defeated)
            FacePlayer();
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

        while (timer < interval)
        {
            // Bobbing effect
            float bob = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
            bossBody.localPosition = initialBodyPos + new Vector3(0, bob, 0);

            // Fire bullets in all directions
            int bulletCount = 12;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * (360f / bulletCount);
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().linearVelocity = dir * 10f;
            }

            timer += 1f;
            yield return new WaitForSeconds(1f);
        }

        bossBody.localPosition = initialBodyPos;
        isAttacking = false;
        currentState = BossState.Idle;
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
