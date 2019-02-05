using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour {
	
	float AngX;
	float AngY;
	
	public float speed;
	public float angSpeed;
	
	public GameObject FoodObject;
	public GameObject Lightning;
	public LayerMask LM;
	
	public bool following;
	public Transform objectToFollow;
	float height;
	float ThrowPower = 0;
	int FoodLeft = 100;
	public Text FoodText;
	
	void Update () {

		//Cam Control

		transform.Translate (Input.GetAxis("Horizontal") * 3, 0, Input.GetAxis("Vertical") * 3);

		if (!following) {
			if (Input.GetKey(KeyCode.Mouse2)) {
				transform.Translate (-Input.GetAxis("Mouse X")*speed*Time.deltaTime, -Input.GetAxis("Mouse Y")*speed*Time.deltaTime,  0);
			} else if (Input.GetKey(KeyCode.Mouse1)) {
				AngX += -Input.GetAxis("Mouse Y")*angSpeed*Time.deltaTime;
				AngY += Input.GetAxis("Mouse X")*angSpeed*Time.deltaTime;
			}
			transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel")*speed*20*Time.deltaTime);
		} else {
			
			//followWarrior
			
			if (objectToFollow != null) {
				height += -Input.GetAxis("Mouse ScrollWheel")*speed*10*Time.deltaTime;
				height = Mathf.Clamp(height, 15, 50);
				transform.position = Vector3.Lerp(transform.position, objectToFollow.position + (Vector3.up*(objectToFollow.GetComponent<Traits>().Size/20)*height), 10*Time.deltaTime);
				AngX = Mathf.Lerp(AngX, 90, 10*Time.deltaTime);
				
				if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
					if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0) {
						following = false;
					}
				}
			} else {
				following = false;
			}
		}
		
		transform.eulerAngles = new Vector3 (AngX, AngY, 0);
		
		float MaxX = 489.5f*Mathf.Cos(Mathf.Asin(transform.position.z/489.5f));
		float MaxZ = 489.5f*Mathf.Cos(Mathf.Asin(transform.position.x/489.5f));
		
		transform.position = new Vector3 (Mathf.Clamp(transform.position.x, -MaxX, MaxX), Mathf.Clamp(transform.position.y, 15, 70), Mathf.Clamp(transform.position.z, -MaxZ, MaxZ));
		
		//Food
		
		if (Input.GetKey(KeyCode.F)) {
			ThrowPower += Time.deltaTime*50;
		}
		if (Input.GetKeyUp(KeyCode.F)) {
			ThrowPower = Mathf.Clamp (ThrowPower, 1, 750);
			if (FoodLeft > 0) {
				if (GameObject.FindObjectOfType<BossAI>() != null) {
					--FoodLeft;
				}
				GameObject G = Instantiate (FoodObject, transform.position + ((transform.right*((Input.mousePosition.x-(Screen.width/2))/Screen.width))*3.75f) + ((transform.up*((Input.mousePosition.y-(Screen.height/2))/Screen.height))*2.75f) + (transform.forward*2), transform.rotation);
				G.transform.LookAt (transform.position);
				G.transform.Rotate (0, 180, 0);
				G.GetComponent<Rigidbody>().AddRelativeForce (0, 0, 500*ThrowPower);
			}
			ThrowPower = 0;
		}
		
		if (GameObject.FindObjectOfType<BossAI>() != null) {
			FoodText.color = Color.white;
			FoodText.text = "\n\nFood Left\n<Size=50>"+FoodLeft+"</Size>";
		} else {
		
			if (Input.GetKeyDown(KeyCode.L)) {
				
				RaycastHit theObject;
				
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObject, Mathf.Infinity, LM)) {
					Instantiate (Lightning, new Vector3 (theObject.point.x, 0, theObject.point.z), Quaternion.identity);
				}
			}
		}
	}

	public void feed () {
		Instantiate (FoodObject, objectToFollow.position, transform.rotation);
	}
}
