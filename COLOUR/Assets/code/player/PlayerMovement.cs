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
	[SerializeField] private float _mass;
	[SerializeField] private float _gravityScale;
	[SerializeField] private bool _facingRight;
	

	[Header("Jump Properties")]
	[SerializeField] private AudioClip _jumpAudio;
	[SerializeField] private float _circleCastRadius;
	[SerializeField] public Transform floorDetection;
	[SerializeField] private Transform _ceilingDetection;
	[SerializeField] private float _jumpThreshold;
	[SerializeField] private bool _jumpHeadHit;
	[SerializeField] private float _jumpMinForce;
	[SerializeField] private float _jumpForceIncrement;
	
	private Rigidbody2D _myRB2D;
	private Animator _myAnimator;
	private SpriteRenderer _mySR;
	private bool _jumped = false;
	private float _jumpStartTime;
	public bool climbing;
	
	public delegate void AnimationChange<T>(string name, T value);
 	public event AnimationChange<float> AnimSetFloat;
 	public event AnimationChange<bool> AnimSetBool;

	private void Start(){
		GetAnimator();
		GetSpriteRenderer();
		InitializePhysicsProperties();
		StartCoroutine( AppearInStage() );
		AnimSetBool += _myAnimator.SetBool;
		AnimSetFloat += _myAnimator.SetFloat;
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
			AnimSetBool("jumping", true);
			GetComponent<AudioSource>().clip = _jumpAudio;
			GetComponent<AudioSource>().Play();
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
			AnimSetBool("jumping", false);
		}
	}
	private bool IsGrounded(){
		RaycastHit2D hit_a = Physics2D.Raycast(floorDetection.position - Vector3.right*0.2f, Vector2.down, .2f, 1<<LayerMask.NameToLayer("Ground"));
		RaycastHit2D hit_b = Physics2D.Raycast(floorDetection.position + Vector3.right*0.2f, Vector2.down, .2f, 1<<LayerMask.NameToLayer("Ground"));
		RaycastHit2D hit_e_a = Physics2D.Raycast(floorDetection.position - Vector3.right*0.2f, Vector2.down, .2f, 1<<LayerMask.NameToLayer("EffectorGround"));
		RaycastHit2D hit_e_b = Physics2D.Raycast(floorDetection.position + Vector3.right*0.2f, Vector2.down, .2f, 1<<LayerMask.NameToLayer("EffectorGround"));
		if(hit_a.collider != null || hit_b.collider != null || hit_e_a.collider != null || hit_e_b.collider != null){
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
			AnimSetBool("climbing", false);
		}
		if(climbing){
			_myRB2D.AddForce(Vector2.up*Input.GetAxisRaw("Vertical")*_walkingVelocity/2);
			_myRB2D.gravityScale = 0;
			_myRB2D.constraints = RigidbodyConstraints2D.FreezePositionX;
			AnimSetBool("climbing", true);
			AnimSetBool("moving", false);
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

		if(horz != 0) {
			AnimSetFloat("direction", horz);
			AnimSetBool("moving", true);
		}else{
			AnimSetBool("moving", false);
		}

		//if((horz < 0 && _facingRight) || (horz > 0 && !_facingRight))
		//Flip();
	}
	private void GetAnimator(){
		_myAnimator = GetComponent<Animator>();
	}
	private void GetSpriteRenderer(){
		_mySR = GetComponent<SpriteRenderer>();
	}
	private IEnumerator AppearInStage(){
		float opacity = 0;
		_mySR.color = new Color(_mySR.color.r,_mySR.color.g,_mySR.color.b,0);

		while(!Mathf.Approximately(opacity,1)){
			opacity = Mathf.Lerp(opacity,1,0.05f);
			_mySR.color = new Color(_mySR.color.r,_mySR.color.g,_mySR.color.b,opacity);
			yield return new WaitForEndOfFrame();
		}
	}
}
