using UnityEngine;

public class Skateboard03 : Skateboard
{
    public override void ApplyModifiers()
    {
        skateSpeed = 10f; // Increased speed
        jumpForce = 5f;   // Normal jump height
    }
}