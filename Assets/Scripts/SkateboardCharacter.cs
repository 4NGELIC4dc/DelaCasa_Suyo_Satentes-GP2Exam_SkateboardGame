using UnityEngine;
using System.IO;

public abstract class SkateboardCharacter : MonoBehaviour
{
    [Header("Character Stats")]
    public float walkSpeed = 2f;
    public float jogSpeed = 6f;
    public float jumpForce = 6f;
    public float skateboardSpeed = 6f;
    public float pushSpeed = 8f;

    protected bool isJogging = false;
    protected bool isJumping = false;
    protected bool isSkateboarding = false;
    protected bool isPushing = false;

    protected Rigidbody rb;
    protected Animator animator;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void SaveState()
    {
        PlayerPrefs.SetFloat("PositionX", transform.position.x);
        PlayerPrefs.SetFloat("PositionY", transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", transform.position.z);
        PlayerPrefs.SetInt("isSkateboarding", isSkateboarding ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    public void LoadState()
    {
        if (PlayerPrefs.HasKey("PositionX"))
        {
            float x = PlayerPrefs.GetFloat("PositionX");
            float y = PlayerPrefs.GetFloat("PositionY");
            float z = PlayerPrefs.GetFloat("PositionZ");
            transform.position = new Vector3(x, y, z);
            isSkateboarding = PlayerPrefs.GetInt("isSkateboarding") == 1;
            Debug.Log("Game Loaded");
        }
    }
}
