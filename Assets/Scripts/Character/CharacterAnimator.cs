using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;

        private void Update()
        {
            var vel = characterController.GetVelocity();
            animator.SetFloat("velocityX", vel.x);
            animator.SetFloat("velocityY", vel.y);
            
            animator.SetBool("isGrounded", characterController.GetIsGrounded());
        }
    }
}