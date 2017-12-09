using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;
public class MyBehaviorTree : MonoBehaviour
{
	public Transform[] wanders;
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
	public GameObject daniel,dave,richard,victim,guard;
	public GameObject[] npc;
	public InteractionObject prop;
	private BehaviorMecanim dan,dav,ric;
	private Vector3 dn,dv;
	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		dan = daniel.GetComponent<BehaviorMecanim> ();
		dav = dave.GetComponent<BehaviorMecanim> ();
		ric = richard.GetComponent<BehaviorMecanim> ();
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		richard.GetComponent<UnitySteeringController> ().maxSpeed2 = 9.0f;
	}

	// Update is called once per frame
	void Update ()
	{

	}
	protected Node ST_ApproachAndWait(GameObject p, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return p.GetComponent<BehaviorMecanim>().Node_GoTo(position);
	}
	protected Node ST_ApproachAndWait2(GameObject p, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( p.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}
	protected Node ST_ApproachArea(GameObject p, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return p.GetComponent<BehaviorMecanim> ().Node_GoToUpToRadius (position, 2.0f);

	}

	protected Node IntroWalk(){
		return new SequenceParallel (
			this.ST_ApproachAndWait (daniel, wanders[0]),
			this.ST_ApproachAndWait (dave, wanders[1]),
			this.ST_ApproachAndWait(richard,this.wander2));
	}
	protected Node faceEachOther(GameObject p1, GameObject p2){
		Val<Vector3> pos1 = Val.V(() => p1.transform.position);
		Val<Vector3> pos2 = Val.V (() => p2.transform.position);
		return
			new SequenceParallel (
				p1.GetComponent<BehaviorMecanim>().ST_TurnToFace(pos2),
				p2.GetComponent<BehaviorMecanim>().ST_TurnToFace(pos1));


	}
	protected Node shakeHands(GameObject p1, GameObject p2){
		InteractionObject p1hand = p1.transform.Find ("Interactions").transform.Find("INTER_Give").GetComponent<InteractionObject>();
		InteractionObject p2hand = p2.transform.Find ("Interactions").transform.Find("INTER_Give").GetComponent<InteractionObject>();
		return new Sequence (
			new SequenceParallel(p1.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand,p2hand),
				p2.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand,p1hand)),
				new LeafWait(2000),
				p1.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand),
				p2.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand)
		);
	}
	protected Node ranHeads(Val<string> gest){
		return new Sequence(new SelectorShuffle (dan.ST_PlayFaceGesture (gest, 2000),
			dav.ST_PlayFaceGesture (gest, 2000),
			ric.ST_PlayFaceGesture (gest, 2000)));
	}
	protected Node ranHands(Val<string> gest){
		return new Sequence(new SelectorShuffle (dan.ST_PlayHandGesture (gest, 2000),
			dav.ST_PlayHandGesture (gest, 2000),
			ric.ST_PlayHandGesture (gest, 2000)));
	}
	protected Node ranHeads2(Val<string> gest){
		return new Sequence(new SelectorShuffle (npc[0].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[1].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[2].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[3].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000)));
	}
	protected Node ranHands2(Val<string> gest){
		return new Sequence(new SelectorShuffle (npc[0].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[1].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[2].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000),
			npc[3].GetComponent<BehaviorMecanim>().ST_PlayHandGesture (gest, 2000)));
	}
	protected Node Conversation(){
		//	int t = 0;
		//Func<bool> act = () => (t < 10);
		return// new DecoratorLoop (
			//new LeafAssert ((t) => t < 10),
			//new LeafInvoke (t += 1),
			new SequenceParallel (
				new SequenceShuffle (
					ranHeads ("HEADSHAKE"),
					ranHeads ("HEADNOD"),
					ranHeads ("DRINK"),
					ranHeads ("EAT"),
					ranHeads ("SAD")
				),
				new SequenceShuffle (
					ranHands ("THINK"),
					ranHands ("CLAP"),
					ranHands ("YAWN"),
					ranHands ("WRITING"),
					ranHands ("CHEER")
				));
		//);
	}
	protected Node Conversation2(){
		//	int t = 0;
		//Func<bool> act = () => (t < 10);
		return// new DecoratorLoop (
			//new LeafAssert ((t) => t < 10),
			//new LeafInvoke (t += 1),
			new SequenceParallel (
				new SequenceShuffle (
					ranHeads2 ("HEADSHAKE"),
					ranHeads2 ("HEADNOD"),
					ranHeads2 ("DRINK"),
					ranHeads2 ("EAT"),
					ranHeads2 ("SAD")
				),
				new SequenceShuffle (
					ranHands2 ("THINK"),
					ranHands2 ("CLAP"),
					ranHands2 ("YAWN"),
					ranHands2 ("WRITING"),
					ranHands2 ("CHEER")
				));
		//);
	}
	protected Node Introduction(){
		return new Sequence(new SequenceParallel (
			this.ST_ApproachArea (daniel, this.wander3),
			this.ST_ApproachArea (dave, this.wander3),
			this.ST_ApproachArea (richard, this.wander3)),
			Conversation()
		);
	}
	//protected Node Killer(){

	//}
	protected Node PickUp(GameObject p, InteractionObject o){
		return p.GetComponent<BehaviorMecanim> ().Node_StartInteraction (FullBodyBipedEffector.RightHand, o);
	}

	protected Node Story1(){
		Val<Vector3> rp = Val.V (() => richard.transform.position);
		Val<Vector3> dp = Val.V (() => daniel.transform.position);
		Val<Vector3> gd = Val.V (() => guard.transform.position);
		Func<bool> caught = () => ((rp.Value - dp.Value).magnitude < 1.2f);
		Func<bool> survive = () => ((rp.Value - gd.Value).magnitude < 1.2f);
		Node trigger = new DecoratorLoop (new LeafInvert(caught));
		Node trigger2 = new DecoratorLoop (new LeafInvert(survive));
		Debug.Log((rp.Value - dp.Value).magnitude.ToString());
		return new Sequence (
			ric.Node_GoToUpToRadius(victim.transform.position, 3.0f),
			ric.ST_PlayBodyGesture("DUCK", 3000),
			new DecoratorForceStatus(RunStatus.Success,new SequenceParallel(
				trigger,
				trigger2,
				new DecoratorLoop(new SelectorShuffle(ric.Node_RunTo(Val.V(()=>wanders[3].position)),ric.Node_RunTo(gd))),
				new Sequence(new LeafWait(5500),new DecoratorLoop(dan.Node_RunToUpToRadius(Val.V(()=>richard.transform.position),1.5f))),
				new Sequence(new LeafWait(5500),new DecoratorLoop(dav.Node_RunToUpToRadius(Val.V(()=>richard.transform.position),1.5f))))),
			new Selector(new Sequence(new LeafAssert(survive),End2()),
				End1())
				
		);
	}
	protected Node End1(){
		return new Sequence (
			dan.ST_PlayHandGesture ("hitstealth", 1000),
			ric.ST_PlayBodyGesture ("DYING", 250),
			new LeafWait(3000));
	}
	protected Node End2(){
		Val<Vector3> dp = Val.V (() => daniel.transform.position);
		return new Sequence (
			ric.ST_PlayHandGesture("shock",1000),
			guard.GetComponent<BehaviorMecanim>().Node_OrientTowards(dp),
			guard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("WARNINGSHOT",1000),
			dan.ST_PlayHandGesture("Stayaway", 1000),
			new LeafWait(3000)

		);
	}
	protected Node NPCRun1(){
		Val<Vector3> gd = Val.V (() => guard.transform.position);
		return new SequenceParallel (
			npc[0].GetComponent<BehaviorMecanim>().Node_RunToUpToRadius(gd.Value,1.9f),
			npc[1].GetComponent<BehaviorMecanim>().Node_RunToUpToRadius(gd.Value,1.9f),
			npc[2].GetComponent<BehaviorMecanim>().Node_RunToUpToRadius(gd.Value,1.9f),
			npc[3].GetComponent<BehaviorMecanim>().Node_RunToUpToRadius(gd.Value,1.9f)
		);
	}
	protected Node NPCBehavior1(){
		Val<Vector3> rp = Val.V (() => richard.transform.position);
		Func<bool> caught = () => ((rp.Value - wanders[3].position).magnitude < 2.0f);
		Node trigger = new DecoratorLoop (new LeafInvert(caught));
		return new Sequence(
			new Sequence (npc[0].GetComponent<BehaviorMecanim>().Node_OrientTowards(wanders[3].position),
				npc[1].GetComponent<BehaviorMecanim>().Node_OrientTowards(wanders[3].position),
				npc[2].GetComponent<BehaviorMecanim>().Node_OrientTowards(wanders[3].position),
				npc[3].GetComponent<BehaviorMecanim>().Node_OrientTowards(wanders[3].position)),
			new LeafWait(1000),
			new SequenceParallel (npc[0].GetComponent<BehaviorMecanim>().Node_SitDown(),
			npc[1].GetComponent<BehaviorMecanim>().Node_SitDown(),
			npc[2].GetComponent<BehaviorMecanim>().Node_SitDown(),
			npc[3].GetComponent<BehaviorMecanim>().Node_SitDown()
		),
			new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(trigger, new DecoratorLoop(Conversation2()))),
			new SequenceParallel (npc[0].GetComponent<BehaviorMecanim>().Node_StandUp(),
				npc[1].GetComponent<BehaviorMecanim>().Node_StandUp(),
				npc[2].GetComponent<BehaviorMecanim>().Node_StandUp(),
				npc[3].GetComponent<BehaviorMecanim>().Node_StandUp()
			),
			NPCRun1()
		);
	}
	protected Node NPCBehavior2(GameObject agent, int x){
		Val<Vector3> rp = Val.V (() => richard.transform.position);
		Val<Vector3> ag = Val.V (() => agent.transform.position);
		Func<bool> caught = () => ((rp.Value - ag.Value).magnitude < 1.3f);
		Debug.Log (agent.transform.position);
		Node roaming = new DecoratorLoop (
			new Sequence (
				this.ST_ApproachArea(agent, wanders[x]),
				agent.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture("texting", 3000),
				this.ST_ApproachArea(agent, wanders[x+1]),
				agent.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture("yawn", 3000)));
		Node trigger = new DecoratorLoop (new LeafInvert(caught));
		return new Sequence(
			new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, roaming)),
			new DecoratorLoop(agent.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture("DUCK", 3000))
		);
	}
	protected Node NPCBehavior3(GameObject agent){
		InteractionObject p1hand = guard.transform.Find ("Interactions").transform.Find("INTER_Give").GetComponent<InteractionObject>();
		Val<Vector3> rp = Val.V (() => richard.transform.position);
		Val<Vector3> ag = Val.V (() => agent.transform.position);
		Func<bool> caught = () => (rp.Value.z < 1.0f);
		Debug.Log (agent.transform.position);
		Node roaming = new DecoratorLoop (
			new Sequence (
				this.ST_ApproachArea(agent, wanders[8]),
				agent.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture("writing", 3000),
				this.ST_ApproachArea(agent, wanders[9]),
				agent.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture("think", 3000)));
		Node trigger = new DecoratorLoop (new LeafInvert(caught));
		return new Sequence (
			new DecoratorForceStatus (RunStatus.Success, new SequenceParallel (trigger, roaming)),
			this.ST_ApproachArea (agent, wanders [8]),
			new DecoratorLoop (new Sequence (agent.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("ChestPumpsalute", 6000))
			));
	}
	protected Node BuildTreeRoot()
	{

		Val<string> gest = new Val<string> ("WAVE");
		Val<long> dur = new Val<long> (2000);
		//Val<FullBodyBipedEffector> eff;
		//Val<InteractionObject> obj = new Val<InteractionObject> (ls.GetComponent<InteractionObject>());
		Node roaming = new SequenceParallel(new Sequence(
					new LeafWait(500),
				new SequenceParallel(victim.GetComponent<BehaviorMecanim>().Node_OrientTowards(daniel.transform.position),
					dan.Node_OrientTowards(victim.transform.position)),
				dan.Node_HeadLook(victim.transform.position),
				PickUp(daniel,prop),
				new LeafWait(2000),
				dan.ST_PlayHandGesture("pistolaim",5000),
					victim.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING", 250),
				new SequenceParallel(dan.Node_SitandStand(2000),dan.ST_PlayHandGesture("DRINK",1000)),
				dan.Node_HeadLookStop(),
				dan.Node_StopInteraction(FullBodyBipedEffector.RightHand),
					IntroWalk(),
				new LeafWait(200),
					faceEachOther(daniel, dave),
				shakeHands(daniel,dave),
					new LeafWait(1000),
					dan.Node_HeadLookStop(),
					dav.Node_HeadLookStop(),
				new SequenceParallel(dan.ST_PlayHandGesture("WAVE",2500),
					dav.ST_PlayHandGesture("WAVE",2500)),
					new LeafWait(1000),
				Introduction(),
				Story1()
		),NPCBehavior1(),
			NPCBehavior2(npc[4],4),
			NPCBehavior2(npc[5],5),
			NPCBehavior2(npc[6],6),
			NPCBehavior3(npc[7]),
			NPCBehavior3(npc[8])
		);
		return roaming;
	}
}
