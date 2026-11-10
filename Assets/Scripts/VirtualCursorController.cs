
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualCursorController : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private float sensitivity = 1f;

    private Transform TransformCursor { get; set; }
    private Camera MainCamera { get; set; }
    private bool IsActive { get; set; }

    private void Awake()
    {
        TransformCursor = transform;
        MainCamera = Camera.main;
    }

    private void Start()
    {
        IsActive = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsActive || !ctx.performed)
            return;

        Vector2 delta = ctx.ReadValue<Vector2>();

        Vector3 position = TransformCursor.position;
        position += GetWorldDelta(delta);

        TransformCursor.position = ClampPosition(position);
    }

    private Vector3 GetWorldDelta(Vector2 delta)
    {
        float worldHeight = MainCamera.orthographicSize * 2f;
        float worldWidth = worldHeight * MainCamera.aspect;

        return new Vector3(
            delta.x * worldWidth / Screen.width,
            delta.y * worldHeight / Screen.height
        ) * sensitivity;
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        float halfHeight = MainCamera.orthographicSize;
        float halfWidth = halfHeight * MainCamera.aspect;

        position.x = Mathf.Clamp(position.x, -halfWidth, halfWidth);
        position.y = Mathf.Clamp(position.y, -halfHeight, halfHeight);

        return position;
    }

    public void OnCursor()
    {
        IsActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OffCursor()
    {
        IsActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsActive = false;
    }
}
