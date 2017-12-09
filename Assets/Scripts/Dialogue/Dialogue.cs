using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	private GameObject triggeringNpc;
	private bool triggering;

	public GameObject npcText;
	public Text changeText;
	public Text questText;
	public Text sideQuestText;

	public int countNextDialogue = 0;

	void Start(){

	}

	void Update(){
		if (triggering) {
			//print ("Player is triggering with " + triggeringNpc);
			npcText.SetActive (true);
			if(Input.GetKeyDown(KeyCode.T)){
				if (triggeringNpc.tag == "NPC1") {
					if (countNextDialogue < 4) { 
						changeText.text = "Hello hero! Follow me! A mother needs help!";
						questText.text = "Active Quest: Follow Guard";
						sideQuestText.text =  "Side Quest: Get Information";
					}
					if (countNextDialogue > 3) { 
						changeText.text = "Follow me hero! I will lead the way!";

					}
				}
				if (triggeringNpc.tag == "NPC2") {
					changeText.text = "Stop bothering me! Go away!";
				}
				if (triggeringNpc.tag == "NPC3") {
					changeText.text = "Poor lady. I can't believe she lost her child to the wolves.";
				}
				if (triggeringNpc.tag == "NPC4") {
					changeText.text = "I heard wolves love apples.";
					sideQuestText.text =  "Side Quest: Find Apples";
				}
				if (triggeringNpc.tag == "NPC5") {
					changeText.text = "I get apples every morning from the apple tree on the mountain.";
				}
				if (triggeringNpc.tag == "NPC6") {
					changeText.text = "Please help me! My child was taken away by a pack of wolves! Please bring him back to me!";
					questText.text = "Active Quest: Save Child";
				}
				if (triggeringNpc.tag == "NPC7") {
					changeText.text = "I am a wizard! I love magic!";
				}
			}
		} else {
			npcText.SetActive (false);
		}
			
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "NPC1" || other.tag == "NPC2"|| other.tag =="NPC3"|| other.tag =="NPC4"||other.tag == "NPC5"|| other.tag =="NPC6"|| other.tag =="NPC7") {
			triggering = true;
			triggeringNpc = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "NPC1" || other.tag =="NPC2"|| other.tag =="NPC3"||other.tag == "NPC4"|| other.tag =="NPC5"|| other.tag =="NPC6"|| other.tag =="NPC7") {
			triggering = false;
			triggeringNpc = null;
			changeText.text = "Press T to Talk";
			countNextDialogue++;
		}
	}

}
