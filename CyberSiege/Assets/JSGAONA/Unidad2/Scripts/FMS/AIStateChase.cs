using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    [CreateAssetMenu(fileName = "NewChaseState", menuName = "CyberSiege/EstadoPerseguir")]
    public class AIStateChase : AIState {


        // Cuando el estado se inicializa
        public override void Initialize(EnemyAI entity) {
            EnemyAi = entity;
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