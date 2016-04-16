﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ShapeController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 5f;
    public float ScaleSpeed = 5f;
    public float MinScale = 0.1f;
    public float MaxScale = 10f;

    public float JumpForce = 10f;

    public float GroundCheckDistance = .1f;
    public LayerMask WhatIsGround;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private float _moveInput;
    private float _rotateInput;
    private float _scaleInput;
    private bool _wantsToJump;

    void Update()
    {
        _moveInput = Input.GetAxis("Move");
        _rotateInput = Input.GetAxis("Rotate");
        _scaleInput = Input.GetAxis("Scale");
        _wantsToJump = Input.GetButtonDown("Jump");
    }

    public void FixedUpdate()
    {
        //Move
        float moveAmount = _moveInput*MoveSpeed;
        Vector2 moveVector = new Vector2( /*_rb.velocity.x +*/ moveAmount, _rb.velocity.y);
        moveVector.x = Mathf.Clamp(moveVector.x, -MoveSpeed, MoveSpeed);

        //Scale
        float scaleAmount = _scaleInput * ScaleSpeed;
        Vector3 newScale = transform.localScale + (new Vector3(scaleAmount, scaleAmount) * Time.fixedDeltaTime);
        newScale.x = Mathf.Clamp(newScale.x, MinScale, MaxScale);
        newScale.y = Mathf.Clamp(newScale.y, MinScale, MaxScale);
        transform.localScale = newScale;

        //Rotate
        float RotateAmount = _rotateInput*RotateSpeed;
        float newRot = transform.rotation.z + RotateAmount;
//        _rb.rotation += (newRot);
        _rb.angularVelocity += RotateAmount * Time.deltaTime;
        _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -RotateSpeed, RotateSpeed);


        //Check if grounded
        float halfHeight = transform.localScale.x / 2;
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, -Vector2.up, halfHeight + GroundCheckDistance, WhatIsGround);
        bool grounded = groundCheck;
        Debug.DrawLine(transform.position, new Vector2 (transform.position.x, transform.position.y) + (-Vector2.up * (halfHeight + GroundCheckDistance)), grounded ? Color.red : Color.green);

        _rb.velocity = moveVector;

        if (_wantsToJump && grounded)
        {
            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            _wantsToJump = false;
        }
    }
}