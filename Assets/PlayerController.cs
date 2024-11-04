using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bombPrefab;          // 炸弹预制件
    public Transform bombSpawnPoint;       // 炸弹生成位置
    public float throwForce = 15f;         // 投掷力度
    public float moveSpeed = 5f;           // 移动速度
    public float airControlFactor = 0.5f;  // 空中控制系数
    public float mouseSensitivity = 2f;    // 鼠标灵敏度

    private Rigidbody rb;
    private bool isGrounded;
    private Bomb currentBomb;
    private float xRotation = 0f;

    public int playerScore;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 隐藏并锁定光标
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        HandleBombThrow();
        HandleBombDetonate();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 旋转摄像机（上下）
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // 旋转玩家（左右）
        transform.Rotate(Vector3.up * mouseX);
    }

    void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * h + transform.forward * v;

        if (isGrounded)
        {
            Vector3 move = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + move);
        }
        else
        {
            // 空中控制
            rb.AddForce(moveDirection * moveSpeed * airControlFactor);
        }
    }

    void HandleBombThrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 生成炸弹
            GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();

            // 投掷方向
            Vector3 throwDirection = Camera.main.transform.forward;
            bombRb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);

            // 保存当前炸弹
            currentBomb = bomb.GetComponent<Bomb>();
        }
    }

    void HandleBombDetonate()
    {
        if (Input.GetMouseButtonDown(1) && currentBomb != null)
        {
            // 引爆炸弹
            currentBomb.Detonate();
            currentBomb = null;
        }
    }

    // 碰撞检测，判断是否着地
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
