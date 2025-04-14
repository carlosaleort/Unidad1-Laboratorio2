// Script de ejemplo para proyectil
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    private void OnEnable()
    {
        Invoke(nameof(Desactivar), lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // aplicar daño
            Debug.Log("Impactó al jugador");
        }

        Desactivar();
    }

    private void Desactivar()
    {
        ObjectPool.Instance.ReturnToPool(gameObject, gameObject);
    }
}
