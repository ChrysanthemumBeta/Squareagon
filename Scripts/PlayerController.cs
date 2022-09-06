using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int speed;
    public int SprintSpeed;
    //public Slider Sprint;
    public Rigidbody2D rb;
    public Transform c;
    public float Jumpforce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    bool isGrounded;
    float lastTimeGrounded;
    public LayerMask groundLayer;
    public float rememberGroundedFor;
    public float checkGroundRadius;
    public bool IsActive = true;
    public Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = PlayerValues.PlayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            Move();
            Jump();
            BetterJump();
            CheckIfGrounded();
            IsDead();
            if (Input.GetKey(KeyCode.Escape))
            {
                InventorySaveItem[] New = new InventorySaveItem[20];
                int i = 0;
                foreach (var item in inv.InventoryData)
                {
                    if(item.Data != null)
                    {
                        New[i] = new InventorySaveItem(type: item.Data.Name, count: item.count);
                    }
                    else
                    {
                        New[i] = new InventorySaveItem(type: null, count: 0);
                    }
                    i++;
                }
                float[] Colour = new float[3];
                Colour[0] = PlayerValues.PlayerColor.r;
                Colour[1] = PlayerValues.PlayerColor.g;
                Colour[2] = PlayerValues.PlayerColor.b;
                SaveAndLoad.SaveCharacter(PlayerValues.PlayerName, Colour, New);
                SceneManager.LoadScene("CharacterSelection");
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Move()
    {
        float horizontal = 0;
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            horizontal = Input.GetAxisRaw("Horizontal") * SprintSpeed;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal") * speed;
        }
        rb.velocity = new Vector2(horizontal, rb.velocity.y);
    }
    void IsDead()
    {
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 100, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
        else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)))
        {
            rb.velocity += (lowJumpMultiplier - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }
    }
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
        }
    }
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(c.position, new Vector2(0.4f, checkGroundRadius), 0, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
}
