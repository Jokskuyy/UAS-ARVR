using UnityEngine;

public class TransformObject : MonoBehaviour
{
    [Header("Target Values (editable di Inspector)")]
    public Vector3 targetPosition;
    public Vector3 targetRotation; // in degrees (Euler)
    public Vector3 targetScale = Vector3.one;

    [Header("Trigger Actions (centang di Inspector)")]
    public bool setPosition;
    public bool setRotation;
    public bool setScale;

    [Header("Optional: Save initial transform for reset")]
    public bool saveInitialOnStart = true;

    // initial values (disimpan jika saveInitialOnStart = true)
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        if (saveInitialOnStart)
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialScale = transform.localScale;
        }
    }

    void Update()
    {
        if (setPosition)
        {
            SetPosition();       // parameterless
            setPosition = false; // auto-disable
        }

        if (setRotation)
        {
            SetRotation();
            setRotation = false;
        }

        if (setScale)
        {
            SetScale();
            setScale = false;
        }
    }

    // -----------------------
    // Parameterless functions (callable from Inspector booleans or UnityEvent)
    // -----------------------
    public void SetPosition()
    {
        transform.position = targetPosition;
    }

    public void SetRotation()
    {
        transform.rotation = Quaternion.Euler(targetRotation);
    }

    public void SetScale()
    {
        transform.localScale = targetScale;
    }

    // -----------------------
    // Functions with parameters (call from scripts)
    // Note: UnityEvents can't select methods with parameters easily,
    // so use the parameterless ones for UnityEvents/Inspector triggers.
    // -----------------------
    public void SetPositionTo(Vector3 newPos)
    {
        transform.position = newPos;
    }

    public void SetRotationTo(Vector3 newRot)
    {
        transform.rotation = Quaternion.Euler(newRot);
    }

    public void SetScaleTo(Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    // -----------------------
    // Optional resets (parameterless)
    // -----------------------
    public void ResetPosition()
    {
        if (saveInitialOnStart) transform.position = initialPosition;
    }

    public void ResetRotation()
    {
        if (saveInitialOnStart) transform.rotation = initialRotation;
    }

    public void ResetScale()
    {
        if (saveInitialOnStart) transform.localScale = initialScale;
    }
}
