using UnityEngine;
namespace Assets.JSGAONA.Unidad2.Scripts.FMS.Temple {

    // Se emplea este scriptable object para generar el FMS del estado de reposo
    [CreateAssetMenu(fileName = "NewPatrolTemple", menuName = "CyberSiege/Behavior/Behavior Patrol")]
    public class AIStatePatrolTemplate : AIStateIdleTemplate {
        
        [SerializeField] [Range(0.1f, 5.0f)] public float ModifiedSpeed = 1.0f;
        [SerializeField] public float Acceleration = 80.0f;

    }
}