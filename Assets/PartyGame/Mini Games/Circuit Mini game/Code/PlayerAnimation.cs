using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed = 5;
    private void Start()
    {
        animator = this.GetComponentInParent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
    }
}
