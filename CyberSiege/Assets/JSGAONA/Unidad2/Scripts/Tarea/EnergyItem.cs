using Assets.JSGAONA.Unidad2.Scripts;
using UnityEngine;

public class EnergyItem : MonoBehaviour, IItem
{
    [SerializeField] private int energyAmount = 30;

    public void ApplyEffect(GameObject player)
    {
        PlayerCombat combat = player.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.RestoreResource(energyAmount); // Lo agregamos también en PlayerCombat
            Destroy(gameObject);
        }
    }
}