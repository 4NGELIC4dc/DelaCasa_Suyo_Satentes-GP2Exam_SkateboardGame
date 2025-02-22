using UnityEngine;

public class Skateboard01 : Skateboard
{
    public override void ApplyModifiers()
    {
        skateSpeed = 5f; // Default speed
        jumpForce = 5f;  // Default jump
    }
}