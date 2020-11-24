using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Node head, tail;

    public Node GetOtherNode(Node node){
        if(head == node){
            return tail;
        }
        if(tail == node){
            return head;
        }
        throw new System.Exception("Invalid Edge.");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect(Node head, Node tail){
        this.head = head;
        this.tail = tail;
        head.edges.Add(this);
        if(tail != null)
            tail.edges.Add(this);
    }
    public void UpdateTransform(Vector3 targetPos){
        if(head == null) return;
        var diff = targetPos - head.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(-diff.normalized.x, diff.normalized.y) * Mathf.Rad2Deg);
        transform.localScale = new Vector3(0.02f, diff.magnitude * 0.8f, 1);
        transform.position = head.transform.position + diff / 2;
    }
}
