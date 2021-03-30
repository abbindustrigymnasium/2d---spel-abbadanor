using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public Rigidbody2D rb;
    public Animator animator;

    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _yMov = Input.GetAxisRaw("Vertical");

        Vector3 inputVector = new Vector3(_xMov, _yMov, 0);
        inputVector.Normalize();

        transform.position += inputVector * Time.deltaTime * moveSpeed;

        animator.SetFloat("Horizontal", _xMov);
        animator.SetFloat("Vertical", _yMov);
        animator.SetFloat("Speed", new Vector2(_xMov, _yMov).sqrMagnitude);
    }
}
