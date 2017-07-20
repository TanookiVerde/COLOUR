using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BatBehavior : MonoBehaviour, IObstacle {
	[SerializeField] private float _avoidDistance;
    [SerializeField] private LayerMask _avoidLayers;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _eye;

    private Rigidbody2D _rg2b;
    private bool _facingRight = false;

    void Start()
    {
        _rg2b = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rg2b.velocity = Vector3.right * _speed;
        if(!_facingRight)
            _rg2b.velocity *= -1;
        Avoid();
    }

    void Avoid()
    {
        Vector3 dir = Vector3.right;
        if(!_facingRight)
            dir *= -1;
        Debug.DrawRay(_eye.position, dir*_avoidDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(_eye.position, dir, _avoidDistance, 1 << LayerMask.NameToLayer("Ground"));
        if(hit.collider && !hit.collider.isTrigger)
            Flip();
    }

    private void Flip()
	{
		Vector3 s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;
		_facingRight = !_facingRight;
	}
    public void Interact(GameObject player){
        Vector2 direction = transform.position-player.transform.position;
		player.GetComponent<PlayerStatus>().Damage(direction);
    }
}
