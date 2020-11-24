using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static DataManager GetInstance(){
        return instance;
    }

    public void SaveData(List<Node> nodes, List<Edge> edges){
        StringBuilder builder = new StringBuilder();
        foreach(var node in nodes){
            builder.Append($"{node.label}|{node.value}|{node.cost}|{node.transform.position.x}|{node.transform.position.y}\n");
        }
        PlayerPrefs.SetString("nodes", builder.ToString());
        builder.Clear();
        foreach(var edge in edges){
            builder.Append($"{nodes.IndexOf(edge.head)}|{nodes.IndexOf(edge.tail)}\n");
        }
        PlayerPrefs.SetString("edges", builder.ToString());
    }

    public void LoadData(List<Node> nodes, List<Edge> edges, GameObject nodePref, GameObject edgePref){
        nodes.Clear();
        edges.Clear();
        string[] nodeStrs = PlayerPrefs.GetString("nodes").Split('\n');
        if(nodeStrs.Length == 0) return;
        foreach(var nodeStr in nodeStrs){
            Debug.Log(nodeStr);
            if(nodeStr.Equals(string.Empty))continue;
            var nodeObj = Instantiate(nodePref);
            Node node = nodeObj.GetComponent<Node>();
            string[] nodeArr = nodeStr.Split('|');
            node.label = nodeArr[0];
            node.value = int.Parse(nodeArr[1]);
            node.SetCost(int.Parse(nodeArr[2]));
            nodeObj.transform.position = new Vector3(float.Parse(nodeArr[3]), float.Parse(nodeArr[4]), 0);
            nodes.Add(node);
        }
        
        string[] edgeStrs = PlayerPrefs.GetString("edges").Split('\n');
        foreach(var edgeStr in edgeStrs){
            Debug.Log(edgeStr);
            string[] edgeArr = edgeStr.Split('|');
            if(!int.TryParse(edgeArr[0], out int headIndex))
                continue;
            if(!int.TryParse(edgeArr[1], out int tailIndex))
                continue;
            if(headIndex < 0 || tailIndex < 0){
                continue;
            }
            if(edgeStr.Equals(string.Empty))continue;
            var edgeObj = Instantiate(edgePref);
            Edge edge = edgeObj.GetComponent<Edge>();
            edge.Connect(nodes[headIndex], nodes[tailIndex]);
            edge.UpdateTransform(edge.tail.transform.position);
            edges.Add(edge);
        }
    }

    public void Export(){
        File.WriteAllText("NodeInfo.txt", PlayerPrefs.GetString("nodes"));
        File.WriteAllText("EdgeInfo.txt", PlayerPrefs.GetString("edges"));
    }

    public void Import(){
        PlayerPrefs.SetString("nodes", File.ReadAllText("NodeInfo.txt"));
        PlayerPrefs.SetString("edges", File.ReadAllText("EdgeInfo.txt"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
