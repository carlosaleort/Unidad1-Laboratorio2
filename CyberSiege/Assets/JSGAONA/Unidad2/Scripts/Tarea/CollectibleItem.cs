using UnityEngine;

public class CollectibleItem : MonoBehaviour, IItem
{
    public static int totalCollected = 0;

    public void ApplyEffect(GameObject player)
    {
        totalCollected++;
        Debug.Log("Coleccionables: " + totalCollected);
        Destroy(gameObject);
    }
}