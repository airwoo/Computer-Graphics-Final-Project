using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;
public class monsterTestTree : MonoBehaviour {

	public Transform wanders;
	public GameObject player;
	public GameObject[] monsters;
	public GameObject apple;
	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		Debug.Log (wanders.GetChild (0).position);

	}

	// Update is called once per frame
	void Update ()
	{

	}

	protected Node ST_ApproachAndWait(GameObject agent, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( agent.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node ST_Enemy(GameObject agent){
		Val<Vector3> p = Val.V (() => player.transform.position);
		Val<Vector3> a = Val.V (() => agent.transform.position);
		Val<Vector3> ap = Val.V (() => apple.transform.position);
		Val<Vector3> atop = Val.V (() => p.Value - a.Value);
		Val<Vector3> f = Val.V (() => agent.transform.TransformPoint(Vector3.forward));
		Val<bool> crouch = Val.V (() => player.GetComponent<Animator> ().GetBool ("isCrouch"));
		Func<bool> act = () => (atop.Value.magnitude < 7.0f && Vector3.Dot (atop.Value.normalized, f.Value.normalized) > 0.6
		                 || !crouch.Value && atop.Value.magnitude < 3.0f || (ap.Value - a.Value).magnitude < 7.0f);
		Func<bool> act2 = () => ((ap.Value - a.Value).magnitude <= 7.0f);
		Node trigger1 = new DecoratorLoop( new LeafAssert (act));
		Node trigger2 = new DecoratorLoop(new LeafAssert (act2));
		Node trigger3 = new DecoratorLoop( new LeafInvert (act));
		return new DecoratorForceStatus(RunStatus.Success, new Selector (new SequenceParallel (trigger3, new DecoratorLoop(new Sequence (ST_ApproachAndWait (agent, wanders.GetChild (0)),
			ST_ApproachAndWait (agent, wanders.GetChild (1)),
			ST_ApproachAndWait (agent, wanders.GetChild (2)),
			ST_ApproachAndWait (agent, wanders.GetChild (3))))),
			new SequenceParallel(trigger2, new Sequence(ST_ApproachAndWait(agent, apple.transform),
				agent.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("pickupright", 2000),
				agent.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("eat",3000))),
			new SequenceParallel(trigger1,new Sequence (agent.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("roar", 3000),
				agent.GetComponent<BehaviorMecanim> ().Node_RunTo (p)))
		));
	}

	protected Node BuildTreeRoot()
	{
		Val<Vector3> p = Val.V (() => player.transform.position);
		Node root =	new DecoratorLoop(ST_Enemy(monsters[0]));
		return root;
	}
}
