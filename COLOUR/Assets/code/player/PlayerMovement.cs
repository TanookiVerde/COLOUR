using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour {

	[Header("Movement Properties")]
	[SerializeField] private float _walkingVelocity;
	[SerializeField] private float _jumpMaxHeight;
	[SerializeField] private float _jumpMinForce;
	[SerializeField] private float _jumpForceIncrement;
	[SerializeField] private float _mass;
	[SerializeField] private float _gravityScale;
	[Range(.1f,1)]
	[SerializeField] private float _circleCastRadius;
	[SerializeField] private Transform _floorDetection;
	[SerializeField] private Transform _ceilingDetection;
	[SerializeField] private bool _jumpHeadHit;
	[SerializeField] private bool _facingRight;
	[SerializeField] private float _jumpThreshold;

	private Rigidbody2D _myRB2D;
	private bool _jumped = false;
	private float _jumpStartTime;
	public bool climbing;

	private void Start(){
		InitializePhysicsProperties();
	}
	private void Update(){
		Walk();
		Jump();
		Climb();
	}
	private void InitializePhysicsProperties(){
		_myRB2D = GetComponent<Rigidbody2D>();
		_myRB2D.gravityScale = _gravityScale;
		_myRB2D.mass = _mass;
	}
	private void Jump(){
		if(HitHead()){
			_jumpHeadHit = true;
		}
		if(!_jumped && Input.GetButtonDown("Jump") && IsGrounded())
		{
			_myRB2D.AddForce(Vector2.up * _jumpMinForce);
			_jumped = true;
			_jumpStartTime = Time.time;
		}
		else if(_jumped && !_jumpHeadHit && Input.GetButton("Jump") && Time.time - _jumpStartTime < _jumpThreshold)
		{
			_myRB2D.AddForce(Vector2.up * _jumpForceIncrement);	
		}
		else if(Time.time - _jumpStartTime >= _jumpThreshold && IsGrounded() && !HitHead())
		{
			_jumped = false;
			_jumpHeadHit = false;
		}
	}
	private bool IsGrounded(){
		RaycastHit2D hit = Physics2D.CircleCast(_floorDetection.position,_circleCastRadius,Vector2.down,_circleCastRadius,1<<LayerMask.NameToLayer("Ground"));
		if(hit.collider != null){
			return true;
		}
		return false;
	}
	private bool HitHead(){
		RaycastHit2D hit = Physics2D.CircleCast(_ceilingDetection.position,_circleCastRadius,Vector2.up,_circleCastRadius,1<<LayerMask.NameToLayer("Ground"));
		if(hit.collider != null){
			return true;
		}
		return false;
	}
	private void Climb(){
		if(( climbing && Input.GetButtonDown("Jump") ) || !IsGrounded()){
			climbing = false;
			_myRB2D.gravityScale = _gravityScale;
			_myRB2D.constraints = RigidbodyConstraints2D.None;
			_myRB2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if(climbing){
			_myRB2D.AddForce(Vector2.up*Input.GetAxisRaw("Vertical")*_walkingVelocity);
			_myRB2D.gravityScale = 0;
			_myRB2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		}
	}
	public void SetClimbing(bool value){
		climbing = value;
	}
	private void Flip(){
		Vector3 s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;
		_facingRight = !_facingRight;
	}
	private void Walk(){
		float horz = Input.GetAxisRaw("Horizontal");

		Vector2 direction = new Vector2(horz, 0).normalized;
		_myRB2D.AddForce(direction*_walkingVelocity);

		if((horz < 0 && _facingRight) || (horz > 0 && !_facingRight))
		Flip();
	}
}
