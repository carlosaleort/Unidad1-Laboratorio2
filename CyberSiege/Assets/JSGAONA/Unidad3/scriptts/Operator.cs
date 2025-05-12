using UnityEngine;

namespace Assets.JSGAONA.Unidad3.Scripts {

    public class Operator : StateMachineBehaviour {

        public string nameParameter = "";
        public bool value = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool(nameParameter, value);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool(nameParameter, !value);
        }
    }
}