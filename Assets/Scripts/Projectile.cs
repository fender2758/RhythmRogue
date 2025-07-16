using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f; // 몇 초 뒤 사라질지 설정

    void Start()
    {
        Destroy(gameObject, lifetime); // 일정 시간 뒤 자동 제거
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        NoteObject note = other.GetComponent<NoteObject>();
        if (note != null && note.life)
        {
            if (note.hitPoint > 1){
                note.hitPoint--;
                note.UpdateHPText();
            }
            
        }
    }
}
