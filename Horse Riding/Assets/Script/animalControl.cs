﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalControl : MonoBehaviour {

	private GameObject horse;
	private Animator _animator;
	private bool isDead;
	private int faceDirection = 0;

	// Use this for initialization
	void Start ()
	{
		_animator = this.GetComponent<Animator>();
		isDead = false;
		this.gameObject.SetActive(false);
		/*while (faceDirection == 0)
		{
			faceDirection = UnityEngine.Random.Range(-1, 2);
		}*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		horse = GameObject.FindWithTag("horse");
		transform.forward += horse.transform.forward.normalized * 1;
		if (!isDead) this.transform.position += this.transform.forward.normalized * 5 * Time.deltaTime;
		
		if (Mathf.Abs(this.transform.eulerAngles.z) >= 70 && Mathf.Abs(this.transform.eulerAngles.z) <= 290)
		{
			Destroy(this.gameObject);
		}
		if (this.transform.position.y <= -20)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("arrow"))
		{
			_animator.SetTrigger("isShot");
			isDead = true;
			StartCoroutine(animalDisappear());
		}
	}


	IEnumerator animalDisappear()
	{
		yield return new WaitForSeconds(0.8f);
		Destroy(this.gameObject);
		//Score
	}
}
