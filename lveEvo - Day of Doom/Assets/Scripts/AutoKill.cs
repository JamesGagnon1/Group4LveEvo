using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour {
	
	float t = 0;
	
	public float KillTime;
	
	void Update () {
		t += Time.deltaTime;
		
		if (t > KillTime) {
			Destroy(this.gameObject);
		}
	}
}
