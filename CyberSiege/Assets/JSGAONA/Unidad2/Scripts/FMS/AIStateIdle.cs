using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {
    
    [CreateAssetMenu(fileName = "NewIdleState", menuName = "CyberSiege/EstadoReposo")]
    public class AIStateIdle : AIState {


        // Cuando el estado se inicializa
        public override void Initialize(EnemyAI enemyMove) {
            EnemyAi = enemyMove;
        }


        // Cuando el estado Entra
        public override void Enter() {

        }


        // Cuando el estado se Ejecuta
        public override void Execute() {
            
        }


        // Cuando el estado se Sale
        public override void Exit() {
            
        }
    }
}