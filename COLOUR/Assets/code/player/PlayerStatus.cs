using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
	[SerializeField] private int life;
	[SerializeField] private float knockBackIntensity;
	
	private Rigidbody2D myRB2D;

	private void Start(){
		myRB2D = GetComponent<Rigidbody2D>();
	}
	public void Damage(Vector2 direction){
		life--;
		myRB2D.AddForce(-direction*knockBackIntensity);
		if(life == 0){
			StartCoroutine( GameOver() );
		}
	}
	private void OnCollisionEnter2D(Collision2D other){	
		if(other.gameObject.GetComponent<IObstacle>() != null){
			other.gameObject.GetComponent<IObstacle>().Interact(this.gameObject);
			
		}
	}
	private void OnTriggerStay2D(Collider2D other){	
		if(other.gameObject.GetComponent<Ladder>() != null){
			other.gameObject.GetComponent<Ladder>().Interact(this.gameObject);
			
		}
	}
	private IEnumerator GameOver(){
		yield return new WaitForEndOfFrame();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("_main");
	}
}
