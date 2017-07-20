using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IObstacle {
	[SerializeField] Transform ladderX;

	public void Interact(GameObject player){
		if(!player.GetComponent<PlayerMovement>().climbing && Input.GetKeyDown(KeyCode.UpArrow)){
			player.transform.position = new Vector3(ladderX.position.x,player.transform.position.y,0);
			player.GetComponent<PlayerMovement>().SetClimbing(true);
		}
		
	}
}
