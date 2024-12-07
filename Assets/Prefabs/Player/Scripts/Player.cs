using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    
    private Rigidbody _rigidbody;
    [SerializeField] private Transform _playerModel;
    
    [SerializeField] private float _mouseSencetivity = 1f;
    [SerializeField] private Transform _cameraTransform;
    
    private bool _isStatic;

    private float _xAngle;
    private bool _grounded;

    private bool _rotRoutine;
    private bool _cameraFreeze;

    private GameObject _floor;

    private InteractiveObject _objectToInteract;

    private Flask _flask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetPosition(Vector3 pos) => transform.position = pos;

    private void Update()
    {
        if (_isStatic) return;
        
        Movement();

        if (Input.GetKeyDown(KeyCode.E) && _objectToInteract != null)
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetFlaskNull();
            Debug.Log("Delete Flask");
        }
        if(Input.GetKey(KeyCode.LeftAlt)) FreezeCamera();
        else UnFreezeCamera();
    }

    private void FreezeCamera() => _cameraFreeze = true;
    private void UnFreezeCamera() => _cameraFreeze = false;

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

        if (!_cameraFreeze) { _cameraTransform.localEulerAngles += new Vector3(0f, mouseX * _mouseSencetivity, 0f); }
        if (inputVector != Vector3.zero && !_rotRoutine)
        {
            _rotRoutine = true;
            StartCoroutine(PlayerModelRotate());
        }

        Vector3 worldVelocity = _playerModel.TransformVector(inputVector) * speed;

        _rigidbody.velocity = new Vector3(worldVelocity.x, _rigidbody.velocity.y, worldVelocity.z);


        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rigidbody.velocity += Vector3.up * _jumpSpeed;
        }
    }

    private void Interact()
    {
        _objectToInteract.Interact();
    }

    public Flask GetFlask() => _flask;
    public void SetFlusk(Flask.FlaskType type) => _flask = new Flask(type);
    public void SetFlaskNull() => _flask = null;


    private void OnCollisionEnter(Collision collision)
    {
        if (Vector3.Angle(collision.contacts[0].normal, Vector3.up) < 40f)
        {
            _grounded = true;
            _floor = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == _floor)
        {
            _grounded = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<InteractiveObject>())
        {
            _objectToInteract = collider.GetComponent<InteractiveObject>();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<InteractiveObject>())
        {
            _objectToInteract = null;
        }
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
        _rotRoutine = false;
    }

    public void SetIsStatic(bool isStatic)
    {
        _isStatic = isStatic;
        _cameraTransform.gameObject.SetActive(!isStatic);
    }
}