using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 3f;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}