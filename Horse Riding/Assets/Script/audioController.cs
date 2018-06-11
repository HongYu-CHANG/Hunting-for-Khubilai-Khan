using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour {

	private AudioSource m_AudioSource;
	// Use this for initialization
	void Start () {
		m_AudioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//过按键A控制播放 
		if (Input.GetKeyDown(KeyCode.A))
		{
			m_AudioSource.Play();
		}
		//通过按键S控制暂停 
		if (Input.GetKeyDown(KeyCode.P))
		{
			m_AudioSource.Pause();
		}
		//通过按键D控制播放停止 
		if (Input.GetKeyDown(KeyCode.D))
		{
			m_AudioSource.Stop();
		}


		
	}
}
