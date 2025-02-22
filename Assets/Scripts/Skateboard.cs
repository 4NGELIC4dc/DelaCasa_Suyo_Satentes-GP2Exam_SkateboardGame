using UnityEngine;

public abstract class Skateboard : MonoBehaviour
{
    public float skateSpeed = 5f;
    public float jumpForce = 5f;

    public virtual void ApplyModifiers() { } // Prevents null reference issues

    public float GetSpeed() => skateSpeed;
    public float GetJumpHeight() => jumpForce;
    public float GetBoostedSpeed() => skateSpeed * 1.5f;
}
