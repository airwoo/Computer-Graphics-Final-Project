using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree3 : MonoBehaviour {

	public Transform[] wanders;
	public GameObject[] npcs;
	public GameObject player;
	public GameObject guard;
	public InteractionObject grabber;
	//public GameObject test;
	//private BehaviorMecanim gd;
	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		//gd = guard.GetComponent<BehaviorMecanim> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	protected Node ST_ApproachInFront(GameObject agent, Transform target)
	{
		
		Val<Vector3> position = Val.V (() => (target.position + target.forward*2));
		return new Sequence(agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(2000));
	}

	protected Node ST_ApproachAndWait(GameObject agent, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}
	protected Node ST_PlayHand(GameObject agent, string s)
	{
		return agent.GetComponent<BehaviorMecanim>().ST_PlayHandGesture(s,3000);
	}
	protected Node ST_PlayFace(GameObject agent, string s)
	{
		return agent.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture(s,3000);
	}
	protected Node ST_FaceEachOther(GameObject ag1, GameObject ag2){
		Val<Vector3> p1 = Val.V (() => ag1.transform.position);
		Val<Vector3> p2 = Val.V (() => ag2.transform.position);
		return new Sequence (ag1.GetComponent<BehaviorMecanim>().Node_OrientTowards(p2),
			ag2.GetComponent<BehaviorMecanim>().Node_OrientTowards(p1));
	}
	protected Node ST_PlayConversation(GameObject ag1, GameObject ag2){
		Val<Vector3> p1 = Val.V (() => ag1.transform.position);
		Val<Vector3> p2 = Val.V (() => ag2.transform.position);
		return new Sequence (ag1.GetComponent<BehaviorMecanim> ().Node_OrientTowards(p2),
			ag2.GetComponent<BehaviorMecanim> ().Node_OrientTowards (p1),
			new SequenceParallel (
				new SequenceShuffle (
					ST_PlayFace (ag1,"HEADSHAKE"),
					ST_PlayFace(ag2,"HEADNOD"),
					ST_PlayFace(ag2,"DRINK"),
					ST_PlayFace(ag2,"EAT"),
					ST_PlayFace(ag1,"SAD")
				),
				new SequenceShuffle (
					ST_PlayHand(ag1,"THINK"),
					ST_PlayHand(ag1,"CLAP"),
					ST_PlayHand(ag2,"YAWN"),
					ST_PlayHand(ag1,"WRITING"),
					ST_PlayHand(ag2,"CHEER")
				)));
	}
	protected Node ST_PlayConversation2(GameObject ag1, GameObject ag2){
		Val<Vector3> p1 = Val.V (() => ag1.transform.position);
		Val<Vector3> p2 = Val.V (() => ag2.transform.position);
		return new SequenceParallel (
				new SequenceShuffle (
					ST_PlayFace (ag1,"HEADSHAKE"),
					ST_PlayFace(ag2,"HEADNOD"),
					ST_PlayFace(ag2,"DRINK"),
					ST_PlayFace(ag2,"EAT"),
					ST_PlayFace(ag1,"SAD")
				),
				new SequenceShuffle (
					ST_PlayHand(ag1,"THINK"),
					ST_PlayHand(ag1,"CLAP"),
					ST_PlayHand(ag2,"YAWN"),
					ST_PlayHand(ag1,"WRITING"),
					ST_PlayHand(ag2,"CHEER")
				));
	}
	protected Node NPCStory1(GameObject ag1, GameObject ag2){
		
		return new Sequence (new SequenceParallel (ST_ApproachAndWait (ag1, wanders [2]),
			ST_ApproachAndWait (ag2, wanders [1])),
			ST_PlayConversation (ag1, ag2),
			new SequenceParallel (ST_ApproachAndWait (ag1, wanders [3]),
				ST_ApproachAndWait (ag2, wanders [4])));
	}
	protected Node NPCStory2(GameObject ag1, GameObject ag2){
		Val<Vector3> p1 = Val.V (() => ag1.transform.position);
		Val<Vector3> p2 = Val.V (() => ag2.transform.position);
		return new Sequence (ag1.GetComponent<BehaviorMecanim> ().Node_SitDown (),
			ag2.GetComponent<BehaviorMecanim> ().Node_SitDown (),
			ST_PlayConversation2 (ag1, ag2));
	}
	protected Node NPCStory3(GameObject ag1){
		Val<Vector3> p1 = Val.V (() => ag1.transform.position);
	
		return new Sequence (ag1.GetComponent<BehaviorMecanim> ().Node_BodyAnimation("breakdance",true));
	}
	protected Node MainStory(){
		Val<Vector3> m = Val.V (() => npcs[5].transform.position + npcs[5].transform.forward*2);
		Val<Vector3> p = Val.V (() => player.transform.position);
		Val<Vector3> g = Val.V (() => guard.transform.position);
		Func<bool> next = () => (g.Value.x < -93); 
		Node trigger = new LeafAssert (next);
		return new Sequence (new DecoratorForceStatus(RunStatus.Success, new Sequence(trigger, guard.GetComponent<BehaviorMecanim>().Node_RunTo(m),
			new LeafWait(5000),
			ST_ApproachAndWait(guard,wanders[5]),
			guard.GetComponent<BehaviorMecanim>().Node_OrientTowards(p),
			guard.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, grabber),
			new LeafWait(5000),
			guard.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand))),
			new Sequence(guard.GetComponent<BehaviorMecanim>().Node_RunTo(wanders[6].position),
				guard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("pointing", 3000)));
	}
	protected Node BuildTreeRoot()
	{ 
		Val<Vector3> p = Val.V (() => player.transform.position);
		Val<Vector3> g = Val.V (() => guard.transform.position);
		Val<Vector3> m = Val.V (() => npcs[5].transform.position);
		Func<bool> act = () => ((p.Value - g.Value).magnitude < 10.0f); 
		Node trigger = new DecoratorLoop(new LeafAssert(act));
		return new SequenceParallel(new Sequence (ST_ApproachInFront (guard, player.transform),
			guard.GetComponent<BehaviorMecanim> ().Node_OrientTowards (p),
			new LeafWait(2000),
			guard.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("callover", 5000),
			guard.GetComponent<BehaviorMecanim> ().Node_RunTo (wanders [0].position),
			new DecoratorLoop(new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(trigger, MainStory())))),
			new DecoratorLoop(NPCStory1(npcs[1], npcs[2])),
			new Sequence(ST_FaceEachOther(npcs[0],npcs[3]), new DecoratorLoop(NPCStory2(npcs[0],npcs[3]))),
				new DecoratorLoop(NPCStory3(npcs[4])),
			new DecoratorLoop(new Sequence(ST_PlayFace(npcs[5],"sad"), ST_PlayHand(npcs[5],"cry")))
			);
	}
}
