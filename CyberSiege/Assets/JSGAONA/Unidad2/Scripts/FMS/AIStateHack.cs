using UnityEngine;
using Assets.JSGAONA.Unidad2.Scripts.FMS.Temple;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    // Se emplea este scriptable object para generar el FMS del estado de reposo
    [CreateAssetMenu(fileName = "NewHackState", menuName = "CyberSiege/AI/Hack State")]
    public class AIStateHack : AIState {
        
        // Cuando el estado se inicializa
        public override void Initialize(EnemyAI enemyMove, AIStateTemplate template) {
            EnemyAi = enemyMove;
        }


        // Cuando el estado Entra
        public override void Enter() {
            // Se activa el efecto de particulas
        }


        // Cuando el estado se Ejecuta
        public override void Execute() {
            // Se valida la duracion de hackeo
            if(EnemyAi.HackingTime()) {
                EnemyAi.ChangeState(EnemyAi.GetChaseState());
            }
        }


        // Cuando el estado se Sale
        public override void Exit() {
            // Se desactiva el efecto de particulas
            EnemyAi.ItsHacked = false;
        }
    }
}