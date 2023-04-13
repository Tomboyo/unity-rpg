using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBehavior : MonoBehaviour
{

    [Header("Movement Parameters")]
    public float moveSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    [Header("Component References")]
    public CharacterController characterController;
    public Transform playerTransform;

    private Vector3 move = Vector3.zero;
    private Vector3 turn;

    public void OnMove(InputAction.CallbackContext context)
    {
        var vec = context.ReadValue<Vector2>();
        move = new Vector3(vec.x, 0f, vec.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var vec = context.ReadValue<Vector2>();
        turn = new Vector3(vec.x, 0f, 0f);
    }

    void Update()
    {
        var turnBy = Quaternion.FromToRotation(
            Vector3.forward,
            turn);
        playerTransform.rotation = Quaternion.Slerp(
            playerTransform.rotation,
            playerTransform.rotation * turnBy,
            Time.deltaTime * turnSpeed * turn.magnitude);

        characterController.Move(playerTransform.rotation * (moveSpeed * Time.deltaTime * move));
    }
}
