using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;
    public float jumpForce = 10;

    void Update() 
    { 
        Move(); 
        Jump();
    } 

    void Move() { 
        float x = Input.GetAxisRaw("Horizontal"); 
        float moveBy = x * speed; 
        rb.velocity = new Vector2(moveBy, rb.velocity.y); 
    }

    void Jump() { 
    if (Input.GetKeyDown(KeyCode.Space)) { 
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
    } 
}
}
