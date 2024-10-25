using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //------------------------------------------------ controller ---------------------------------------------------
    private CharacterController _controller;
    private Transform _camera;

    // -------------------------------------------------inputs -------------------------------------------------------
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _jumpHeight = 1; 
    [SerializeField] private float _turnSmoothTime = 0.5f;
    private float _turnSmoothVelocity;
    
    private float _horizontal;
    private float _vertical;

    //------------------------------------------------- cosas de gravedad----------------------------------------------
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //------------------------------------------------- cosas del GroundSensor-----------------------------------------
    [SerializeField] Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] LayerMask _groundLayer;
  

    //------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");



            IsGrounded();

            if(Input.GetButtonDown("Jump") && IsGrounded())
            {
                Jump();

            }


          // Movement();
          // AimMovement();
          if(Input.GetButton("Fire2"))
          {
            AimMovement();

          }
          else
          {
            Movement();
          }



            Gravity();


    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        if( direction != Vector3.zero)
        {

         float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
         float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
         transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
         Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

         _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);

        }

       

    }
     void AimMovement()
    {
         Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
       

        if(direction != Vector3.zero)
        { 
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }
       
 
        


        

       

    void Gravity()
    {
        if(!IsGrounded())
        {
          _playerGravity.y += _gravity * Time.deltaTime;
        }
       else if(IsGrounded() && _playerGravity.y < 0)
       {
         _playerGravity.y = -1;
       }

        _controller.Move(_playerGravity * Time.deltaTime);

    } 
    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);

    }
   
    
}