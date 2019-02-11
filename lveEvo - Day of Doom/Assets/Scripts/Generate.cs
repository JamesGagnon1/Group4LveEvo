using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generate : MonoBehaviour {
	
	public GameObject WarriorPrefab;
	
	public GameObject EnemyPrefab;
	
	public GameObject[] DestOnGen;

	public Vector2[] EnemySpawnPoints;//this
	public int EnemyNumbs;
	
	public Animator GateOpen;
	
	public Image ControlsImage;
	public Sprite[] ControlsSprite;
	
	NeuralNetwork[] GenePool;
	
	float[] scores;
	
	bool ReadyToFight = false;
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		
		if (GameObject.FindObjectOfType<BossAI>() != null) {
			if (!ReadyToFight) {
				NeuralNetwork Keep = null;
				float MaxScore = -100000;
				foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
					if (NN.Score > MaxScore) {
						Keep = NN;
						MaxScore = NN.Score;
					}
				}
				foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
					if (NN != Keep) {
						Destroy(NN.gameObject);
					}
				}
				ReadyToFight = true;
			}
			
			
		} else {
		
			int CanRepro = 0;
			bool areAny = false;
			
			foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
				areAny = true;
				if (NN.Health >= 20) {
					CanRepro++;
				}
			}
			
			if (!areAny) {
				ControlsImage.sprite = ControlsSprite[0];
			} else {
				ControlsImage.sprite = ControlsSprite[1];
			}
			
			if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
				
				if (GameObject.FindObjectsOfType<Enemy>().Length < 10) {
				
					//Spawn Enemys

					for (int i = 0; i < EnemyNumbs; i++) {
						int SpawnLocation = UnityEngine.Random.Range (0, EnemySpawnPoints.Length);
						int OffsetX = UnityEngine.Random.Range (-30, 30);
						int OffsetY = UnityEngine.Random.Range (-30, 30);
						Instantiate (EnemyPrefab, new Vector3(EnemySpawnPoints[SpawnLocation].x + OffsetX, 0, EnemySpawnPoints[SpawnLocation].y + OffsetY), transform.rotation);
					}

					/*Array.Resize (ref DestOnGen, 75);
					for (int i = 0; i < 75; i++) {
						
						if (DestOnGen[i] != null) {
							Destroy (DestOnGen[i]);
						}
						int SpawnPoint = UnityEngine.Random.Range(0, 4);
						
						DestOnGen[i] = Instantiate (EnemyPrefab, new Vector3 (UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(530, 600)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
					} //this*/
					
					GateOpen.Play("OpenGate");
				}
			}
			
			bool CantClose = false;
			
			for (int i = 0; i < DestOnGen.Length; i++) {
				if (DestOnGen[i] != null) {
					if (DestOnGen[i].transform.position.magnitude > 480) {
						CantClose = true;
					}
				}
			}
			
			if (!CantClose && GateOpen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "GateOpened") {
				GateOpen.Play("CloseGate");
			}
			
			
			if (Input.GetKeyDown(KeyCode.Space)) {
					
				//Spawn Warriors
				Array.Resize (ref GenePool, 0);
				
				if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0 && CanRepro >= 1) {
					
					foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
						for (int i = 0; i < NN.Health+1; i++) {
							Array.Resize (ref GenePool, GenePool.Length+1);
							GenePool[GenePool.Length-1] = NN;
						}
					}
					
					for (int i = 0; i < 100; i++) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
						G.GetComponent<NeuralNetwork>().GenerateSimilar (GenePool[UnityEngine.Random.Range(0, GenePool.Length)], GenePool[UnityEngine.Random.Range(0, GenePool.Length)]);
					}
					
					for (int i = 0; i < GenePool.Length; i++) {
						if (GenePool[i] != null) {
							Destroy(GenePool[i].gameObject);
						}
					}
					
				} else if ((GameObject.FindObjectsOfType<NeuralNetwork>().Length == 0) || (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))) {
					
					if (GameObject.FindObjectsOfType<NeuralNetwork>().Length != 0) {
						foreach (NeuralNetwork NN in GameObject.FindObjectsOfType<NeuralNetwork>()) {
							Destroy(NN.gameObject);
						}
					}
					
					for (int i = 0; i < 100; i++) {
						GameObject G = Instantiate (WarriorPrefab, new Vector3 (UnityEngine.Random.Range(-250, 250), 0, UnityEngine.Random.Range(-250, 250)), Quaternion.Euler (0, UnityEngine.Random.Range(0, 360), 0));
						G.GetComponent<NeuralNetwork>().OnCreation ();
					}
				}
			}
		}
	}
}
