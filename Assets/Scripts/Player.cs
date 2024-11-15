using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody _rigidbody;
    [SerializeField] private Transform _playerModel;
    [SerializeField] private float _mouseSencetivity = 1f;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _jumpSpeed;

    private float _xAngle;
    private bool _grounded;

    private bool _rotCoroutine;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float speed = _speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 2;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);

        _cameraTransform.localEulerAngles += new Vector3(0f, mouseX * _mouseSencetivity, 0f);
        if (inputVector != Vector3.zero && !_rotCoroutine)
        {
            _rotCoroutine = true;
            StartCoroutine(PlayerModelRotate());
        }

        Vector3 worldVelocity = _playerModel.TransformVector(inputVector) * speed;

        _rigidbody.velocity = new Vector3(worldVelocity.x, _rigidbody.velocity.y, worldVelocity.z);
        

        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rigidbody.velocity += Vector3.up * _jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Vector3.Angle(collision.contacts[0].normal, Vector3.up) < 40f)
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _grounded = false;
    }

    private IEnumerator PlayerModelRotate()
    {
        Vector3 startRotation = _playerModel.localEulerAngles;
        Vector3 endRotation = _cameraTransform.localEulerAngles;

        if (Mathf.Abs(startRotation.y - endRotation.y) > 180f)
        {
            if (endRotation.y < startRotation.y)
            {
                endRotation.y += 360;
            }
            else
            {
                endRotation.y -= 360f;
            }
        }

        for (float t = 0; t <= 1f; t += (Time.deltaTime / 0.1f))
        {
            _playerModel.localEulerAngles = new Vector3(_playerModel.localEulerAngles.x,
                Mathf.Lerp(startRotation.y, endRotation.y, t),
                _playerModel.localEulerAngles.z);
            yield return null;
        }


        _playerModel.localEulerAngles = endRotation;
        _rotCoroutine = false;
    }
}