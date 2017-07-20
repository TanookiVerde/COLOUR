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
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _mass;
	[SerializeField] private float _gravityScale;
	[Range(.1f,1)]
	[SerializeField] private float _circleCastRadius;
	[SerializeField] private Transform _floorDetection;
	private Rigidbody2D _myRB2D;


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
		if(Input.GetButtonDown("Jump") && IsGrounded()){
			_myRB2D.AddForce(Vector2.up*_jumpForce);
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
