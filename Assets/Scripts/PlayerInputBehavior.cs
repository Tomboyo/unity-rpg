using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBehavior : MonoBehaviour
{

    [Header("Component References")]
    public CharacterController characterController;

    private Vector3 move;

    public void OnMove(InputAction.CallbackContext context)
    {
        var vec = context.ReadValue<Vector2>();
        move = new Vector3(vec.x, 0f, vec.y);
    }

    void Update()
    {
        characterController.Move(move * Time.deltaTime);
    }
}
