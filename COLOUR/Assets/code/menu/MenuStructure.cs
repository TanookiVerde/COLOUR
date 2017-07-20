using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuStructure : MonoBehaviour {

	[Header("CutScene Preferences and Content")]
	[SerializeField] private float _timeBetweenFrames;
	[SerializeField] private float _increaseAlphaSpeed;
	[SerializeField] private List<Sprite> _frameList;
	[SerializeField] private List<string> _frameDescriptionList;
	[SerializeField] private Image _imageComponent;
	[SerializeField] private Text _textComponent;

	[Header("Menu Showign Preferences and Objects")]
	[SerializeField] private GameObject _menu;
	[SerializeField] private Transform _menuInitial;
	[SerializeField] private Transform _menuFinal;

	private void Start(){
		StartCoroutine( CutsceneStart() );
	}
	private IEnumerator CutsceneStart(){
		int frameIndex = 0;
		int size, length;
		ToggleMenu(false);
		ToggleCutscene(true);
		while(frameIndex < _frameList.Count){
			size = 0;
			length = _frameDescriptionList[frameIndex].Length;
			Sprite temp = _frameList[frameIndex];
			_imageComponent.sprite = temp;
			_imageComponent.color = new Color(1,1,1,0);
			while(_imageComponent.color.a < 1 || size < length){
				_imageComponent.color += new Color(0,0,0,1*Time.deltaTime*_increaseAlphaSpeed);
				_textComponent.text = GetCutString(_frameDescriptionList[frameIndex], size);
				if(size < length) size++;
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(_timeBetweenFrames);
			size--;
			while(_imageComponent.color.a > 0 || size > 0){
				_imageComponent.color -= new Color(0,0,0,1*Time.deltaTime*_increaseAlphaSpeed);
				_textComponent.text = GetCutString(_frameDescriptionList[frameIndex],size);
				if(size > 0) size--;
				yield return new WaitForEndOfFrame();
			}
			frameIndex++;
		}
		ToggleMenu(true);
		yield return ShowMenu();
	}
	private IEnumerator ShowMenu(){
		ToggleCutscene(false);
		_menu.transform.position = _menuInitial.position;

		while(!Mathf.Approximately(_menu.transform.position.y,_menuFinal.position.y)){
			_menu.transform.position = Vector3.Lerp(_menu.transform.position,_menuFinal.position,0.1f);
			yield return new WaitForEndOfFrame();
		}

	}
	private void ToggleMenu(bool b){
		_menu.SetActive(b);
	}
	private void ToggleCutscene(bool b){
		_imageComponent.gameObject.SetActive(b);
	}
	private string GetCutString(string original, int size){
		return original.Substring(0,size);
	}
}
