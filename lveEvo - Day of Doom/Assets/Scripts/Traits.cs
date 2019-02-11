using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traits : MonoBehaviour {
	
	string[] charNames1 = {"Tug", "Gru", "Bor", "Du", "Gor", "Dom", "Bru", "cug", "Frub", "Tac", "Boc", "Gub", "Duc", "Ruc", "Brog", "Fu", "Dug", "Gur", "Dub", "Trud"};
	string[] charNames2 = {"tug", "dub", "gug", "du", "dig", "dom", "bom", "rag", "dor", "bug", "den", "tob", "rub", "gud", "cug", "turk", "dog", "tuc", "", ""};
	
	//Combined Total of 100
	public float Size;
	public float Speed;
	//Combined Total of 100
	public float Health;
	public float AttackPower;
	
	Animator Anim;
	
	Animator sideScreen;
	
	string[] names = {"Score", "Food", "Size", "Speed", "MaxHealth", "AttackPower", "Health"};
	Text[] stats;
	Text nameOfChar;
	
	Text[] Nodes1;
	Text[] Nodes2;
	Text[] Nodes3;
	Text Output;
	
	NeuralNetwork Neu;
	bool CanUpdateLines = true;
	
	void Start () {
		
		Neu = GetComponent<NeuralNetwork>();
		
		name = charNames1[Random.Range(0, charNames1.Length)]+charNames2[Random.Range(0, charNames2.Length)];
		
		nameOfChar = GameObject.Find("CharName").GetComponent<Text>();
		
		sideScreen = GameObject.Find("WarriorDisplay").GetComponent<Animator>();
		
		System.Array.Resize (ref stats, 7);
		
		for (int i = 0; i < stats.Length; i++) {
			stats[i] = GameObject.Find(names[i]).GetComponentInChildren<Text>();
		}
		
		/*Nodes1 = GameObject.Find("WeightWeights").GetComponentsInChildren<Text>();
		Nodes2 = GameObject.Find("Inputs").GetComponentsInChildren<Text>();
		Nodes3 = GameObject.Find("Animations").GetComponentsInChildren<Text>();
		Output = GameObject.Find("Output").GetComponentInChildren<Text>();*/
	}
	
	void Update () {
		
		Anim = GetComponent<Animator>();
		Anim.SetFloat("MoveSpeed", Speed);
		transform.localScale = Vector3.one*(Size/20);
		
		if (Camera.main.GetComponent<God>().following) {
			if (sideScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Hidden") {
				sideScreen.Play("ShowSide");
			}
			
			if (Camera.main.GetComponent<God>().objectToFollow == transform) {
				//Set all Text
				
				stats[0].text = Neu.Score.ToString();
				stats[1].text = Neu.Health.ToString();
				stats[2].text = Size.ToString();
				stats[3].text = Speed.ToString();
				stats[4].text = Health.ToString();
				stats[5].text = AttackPower.ToString();
				stats[6].text = Neu.healthLeft.ToString();
				
				/*Nodes1[0].text = ((int)(Neu.inputs[0].weight*100)).ToString();
				Nodes1[1].text = ((int)(Neu.inputs[3].weight*100)).ToString();
				Nodes1[2].text = "1";*/
				
				nameOfChar.text = name;
				
				GameObject.Find("MoodImage").GetComponent<Image>().sprite = GetComponentInChildren<Mood>().HisMood.sprite;
				
				if (CanUpdateLines) {
					foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Lines")) {
						Destroy (Go);
					}
				}
				
				/*for (int i = 0; i < Nodes2.Length; ++i) {
					Nodes2[i].text = (((int)(Neu.inputs[i].inputValue*(Neu.inputs[i].weight))).ToString());
					if (CanUpdateLines) {
						for (int x = 0; x < Nodes3.Length; ++x) {
							GameObject Temp = new GameObject("Line"+x, typeof(RectTransform));
							Temp.tag = "Lines";
							Temp.transform.parent = Nodes2[i].transform;
							Temp.transform.position = Nodes2[i].transform.position;
							Temp.transform.LookAt(Nodes3[x].transform.position);
							Vector3 Rot = Temp.transform.eulerAngles;
							Temp.transform.eulerAngles = new Vector3 (0, 0, -Rot.x);
							Temp.GetComponent<RectTransform>().pivot = new Vector2 (0, 0);
							Temp.GetComponent<RectTransform>().sizeDelta = new Vector2 (Vector3.Distance(Nodes2[i].transform.position, Nodes3[x].transform.position), 5);
							Temp.AddComponent<Image>();
							float theColor = (Neu.inputs[i].weights[x]/20)+0.5f;
							Temp.GetComponent<Image>().color = new Color(theColor, theColor, theColor, Mathf.Abs(Neu.inputs[i].weights[x]));
							Temp.transform.parent = GameObject.Find("BG").transform;
						}
					}
				}
				CanUpdateLines = false;
				
				for (int i = 0; i < Nodes3.Length; ++i) {
					Nodes3[i].text = ((int)Neu.allValues[i]).ToString();
				}
				
				Output.text = Neu.AnimClipInfo[0].clip.name;
			} else {
				CanUpdateLines = true;*/
			}
			
		} else {
			if (sideScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Shown") {
				sideScreen.Play("HideSide");
			}
		}
	}
	
	public void CreateNew () {
		//Randomize for 1st Generation
		
		Speed = Random.Range(0, 101);
		Size = (100-Speed)+Random.Range (-10, 11);
		Speed += Random.Range (-10, 11);
		
		Health = Random.Range(0, 101);
		AttackPower = (100-Health)+Random.Range (-10, 11);
		Health += Random.Range (-10, 11);
		
		Size = Mathf.Clamp (Size, 20, 50);
		Speed = Mathf.Clamp (Speed, 10, 50);
		Health = Mathf.Clamp (Health, 10, 50);
		AttackPower = Mathf.Clamp (AttackPower, 10, 50);
	}
	
	public void CreateSimilar (Traits T1, Traits T2) {
		//Randomize for allOther Generations

		int T1Or2 = Random.Range (0, 2);
		
		float R1 = (T1.Neu.Score/10);
		if (R1 == 0) {
			if (R1 >= 0) {
				R1 = 1;
			} else {
				R1 = -1;
			}
		}
		float R2 = (T2.Neu.Score/10);
		if (Mathf.Abs(R2) < 1) {
			if (R2 >= 0) {
				R2 = 1;
			} else {
				R2 = -1;
			}
		}
		
		R1 = 10/R1;
		R2 = 10/R2;
		
		if (T1Or2 == 0) {
			Speed = T1.Speed + Random.Range ((int)-R1, (int)R1+1);
		} else {
			Speed = T2.Speed + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			Size = T1.Size + Random.Range ((int)-R1, (int)R1+1);
		} else {                          
			Size = T2.Size + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			Health = T1.Health + Random.Range ((int)-R1, (int)R1+1);
		} else {                              
			Health = T2.Health + Random.Range ((int)-R2, (int)R2+1);
		}
		
		T1Or2 = Random.Range (0, 2);
		
		if (T1Or2 == 0) {
			AttackPower = T1.AttackPower + Random.Range ((int)-R1, (int)R1+1);
		} else {                                        
			AttackPower = T2.AttackPower + Random.Range ((int)-R2, (int)R2+1);
		}
		
		Size = Mathf.Clamp (Size, 10, 50);
		Speed = Mathf.Clamp (Speed, 10, 50);
		Health = Mathf.Clamp (Health, 10, 50);
		AttackPower = Mathf.Clamp (AttackPower, 10, 50);
	}
}
