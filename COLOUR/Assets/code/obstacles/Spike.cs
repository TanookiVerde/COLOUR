using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour,IObstacle {

	public void Interact(GameObject player){
		Vector2 direction = transform.position-player.transform.position;
		player.GetComponent<PlayerStatus>().Damage(direction);
	}
}
