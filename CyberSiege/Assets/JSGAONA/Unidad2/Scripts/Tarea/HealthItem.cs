using Assets.JSGAONA.Unidad2.Scripts;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    [SerializeField] private int healAmount = 50;

    public void ApplyEffect(GameObject player)
    {
        PlayerCombat combat = player.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.Heal(healAmount); // Método que implementaremos en PlayerCombat
            Destroy(gameObject);
        }
    }
}
