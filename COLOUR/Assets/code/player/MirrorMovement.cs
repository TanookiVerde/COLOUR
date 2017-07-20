using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour {

	[SerializeField] private Transform _player;
	private float _offset;

	void Start () {
		_offset = (transform.position - _player.position).x;
	}

	void Update () {
		transform.position = _player.transform.position + new Vector3(_offset, 0);
	}
}
