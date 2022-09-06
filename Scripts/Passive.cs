using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    [SerializeField]Vector3 Dir;
    Rigidbody2D rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //rb.MovePosition( transform.position + (Dir * speed * Time.deltaTime));
        float horizontal = (Dir.x * speed);
        rb.velocity = new Vector2(horizontal, rb.velocity.y);

    }
}
