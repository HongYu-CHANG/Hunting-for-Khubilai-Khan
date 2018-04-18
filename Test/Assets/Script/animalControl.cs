using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalControl : MonoBehaviour {

	private Animator _animator;
	private bool isDead;

	// Use this for initialization
	void Start ()
	{
		_animator = this.GetComponent<Animator>();
		isDead = false;


	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!isDead) this.transform.position += this.transform.forward.normalized * 5 * Time.deltaTime;
		
		if (Mathf.Abs(this.transform.eulerAngles.z) >= 70 && Mathf.Abs(this.transform.eulerAngles.z) <= 290)
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
		yield return new WaitForSeconds(2f);
		Destroy(this.gameObject);
		//Score
	}
}
