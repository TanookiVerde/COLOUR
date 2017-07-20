using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MirrorMovement : MonoBehaviour {

	[SerializeField] private Transform _player;
	private float _offset;
	private Animator _animator;

	void Start () {
		_offset = (transform.position - _player.position).x;
		_animator = GetComponent<Animator>();
		var playerScript = _player.GetComponent<PlayerMovement>();
		playerScript.AnimSetBool += _animator.SetBool;
		playerScript.AnimSetFloat += _animator.SetFloat;
	}

	void Update () {
		transform.position = _player.position + new Vector3(_offset, 0);
		transform.localScale =_player.localScale;
	}
}