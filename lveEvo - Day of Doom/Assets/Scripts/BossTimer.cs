using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour {
	
	float T = 0;
	
	public float MaxTime = 600;
	
	public RectTransform Icon;
	public Text TimeLeft;
	
	bool CanStart = false;
	bool BossHasStarted = false;
	
	public GameObject BossPrefab;
	public GameObject ObjectToDisable;
	public GameObject ObjectToDisableAtEnd;
	
	void Update () {
		
		MaxTime = Mathf.Clamp(MaxTime, 30, 1800);
		
		if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
			CanStart = true;
		}
		
		if (CanStart) {
			ObjectToDisable.SetActive(false);
			T += Time.deltaTime;
			T = Mathf.Clamp(T, 0, MaxTime);
		}
		
		int Mins = (int)(((int)MaxTime-T)/60);
		int Secs = (int)((MaxTime-T)%60);
		string Ms = "";
		string Ss = "";
		
		if (Mins < 10) {
			Ms = "0"+Mins;
		} else {
			Ms = ""+Mins;
		}
		if (Secs < 10) {
			Ss = "0"+Secs;
		} else {
			Ss = ""+Secs;
		}
		
		if (!BossHasStarted) {
			TimeLeft.text = "Time Left\n<Size=50>"+Ms + ":" + Ss+"</Size>";
			
			if (MaxTime-T <= 60) {
				TimeLeft.color = Color.red;
			} else {
				TimeLeft.color = Color.white;
			}
			
			Icon.localPosition = new Vector3 (Icon.localPosition.x, (((MaxTime-T)/MaxTime)-0.5f)*900, Icon.localPosition.z);
		} else {
			ObjectToDisableAtEnd.SetActive(false);
		}
		
		if (MaxTime-T <= 0 && !BossHasStarted) {
			BossHasStarted = true;
			Destroy (GameObject.FindObjectOfType<AudioSource>().gameObject);
			Instantiate (BossPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
		}
	}
	
	void IncTime () {
		MaxTime += 30;
	}
	
	void DecTime () {
		MaxTime -= 30;
	}
}
