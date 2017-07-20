using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour {

	enum JumpState
	{
		NONE, DOWN, UP_TIME_OUT, AIR
	};

	[Header("Movement Properties")]
	[SerializeField] private float _walkingVelocity;
	[SerializeField] private float _jumpMaxForce;
	[SerializeField] private float _jumpMinForce;
	[SerializeField] private float _mass;
	[SerializeField] private float _gravityScale;
	[Range(.1f,1)]
	[SerializeField] private float _circleCastRadius;
	[SerializeField] private Transform _floorDetection;
	[SerializeField] private float _jumpThreshold;
	private Rigidbody2D _myRB2D;

	private JumpState _currentJumpState = JumpState.NONE;
	private float _jumpElapsedTime = 0f;
	private float _jumpStartTime = 0f;


	private void Start(){
		InitializePhysicsProperties();
	}
	private void Update(){
		Walk();
		Jump();
	}
	private void InitializePhysicsProperties(){
		_myRB2D = GetComponent<Rigidbody2D>();
		_myRB2D.gravityScale = _gravityScale;
		_myRB2D.mass = _mass;
	}
	private void Walk(){
		Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;
		_myRB2D.AddForce(direction*_walkingVelocity);
	}
	private void Jump(){
		switch(_currentJumpState)
		{
			case JumpState.NONE:
				if(Input.GetButtonDown("Jump") && IsGrounded()){
					_currentJumpState = JumpState.DOWN;
					_jumpStartTime = Time.time;
				}
				break;
			case JumpState.DOWN:
				_jumpElapsedTime = Time.time - _jumpStartTime;
				if(!IsGrounded())
				{
					_currentJumpState = JumpState.NONE;
				}			
				else if(Input.GetButtonUp("Jump") || _jumpElapsedTime >= _jumpThreshold)
				{
					_currentJumpState = JumpState.UP_TIME_OUT;
				}
				break;
			case JumpState.UP_TIME_OUT:
				float jumpForce = _jumpMinForce + (_jumpMaxForce - _jumpMinForce) * _jumpElapsedTime/_jumpThreshold;
				_myRB2D.AddForce(Vector2.up * jumpForce);
				_currentJumpState = JumpState.AIR;
				break;
			case JumpState.AIR:
				if(IsGrounded())
					_currentJumpState = JumpState.NONE;
				break;
		}

	}
	private bool IsGrounded(){
		RaycastHit2D hit = Physics2D.CircleCast(_floorDetection.position,_circleCastRadius,Vector2.down,_circleCastRadius,1<<LayerMask.NameToLayer("Ground"));
		if(hit.collider != null){
			return true;
		}
		return false;
	}








}
