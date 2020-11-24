using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool toggled;
    public SpriteRenderer sprite;
    public Color activeColor, passiveColor, statColor;
    public string label;
    public int value;
    public int cost;

    private GameController gameController;

    public HashSet<Edge> edges = new HashSet<Edge>();
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UnToggle();

    }



    // Update is called once per frame
    void Update(){
        
    }

    public void SetCost(int cost){
        this.cost = cost;
        switch(cost){
            case 3:
                sprite.color = statColor;
                break;
            case 4:
                sprite.color = passiveColor;
                break;
            default:
                sprite.color = activeColor;
                break;
        }
        if(!toggled){
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        }
    }

    public void UnToggleAll(){
        if(toggled)
            UnToggle();
        else
            return;

        foreach(var edge in edges){
            var node = edge.GetOtherNode(this);
            node.UnToggleAll();
        }
    }

    public void UnToggle(){
        if(edges.Count == 0){
            this.toggled = false;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            gameController.CalculateInfo();
            return;
        }
        if(this == gameController.initialNode)
            return;
        int toggledCount = 0;
        foreach(var edge in edges){
            if(edge.GetOtherNode(this).toggled){
                toggledCount++;
                Debug.Log(toggledCount);
                if(toggledCount > 1){
                    return;
                }
            }
        }
        
        toggled = false;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        gameController.CalculateInfo();
    }

    public void Toggle(){
        if(edges.Count == 0 || this == gameController.initialNode){
            this.toggled = true;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            gameController.CalculateInfo();
            return;
        }
        foreach(var edge in edges){
            if(edge.GetOtherNode(this).toggled){
                this.toggled = true;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                gameController.CalculateInfo();
                break;
            }
        }
    }

    public bool IsToggled(){
        return toggled;
    }
    public void AddChild(){

    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameController.newEdge != null)
            return;
        if(eventData.button == PointerEventData.InputButton.Left){
            if(!toggled){
                this.Toggle();
            }
            else{
                UnToggle();
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Right){
            gameController.curNode = this;
            gameController.ActivateMouseMenu();
        }
    }

    public void Delete(){
        foreach(var edge in edges){
            edge.GetOtherNode(this).edges.Remove(edge);
            Destroy(edge.gameObject);
        }
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameController.hoveredNode = this;
        gameController.ActivateInfoPanel(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameController.hoveredNode = null;
        gameController.DeactivateInfoPanel();
    }
}
