using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputBehavior : MonoBehaviour
{

    [Tooltip("Inputs whose magnitude is less than the deadZone are ignored.")]
    [Range(0f, 1f)]
    public float deadZone = 0.05f;
    public float moveSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    [Tooltip("The player cannot depress the camera further than this.")]
    public float minimumElevation = -90f;
    [Tooltip("The player cannot raise the camera higher than this.")]
    public float maximumElevation = 90f;

    [Header("Component References")]
    public Transform cameraTarget;

    private CharacterController controller;
    private Vector3 move = Vector3.zero;
    private float cameraTurn;
    private float cameraLift;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var vec = context.ReadValue<Vector2>();
        move = new Vector3(vec.x, 0f, vec.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var vec = context.ReadValue<Vector2>();
        cameraTurn = vec.x;
        cameraLift = vec.y;
    }

    void Update()
    {
        // Rotation about the Y axis (lLeft and right")
        if (Mathf.Abs(cameraTurn) > deadZone)
        {
            var turnDirection = Quaternion.AngleAxis(cameraTurn <= 0 ? -90 : 90, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                transform.rotation * turnDirection,
                Time.deltaTime * turnSpeed * Mathf.Abs(cameraTurn));
        }

        // Rotation about the X axis ("up and down")
        if (Mathf.Abs(cameraLift) > deadZone)
        {
            // Rotate the local cameraAnchor rotation towards the minimum or maximum angle on the X axis. Because we
            // only ever set the X axis rotation on the camera Anchor (putting all Y-axis rotation on the player
            // transform), we don't need to acknowledge either of the other axes.
            cameraTarget.localRotation = Quaternion.Slerp(
                cameraTarget.localRotation,
                Quaternion.Euler(cameraLift < 0 ? maximumElevation : minimumElevation, 0f, 0f),
                Time.deltaTime * turnSpeed * Mathf.Abs(cameraLift));
        }


        if (Mathf.Abs(move.magnitude) > deadZone)
        {
            controller.Move(transform.rotation * (moveSpeed * Time.deltaTime * move));
        }
    }
}
