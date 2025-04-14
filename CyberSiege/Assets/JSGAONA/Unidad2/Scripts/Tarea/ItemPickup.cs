using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IItem item = GetComponent<IItem>();
            item?.ApplyEffect(other.gameObject);
        }
    }
}
