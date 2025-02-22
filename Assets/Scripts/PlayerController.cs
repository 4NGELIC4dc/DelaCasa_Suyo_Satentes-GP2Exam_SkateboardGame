using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float jogSpeed = 6f;
    public float jumpForce = 6f;
    public float skateSpeed = 6f;
    private float currentSpeed;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private bool isSkateboarding = false;
    private bool isPushing = false;

    private GameObject activeSkateboardInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        int playerLayer = LayerMask.NameToLayer("Player");
        int skateboardLayer = LayerMask.NameToLayer("Skateboard");
        Physics.IgnoreLayerCollision(playerLayer, skateboardLayer, true);

        if (GameManager.instance != null)
        {
            GameManager.instance.LoadSelectedSkateboard();
            activeSkateboardInstance = GameManager.instance.GetSelectedSkateboardInstance();

            if (activeSkateboardInstance != null)
            {
                Rigidbody skateboardRb = activeSkateboardInstance.GetComponent<Rigidbody>();
                if (skateboardRb != null)
                {
                    skateboardRb.isKinematic = true; 
                }
                DontDestroyOnLoad(activeSkateboardInstance);
                activeSkateboardInstance.SetActive(false);
            }
        }
    }
    void Update()
    {
        MovePlayer();
        HandleJump();
        HandleSkateboarding();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        currentSpeed = isSkateboarding ? (Input.GetKey(KeyCode.LeftShift) ? skateSpeed * 1.5f : skateSpeed) : (Input.GetKey(KeyCode.LeftShift) ? jogSpeed : walkSpeed);
        isPushing = isSkateboarding && Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized * currentSpeed;

        if (movement.magnitude > 0)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            transform.forward = movement;
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        animator.SetBool("isIdle", movement.magnitude == 0 && !isSkateboarding);
        animator.SetBool("isWalking", movement.magnitude > 0 && !Input.GetKey(KeyCode.LeftShift) && !isSkateboarding);
        animator.SetBool("isJogging", movement.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && !isSkateboarding);
        animator.SetBool("isSkateboarding", isSkateboarding && !isJumping);
        animator.SetBool("isPushing", isPushing && !isJumping);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;

            animator.SetBool("isJumping", true);
            animator.SetBool("isSkateboarding", isSkateboarding);
            animator.SetBool("isPushing", false);
        }
    }

    void HandleSkateboarding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isSkateboarding = !isSkateboarding;

            if (isSkateboarding && activeSkateboardInstance != null)
            {
                activeSkateboardInstance.transform.SetParent(transform);
                activeSkateboardInstance.transform.localPosition = new Vector3(0, 0, 0.2f);
                activeSkateboardInstance.transform.localRotation = Quaternion.identity;

                Rigidbody skateboardRb = activeSkateboardInstance.GetComponent<Rigidbody>();
                Collider skateboardCollider = activeSkateboardInstance.GetComponent<Collider>();

                if (skateboardRb != null)
                {
                    skateboardRb.isKinematic = true; 
                    skateboardRb.useGravity = false;
                    skateboardRb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                if (skateboardCollider != null)
                {
                    skateboardCollider.enabled = false; 
                }
                activeSkateboardInstance.SetActive(true);
            }
            else if (activeSkateboardInstance != null)
            {
                activeSkateboardInstance.transform.SetParent(null);
                Rigidbody skateboardRb = activeSkateboardInstance.GetComponent<Rigidbody>();
                Collider skateboardCollider = activeSkateboardInstance.GetComponent<Collider>();

                if (skateboardRb != null)
                {
                    skateboardRb.isKinematic = false; 
                    skateboardRb.useGravity = true;
                    skateboardRb.constraints = RigidbodyConstraints.None;
                }
                if (skateboardCollider != null)
                {
                    skateboardCollider.enabled = true;
                }

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
                {
                    activeSkateboardInstance.transform.position = hit.point + new Vector3(0, 0.05f, 0);
                }
                activeSkateboardInstance.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);

            if (isSkateboarding && activeSkateboardInstance != null)
            {
                Rigidbody skateboardRb = activeSkateboardInstance.GetComponent<Rigidbody>();
                if (skateboardRb != null)
                {
                    skateboardRb.isKinematic = true;
                    skateboardRb.useGravity = false;
                }
                activeSkateboardInstance.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            }
        }
    }

    public void CollectBurger()
    {
        if (isSkateboarding && activeSkateboardInstance != null)
        {
            activeSkateboardInstance.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
    }

}