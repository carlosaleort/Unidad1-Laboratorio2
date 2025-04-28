using UnityEngine;
using Assets.JSGAONA.Unidad2.Scripts.FMS.Temple;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    // Se emplea este scriptable object para generar el FMS del estado de reposo
    [CreateAssetMenu(fileName = "NewPatrolState", menuName = "CyberSiege/AI/Patrol State")]
    public class AIStatePatrol : AIStateIdle {
        
        private AIStatePatrolTemplate patrolTemplate;


        // Cuando el estado se inicializa
        public override void Initialize(EnemyAI enemyMove, AIStateTemplate template) {
            base.Initialize(enemyMove, template);
            EnemyAi = enemyMove;
            patrolTemplate = (AIStatePatrolTemplate) template;
        }


        // Cuando el estado Entra
        public override void Enter() {
            EnemyAi.AdjustAgent(1, patrolTemplate.ModifiedSpeed, patrolTemplate.Acceleration);
            EnemyAi.SetDestinationPatrol();
        }


        // Cuando el estado se Ejecuta
        public override void Execute() {
            base.Execute();
            if(EnemyAi.GetResetEnemyAi(false)) {
                EnemyAi.SetDestinationPatrol();
            }
        }


        // Cuando el estado se Sale
        public override void Exit() {

        }
    }
}