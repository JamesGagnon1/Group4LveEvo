using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {
	
	public NeuralNetwork NN;
	public float Range;
	public Animator Anim;
	bool WasInRange;
	bool BackToStart = false;
	bool doNew;
	void Update () {
		
		AnimatorClipInfo[] AnimClipInfo = Anim.GetCurrentAnimatorClipInfo(0);
		
		if (GameObject.FindObjectsOfType<NeuralNetwork>().Length == 1) {
		
			NN = GameObject.FindObjectOfType<NeuralNetwork>();
			
			if (Vector3.Distance (transform.position, NN.transform.position) < Range) {
				if (AnimClipInfo[0].clip.name == "BossIdle" || AnimClipInfo[0].clip.name == "BossRun") {
					int R = Random.Range(1, 4);
					Anim.Play("BossAttack"+R);
				}
				
			} else {
				
				transform.LookAt (new Vector3 (NN.transform.position.x, transform.position.y, NN.transform.position.z));
				
				if (AnimClipInfo[0].clip.name == "BossIdle") {
					if (transform.position.y < 138) {
						Anim.Play("BossRun");
					}
				}
			}
		}
	}
}
