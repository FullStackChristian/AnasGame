using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> ConnectedTo = new List<Node>();

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "blendSampler");

        foreach (var node in ConnectedTo)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
