using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerInputBehavior : MonoBehaviour
{

    public float moveSpeed = 2f;
    [Tooltip("Camera movement speed in degrees per second.")]
    public float turnSpeed = 360f;
    public float minPitch = -30f;
    public float maxPitch = 70f;
    public Transform cameraTarget;

    private CharacterController controller;
    private Animator animator;
    private Vector3 move = Vector3.zero;
    private Vector2 look = Vector2.zero;
    private float yaw;   // left-right
    private float pitch; // up-down

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        yaw = controller.transform.eulerAngles.y;
        pitch = cameraTarget.eulerAngles.x;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var move2 = context.ReadValue<Vector2>();
        move = new Vector3(move2.x, 0f, move2.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Debug.Log(move);
        animator.SetFloat("moveX", move.x);
        animator.SetFloat("moveY", move.z);
    }

    private void OnAnimatorMove()
    {
        controller.Move(transform.rotation * (moveSpeed * Time.deltaTime * move));

        // Look
        yaw += (look.x * turnSpeed * Time.deltaTime);
        pitch = Mathf.Clamp(pitch + look.y * turnSpeed * Time.deltaTime, minPitch, maxPitch);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
