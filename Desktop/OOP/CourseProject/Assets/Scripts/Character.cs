using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Serializator;

public class Character : Unit, IDataPersistance
{
    public int Lives { get { return lives; } set { if (value < 15) lives = value; livesBar.Refresh(); } }
    public int Coins { get; set; }
    public int LevelNumber { get { return levelNumber; } set { levelNumber = value; } }
    private LivesBar livesBar;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float jumpForce = 15.0f;
    private bool isGrounded = false;
    private Bullet bullet;
    private int levelNumber;
    public static Character Instance { get; private set; }
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    private void Awake()
    {
        Instance= this;
        levelNumber = 1;
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("bullet");
    }
    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y += 1.0f;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0f : 1.0f);
    }
    public override void ReceiveDamage()
    {
        --Lives;
        if (Lives == 0) { SceneManager.LoadScene(2); }
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);
    }
    void Start()
    {
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        CheckGround();
    }
    void Update()
    {
        if (transform.position.y < -8) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        if (isGrounded) State = CharState.Idle;
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }
    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x < 0;
        if (isGrounded) State = CharState.Run;
    }
    private void Jump()
    {
        State = CharState.Jump;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded) State = CharState.Jump;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        this.Lives = data.characterLives;
        this.Coins = data.characterCoins;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
        data.characterLives = this.Lives;
        data.characterCoins = this.Coins;
        Debug.Log("Character Data was saved");
    }
}
public enum CharState
{
    Idle, Run, Jump
}