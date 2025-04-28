using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    // Se emplea este scriptable object para generar el FMS del estado de ataque basico
    [CreateAssetMenu(fileName = "NewRangeAttackState", menuName = "CyberSiege/AI/RangeAttack State")]
    public class AIStateRangeAttack : AIStateBasicAttack {

        protected override void Attack() {
            combatEnemy.UseWeapon();
        }
    }
}