using UnityEngine;
using Assets.JSGAONA.Unidad2.Scripts.FMS.Temple;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    // Se emplea este scriptable object para generar el FMS del estado de ataque basico
    [CreateAssetMenu(fileName = "NewBasicAttackState", menuName = "CyberSiege/AI/BasicAttack State")]
    public class AIStateBasicAttack : AIState {

        // Referencia del script del sistema de combate del enemigo
        protected EnemyCombat combatEnemy;
        private AIStateBasicAttackTemplate basicAttackTemple;


        // Cuando el estado se inicializa
        public override void Initialize(EnemyAI enemyAi, AIStateTemplate template) {
            EnemyAi = enemyAi;
            basicAttackTemple = (AIStateBasicAttackTemplate) template;
            combatEnemy = enemyAi.gameObject.GetComponent<EnemyCombat>();
        }


        // Cuando el estado Entra
        public override void Enter() {
            EnemyAi.AdjustAgent(0, 80, 0);
        }


        // Cuando el estado se Ejecuta
        public override void Execute() {
            // Se valida el intervalo de chequeo
            if(!EnemyAi.CheckInterval()) return;
            float distance = EnemyAi.GetDistance();
            
            // Se valida si se encuentra a rango de alcance o no exista una pared
            if(EnemyAi.Chase(distance)){
                EnemyAi.ChangeState(EnemyAi.GetChaseState());
            }else{                
                Attack ();
            }
        }


        // Cuando el estado se Sale
        public override void Exit() {

        }


        protected virtual void Attack () {
            // Se verifica si se puede realizar un ataque
            if(combatEnemy.CheckIntervalBasicAttack(basicAttackTemple.SpeedAttack)){
                combatEnemy.BasicAttack();
            }
        }
    }
}