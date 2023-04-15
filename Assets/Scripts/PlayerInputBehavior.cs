using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBehavior : MonoBehaviour
{

    [Tooltip("Inputs whose magnitude is less than the deadZone are ignored.")]
    [Range(0f, 1f)]
    public float deadZone = 0.05f;
    public float moveSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    [Tooltip("The player cannot depress the camera further than this.")]
    public float minimumElevation = -60f;
    [Tooltip("The player cannot raise the camera higher than this.")]
    public float maximumElevation = 90f;

    [Header("Component References")]
    public CharacterController characterController;
    public Transform playerTransform;
    public Transform cameraAnchor;

    private Vector3 move = Vector3.zero;
    private float cameraTurn;
    private float cameraLift;

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
        if (Mathf.Abs(cameraTurn) > deadZone)
        {
            // Rotation about the Y axis (up)
            var turnDirection = cameraTurn <= 0
                ? Quaternion.AngleAxis(-90, Vector3.up)
                : Quaternion.AngleAxis(90, Vector3.up);
            playerTransform.rotation = Quaternion.Slerp(
                playerTransform.rotation,
                playerTransform.rotation * turnDirection,
                Time.deltaTime * turnSpeed * Mathf.Abs(cameraTurn));
        }

        if (Mathf.Abs(cameraLift) > deadZone)
        {
            // Rotation about the X axis (right)
            var turnDirection = Quaternion.Euler(cameraLift < 0 ? 90 : -90, 0f, 0f);
            cameraAnchor.rotation = Quaternion.Slerp(
                cameraAnchor.rotation,
                cameraAnchor.rotation * turnDirection,
                Time.deltaTime * turnSpeed * Mathf.Abs(cameraLift));
        }


        if (Mathf.Abs(move.magnitude) > deadZone)
        {
            characterController.Move(playerTransform.rotation * (moveSpeed * Time.deltaTime * move));
        }
    }
}
