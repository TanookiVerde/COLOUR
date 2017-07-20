using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform _player;

	private float _yOffset;
	private Vector3 _startPos; 
	private bool _follow = false;

	void Start()
	{
		_startPos = transform.position;
	}

	void Update () {
		
		if(!_follow)
		{ 
			_yOffset = (transform.position - _player.position).y;
			_follow = true;
		} 
		else if (_player.position.y <= _startPos.y)
		{
			_follow = false;
			transform.position = _startPos;
			return;
		}

		Vector3 p = transform.position;
		p.y = _player.position.y + _yOffset;
		transform.position = p;
	}
}
