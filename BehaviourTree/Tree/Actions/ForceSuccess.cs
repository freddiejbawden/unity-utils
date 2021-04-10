using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSuccess : DecoratorNode
{
    public override NodeStatus Behave(Context context, GameObject go)
    {
        NodeStatus ns = GetChild().Behave(context, go);
        if (ns != NodeStatus.RUNNING)
        {
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }

    public override void OnReset()
    {
        GetChild().Reset();
    }
}
