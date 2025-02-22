using UnityEngine;

public class Skateboard02 : Skateboard
{
    public override void ApplyModifiers()
    {
        skateSpeed = 5f; // Same speed
        jumpForce = 7.5f; // Increased jump height
    }
}