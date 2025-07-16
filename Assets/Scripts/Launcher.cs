using UnityEngine;
using UnityEngine.InputSystem; // 새 Input System 네임스페이스

public class Launcher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;

    private bool isFiring = false;
    private float fireTimer = 0f;

    void Update()
    {
        // ✅ 새 Input System
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isFiring = !isFiring;
            Debug.Log("Firing: " + isFiring);
        }

        if (isFiring)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                Fire();
            }
        }
    }

    void Fire()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;

        // 🔄 -45도 ~ +45도 랜덤 각도 생성
        float angle = Random.Range(-45f, 45f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // 🔁 Launcher 자체 회전
        transform.rotation = rotation;

        // 발사체 생성 + 방향 설정
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, rotation);

        // Rigidbody2D를 사용해 속도 부여
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = projectile.GetComponent<Projectile>().speed;
            rb.velocity = rotation * Vector2.up * speed;
        }
    }
}
