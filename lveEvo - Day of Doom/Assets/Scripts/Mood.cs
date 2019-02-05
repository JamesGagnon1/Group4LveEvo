using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mood : MonoBehaviour {
	
	public NeuralNetwork NN;
	
	public Transform size;
	
	public Image CanReproduce;
	public Image HisMood;
	public Text HisName;
	
	public Sprite[] MoodImages;
	
	bool WasActive;
	
	void Update () {
		float dis = Vector3.Distance(size.position, Camera.main.transform.position);
		if (dis > 75 || dis < 5 || Camera.main.GetComponent<God>().following) {
			size.gameObject.SetActive(false);
			WasActive = false;
		} else {
			size.gameObject.SetActive(true);
			if (!WasActive) {
				Activate();
				WasActive = true;
			}
			
			HisName.text = GetComponentInParent<Traits>().name;
			
			size.LookAt (Camera.main.transform.position, Camera.main.transform.up);
			size.localScale = new Vector3(-1, 1, 1)*Mathf.Clamp(dis/GetComponentInParent<Traits>().Size, 1, 100);
			if (NN.Health >= 20 && GameObject.FindObjectOfType<BossAI>() == null) {
				CanReproduce.enabled = true;
			} else {
				CanReproduce.enabled = false;
			}
		}
		
		if (NN.healthLeft <= 5) {
			HisMood.sprite = MoodImages[3];
		} else if (NN.TimeFood > 40) {
			HisMood.sprite = MoodImages[1];
		} else if (NN.TimeAttack > 40) {
			HisMood.sprite = MoodImages[2];
		} else {
			HisMood.sprite = MoodImages[0];
		}
	}
	
	void Activate () {
		GetComponent<Animator>().Play("Show");
	}
}
