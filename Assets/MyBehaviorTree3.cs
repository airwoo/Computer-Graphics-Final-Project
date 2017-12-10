using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree3 : MonoBehaviour {

	public Transform[] wanders;
	public GameObject[] npcs;
	public GameObject[] enemies;
	public GameObject player;
	public GameObject guard;
	public GameObject apple;
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
	protected Node ST_Converse(GameObject agent){
		return new SequenceParallel (
			new SequenceShuffle (
				ST_PlayFace (agent,"HEADSHAKE"),
				ST_PlayFace(agent,"HEADNOD"),
				ST_PlayFace(agent,"DRINK"),
				ST_PlayFace(agent,"EAT"),
				ST_PlayFace(agent,"SAD")
			),
			new SequenceShuffle (
				ST_PlayHand(agent,"THINK"),
				ST_PlayHand(agent,"CLAP"),
				ST_PlayHand(agent,"YAWN"),
				ST_PlayHand(agent,"WRITING"),
				ST_PlayHand(agent,"CHEER")
			));

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
	protected Node NPCStory4(GameObject agent){
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> g = Val.V (() => player.transform.position);
		Func<bool> next = () => ((g.Value - a.Value).magnitude < 5.0f); 
		Node trigger = new DecoratorLoop(new LeafInvert (next));
		return	new Selector(new SequenceParallel(trigger,agent.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("drink",3000)),
				new Sequence(agent.GetComponent<BehaviorMecanim>().Node_OrientTowards(g),
					agent.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("fight",3000)));
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
	protected Node NPCGate(GameObject agent){
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> g = Val.V (() => player.transform.position);
		Func<bool> next = () => ((g.Value - a.Value).magnitude < 5.0f); 
		Node trigger = new DecoratorLoop(new LeafAssert (next));
		return new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, 
			new Sequence(agent.GetComponent<BehaviorMecanim>().Node_OrientTowards(g),
				agent.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("surrender",3000))));
	}
	protected Node NPCRoam(GameObject agent){
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> g = Val.V (() => player.transform.position);
		Func<bool> next = () => ((g.Value - a.Value).magnitude < 5.0f); 
		Node trigger = new DecoratorLoop(new LeafInvert (next));
		return
			new Selector(new SequenceParallel(trigger,new DecoratorLoop(new SequenceShuffle(ST_ApproachAndWait(agent, wanders[7]),
				ST_ApproachAndWait(agent, wanders[8]),
				ST_ApproachAndWait(agent, wanders[9]),
				ST_ApproachAndWait(agent, wanders[10])))),
			new Sequence(agent.GetComponent<BehaviorMecanim>().Node_OrientTowards(g),
					agent.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("surrender",3000)));
	}
	protected Node ST_Enemy(GameObject agent){
		Val<Vector3> p = Val.V (() => player.transform.position);
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> ap = Val.V (() => apple.transform.position);
		Val<Vector3> atop = Val.V (() => p.Value - a.Value);
		Val<Vector3> f = Val.V (() => agent.transform.TransformPoint(Vector3.forward));
		Val<bool> crouch = Val.V (() => player.GetComponent<Animator> ().GetBool ("isCrouch"));
		Func<bool> act = () => (atop.Value.magnitude < 12.0f && Vector3.Dot (atop.Value.normalized, f.Value.normalized) > 0.6
			|| !crouch.Value && atop.Value.magnitude < 5.0f || (ap.Value - a.Value).magnitude < 10.0f);
		Func<bool> act2 = () => ((ap.Value - a.Value).magnitude <= 7.0f);
		Node trigger1 = new DecoratorLoop( new LeafAssert (act));
		Node trigger2 = new DecoratorLoop(new LeafAssert (act2));
		Node trigger3 = new DecoratorLoop( new LeafInvert (act));
		return new DecoratorForceStatus(RunStatus.Success, new Selector (new SequenceParallel (trigger3, new DecoratorLoop(new SequenceShuffle (ST_ApproachAndWait (agent, wanders[11]),
			ST_ApproachAndWait (agent, wanders[12]),
			ST_ApproachAndWait (agent, wanders[13]),
			ST_ApproachAndWait (agent, wanders[14])))),
			new SequenceParallel(trigger2, new Sequence(ST_ApproachAndWait(agent, apple.transform),
				agent.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("pickupright", 2000),
				agent.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("eat",3000))),
			new SequenceParallel(trigger1,new Sequence (agent.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("roar", 3000),
				agent.GetComponent<BehaviorMecanim> ().Node_RunTo (p)))
		));
	}
	protected Node NPC_Husband(GameObject agent){
		Val<Vector3> p = Val.V (() => player.transform.position);
		Val<Vector3> p2 = Val.V (() => player.transform.position+player.transform.TransformDirection(Vector3.right)*2.0f);
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> m = Val.V (() => npcs [5].transform.position);
		Func<bool> act = () => ((a.Value-p.Value).magnitude < 10.0f);
		Func<bool> act2 = () => ((a.Value-m.Value).magnitude < 5.0f);
		Node trigger1 = new DecoratorLoop( new LeafAssert (act));
		Node trigger2 = new DecoratorLoop( new LeafInvert (act));
		Node trigger3 = new DecoratorLoop (new LeafAssert (act2));
		Node trigger4 = new DecoratorLoop (new LeafInvert (act2));
		return new DecoratorForceStatus (RunStatus.Success, new Selector (new SequenceParallel(trigger1, trigger4,
			new DecoratorLoop(agent.GetComponent<BehaviorMecanim>().Node_RunToUpToRadius(p2, 1.0f))),
			new SequenceParallel(trigger2, trigger4, new DecoratorLoop(agent.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("duck",2000))),
			new SequenceParallel(trigger3, new DecoratorLoop(agent.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cheer", 5000)))));

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
			new DecoratorLoop(new Sequence(ST_PlayFace(npcs[5],"sad"), ST_PlayHand(npcs[5],"cry"))),
			new DecoratorLoop(NPCGate(npcs[6])),
			new DecoratorLoop(NPCGate(npcs[7])),
			new DecoratorLoop(NPCGate(npcs[12])),
			new DecoratorLoop(NPCGate(npcs[13])),
			new DecoratorLoop(NPCRoam(npcs[8])),
			new DecoratorLoop(NPCRoam(npcs[9])),
			new DecoratorLoop(NPCRoam(npcs[10])),
			new DecoratorLoop(NPCRoam(npcs[11])),
			new Sequence(npcs[14].GetComponent<BehaviorMecanim>().Node_OrientTowards(npcs[3].transform.position), new DecoratorLoop(ST_Converse(npcs[14]))),
			new DecoratorLoop(NPCStory4(npcs[15])),
			new DecoratorLoop(NPCStory4(npcs[16])),
			new DecoratorLoop(ST_Enemy(enemies[0])),
			new DecoratorLoop(ST_Enemy(enemies[1])),
			new DecoratorLoop(ST_Enemy(enemies[2])),
			new DecoratorLoop(NPC_Husband(npcs[17]))
			);
	}
}
