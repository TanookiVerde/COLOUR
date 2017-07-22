

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum COLOR {
	RED, GREEN, BLUE, N_COLORS
};

public class MapManager : MonoBehaviour {

	[Header("Configs")]
	[SerializeField] private int _nSections = 10;
	[SerializeField] private List<GameObject> _sectionsPrefabs = new List<GameObject>();
	[SerializeField] private Vector2 _sectionDimentions;
	[SerializeField] private Transform _redSpawner;
	[SerializeField] private Transform _greenSpawner;
	[SerializeField] private Transform _blueSpawner;
    [SerializeField] private GameObject _walls;

    [Header("Colors")]
    [SerializeField] private Color _red;
    [SerializeField] private Color _blue;
    [SerializeField] private Color _green;

    private int[] _mapSections;

	void Start () {
		_mapSections = ChooseSections();
		InstantiateScections();
	}
	
	void Update () {
		
	}
	
	int[] ChooseSections()
	{
		int[] sections = new int[_nSections];
		for(int i = 0; i < _nSections; i++)
			sections[i] = Random.Range(0, _sectionsPrefabs.Count);
		return sections;
	}

	void InstantiateScections()
	{
		Vector3 offset;

		for(int i = 0; i < _nSections; i++)
		{
			offset = new Vector3(0, i * _sectionDimentions.y);
			GameObject prefab = _sectionsPrefabs[_mapSections[i]];

			GameObject redSection = (GameObject) Instantiate(prefab, _redSpawner.position + offset, Quaternion.identity, transform);

            Instantiate(_walls, _walls.transform.position + offset, Quaternion.identity);

			//It was coloring the prefab instead of the GameObject, it was suposed to color a copy of it, 
			//and to all be colored the same way the redSection is passed as original object on Instantiate
			//instead of the prefab 
			Colorize(redSection);

			GameObject greebSection = (GameObject) Instantiate(redSection, _greenSpawner.position + offset, Quaternion.identity, transform);
			GameObject blueSection = (GameObject) Instantiate(redSection, _blueSpawner.position + offset, Quaternion.identity, transform);

			HidePlatforms(redSection, COLOR.RED);
			HidePlatforms(greebSection, COLOR.GREEN);
			HidePlatforms(blueSection, COLOR.BLUE);
		}
	}

	void Colorize(GameObject s)
	{
		SpriteRenderer[] platforms = s.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer sr in platforms)
		{
			if(sr.tag == "Black")
			{
				sr.color = Color.black;
			}
			else if (sr.tag == "Randomize")
			{
				COLOR clr = (COLOR) Random.Range(0, (int) COLOR.N_COLORS);
				switch(clr)
				{
					case COLOR.RED:
						sr.color = _red;
						break;
					case COLOR.GREEN:
						sr.color = _green;
						break;
					case COLOR.BLUE:
						sr.color = _blue;
						break;
				}
			}
		}
	}

	void HidePlatforms(GameObject s, COLOR c)
	{
		SpriteRenderer[] platforms = s.GetComponentsInChildren<SpriteRenderer>();
		COLOR clr;
		foreach(SpriteRenderer sr in platforms)
		{
			//k = r * 1 + g * 2 + b * 3 - 1
			//black (0, 0, 0, 1) -> k = -1
			//red 	(1, 0, 0, 1) -> k = 0
			//green (0, 1, 0, 1) -> k = 1
			//blue 	(0, 0, 1, 1) -> k = 2
			int k = (int) Vector4.Dot((Vector4) sr.color, new Vector4(1, 2, 3, 0));

            Debug.Log("color: " + sr.color + ", k: " + k + ", COLOR: " + (COLOR) k);
            //continue;

			if(k < 3) clr = (COLOR) k;
			else continue;

			if(clr == c)
				sr.enabled = false;
		}
	}
}
