using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Node nodePref;
    public Edge edgePref;

    public Node initialNode;

    public GameObject mouseMenu;
    public GameObject editMenu;
    public GameObject nodeInfo;
    public Text summary;
    public Node curNode;
    private List<Node> nodes;
    private List<Edge> edges;
    public Text info;
    public Node newNode;
    public Edge newEdge;
    public Node hoveredNode;
    
    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();

        var node = Instantiate(nodePref.gameObject);
        initialNode = node.GetComponent<Node>();
        nodes.Add(initialNode);
    }

    // Update is called once per frame
    void Update()
    {
        var xMove = Input.GetAxisRaw("Horizontal");
        var yMove = Input.GetAxisRaw("Vertical");
        Camera.main.transform.position += new Vector3(xMove, yMove, 0) * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + Input.mouseScrollDelta.y * Time.deltaTime * 10, 0.5f, 5);

        if(newEdge == null) return;
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = new Vector3(position.x, position.y, 0);
        
        if(Input.GetMouseButtonDown(0)){
            if(newNode == null){
                if(hoveredNode == null)
                    return;
                newEdge.Connect(curNode, hoveredNode);
                newEdge.UpdateTransform(hoveredNode.transform.position);
                hoveredNode = null;
                newEdge = null;
            }
            curNode = newNode;
            newEdge = null;
            newNode = null;
            editMenu.SetActive(true);
        }else{
            newEdge.UpdateTransform(position);
            if(newNode == null) return;
            newNode.transform.position = position;
        }
    }

    public void ActivateMouseMenu(){
        if(newEdge != null)
            return;
        mouseMenu.SetActive(true);
    }

    public void ActivateInfoPanel(Node node){
        if(newEdge != null || mouseMenu.activeSelf || editMenu.activeSelf)
            return;
        nodeInfo.SetActive(node);
        nodeInfo.GetComponent<InfoPanel>().SetData(node);
    }

    public void DeactivateInfoPanel(){
        nodeInfo.SetActive(false);
    }

    public void DeleteCurrentNode(){
        if(curNode == null || curNode == initialNode){
            return;
        }
        nodes.Remove(curNode);
        curNode.Delete();
        curNode = null;
    }

    public void CalculateInfo(){
        Dictionary<string, int> dict = new Dictionary<string, int>();
        int totalCost = 0;
        foreach(var node in nodes){
            if(!node.IsToggled())
                continue;
            totalCost += node.cost;
            string[] names = node.label.Split(',');
            foreach(var name in names){
                if(name == null || name.Equals(string.Empty))
                    continue;
                if(dict.ContainsKey(name)){
                    dict[name] += node.value;
                }else{
                    dict.Add(name, node.value);
                }
            }
        }
        string str = $"Total Cost: {totalCost}\n";
        foreach(var pair in dict){
            str += pair.Value > 0 ? pair.Key + ": +" + pair.Value + "\n" : pair.Key + "\n";
        }
        summary.text = str;
    }

    public void CreateNode(){
        mouseMenu.SetActive(false);
        GameObject node = Instantiate(nodePref.gameObject);
        newNode = node.GetComponent<Node>();
        GameObject edge = Instantiate(edgePref.gameObject);
        newEdge = edge.GetComponent<Edge>();
        newEdge.Connect(curNode, newNode);
        nodes.Add(newNode);
        edges.Add(newEdge);
    }

    public void CreateEdge(){
        mouseMenu.SetActive(false);
        GameObject edge = Instantiate(edgePref.gameObject);
        newEdge = edge.GetComponent<Edge>();
        newEdge.Connect(curNode, null);
        edges.Add(newEdge);
    }

    public void ClearAll(){
        foreach(var node in nodes){
            Destroy(node.gameObject);
        }
        foreach(var edge in edges){
            Destroy(edge.gameObject);
        }
        nodes.Clear();
        edges.Clear();
        var nodeObj = Instantiate(nodePref.gameObject);
        initialNode = nodeObj.GetComponent<Node>();
        nodes.Add(initialNode);
    }

    public void Save(){
        DataManager.GetInstance().SaveData(nodes, edges);
    }
    public void Load(){
        ClearAll();
        Destroy(initialNode.gameObject);
        nodes.Clear();
        DataManager.GetInstance().LoadData(nodes, edges, nodePref.gameObject, edgePref.gameObject);
        initialNode = nodes[0];
    }
}
