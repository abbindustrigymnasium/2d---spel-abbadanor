using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;

    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _yMov = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", _xMov);
        animator.SetFloat("Vertical", _yMov);
        animator.SetFloat("Speed", new Vector2(_xMov, _yMov).sqrMagnitude);

        transform.position += new Vector3(_xMov, _yMov, 0) * Time.deltaTime * moveSpeed;
    }
}
