using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindStage2 : MonoBehaviour
{
	private bool active = false;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Stage2"))
		{
			if (active == true)
			{
				return;
			} else
			{
				GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).gameObject.SetActive(true);
				GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
				if(Random.RandomRange(-2, 1) < 0)
				{
					GameObject.Find("Canvas").transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);
					GameObject.Find("Canvas").transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
				} else
				{
					GameObject.Find("Canvas").transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
					GameObject.Find("Canvas").transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(false);
				}
				active = true;
			}
		} else
		{
			GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).gameObject.SetActive(false);
			active = false;
		}
	}
}
