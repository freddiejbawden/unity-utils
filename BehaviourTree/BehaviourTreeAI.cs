using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


public class BehaviourTreeAI : MonoBehaviour
{
    public BehaviourTree behaviourTree;
    private BehaviourNode behaviourTreeRoot;
    private Context context = Context.GetInstance();
    public Blackboard blackboard;
    public Blackboard groupBlackboard;
    private bool done;

   
    public T GetValueFromBlackboard<T>(string key)
    {
        // Make sure blackboard is set.
        if (blackboard == null) { return default(T); }
        return blackboard.GetValue<T>(key);
    }
 
    void Awake()
    {
        blackboard = new Blackboard();
    }
    void Start()
    {
        // Create a copy of the behaviour tree so we can indivudually set values
        behaviourTree = (BehaviourTree)behaviourTree.Copy();
        XNode.Node entry = ((BehaviourTree)behaviourTree.Copy()).FindEntryNode();
        behaviourTreeRoot = (BehaviourNode)entry.GetOutputPort("child").GetConnection(0).node;
        behaviourTreeRoot.Reset();
    }

    void Update()
    {
        if (!done)
        {
            NodeStatus s = behaviourTreeRoot.Behave(context, gameObject);
 
        }
       
    }
}
