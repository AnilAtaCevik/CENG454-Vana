using UnityEngine;

public class AABullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 100f; // Mermi uçuş hızı
    [SerializeField] private float damage = 5f;  // Merminin vereceği hasar
    [SerializeField] private float lifeTime = 3f; // Vuramazsa 3 saniye sonra havada yok olsun (performans için)

    void Start()
    {
        // Doğduktan 3 saniye sonra kendi kendini imha et (çöp temizliği)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Her frame'de mermiyi kendi önü (Z ekseni) yönünde ilerlet
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Mermi başka bir objenin içinden geçtiğinde tetiklenir (Is Trigger açık olmalı)
    private void OnTriggerEnter(Collider other)
    {
        // Eğer çarptığımız objenin etiketi "Player" ise
        if (other.CompareTag("Player"))
        {
            // Çarptığımız objenin üstündeki Can sistemini bul
            HelicopterHealth targetHealth = other.GetComponent<HelicopterHealth>();

            if (targetHealth != null)
            {
                // Hasarı ver
                targetHealth.TakeDamage(damage);
            }

            // Çarptıktan sonra mermiyi yok et
            Destroy(gameObject);
        }
    }
}
