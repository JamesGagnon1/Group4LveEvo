using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public Animator Anim;
	public GameObject Death;
	
	public float Health = 50;
	bool canGetHit = true;
	
	float range = 10;
	
	public static NeuralNetwork[] NN;
	
	float T = 0;
	int C = 0;
	
	bool isClose = false;
	
	void Update () {
		
		if (name == "StayForever") {
			NN = GameObject.FindObjectsOfType<NeuralNetwork>();
		} else if (NN != null) {
			
			float dist = 100000;
			isClose = false;
			
			for (int i = 0; i < NN.Length; i++) {
				
				float thisDist = Vector3.Distance(transform.position, NN[i].transform.position);
				
				if (thisDist <= dist) {
					dist = thisDist;
					C = i;
				}
				
				if (thisDist <= range) {
					
					Anim.SetBool("Move", false);
					isClose = true;
				
					T += Time.deltaTime;
				
					if (T > 2) {
						
						Anim.SetBool("Swing", true);
						
						if (T > 5) {
							if (NN[i].Blocking) {
								NN[i].Score += 5;
							} else {
								NN[i].Score -= 10;
								--NN[i].healthLeft;
							}
							T = 0;
							Anim.SetBool("Swing", false);
						}
						
						NN[i].BeingAttacked(T);
					}
					
					if (NN[i].Attacking) {
						if (canGetHit) {
							NN[i].TimeAttack = 0;
							NN[i].Score += 10;
							Health -= NN[i].GetComponent<Traits>().AttackPower;
							canGetHit = false;
							if (Health <= 0) {
								NN[i].Score += 20;
								Destroy(this.gameObject);
							}
						}
					} else {
						canGetHit = true;
					}
				}
			}
			
			if (!isClose) {
				Anim.SetBool("Move", true);
				Anim.SetBool("Swing", false);
			}
			
			transform.LookAt(NN[C].transform.position);
			
			if (GameObject.FindObjectOfType<BossAI>() != null) {
				Destroy(this.gameObject);
			}
			
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Lightning") {
			Destroy (this.gameObject);
		}
	}
	
	void OnDestroy() {
		Instantiate(Death, transform.position, Quaternion.identity);
	}
}
