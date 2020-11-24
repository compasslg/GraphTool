using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditMenu : MonoBehaviour
{
    public InputField nameWidget;
    public InputField valueWidget;
    public Dropdown costWidget;
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        if(gameController.curNode == null){
            gameObject.SetActive(false);
            return;
        }
        nameWidget.text = gameController.curNode.label;
        if(gameController.curNode.value > 0)
            valueWidget.text = gameController.curNode.value.ToString();
        costWidget.value = gameController.curNode.cost - 1;
    }


    public void Close(){
        if(nameWidget.text == null || nameWidget.text.Equals(string.Empty)){
            return;
        }
        gameObject.SetActive(false);
    }

    void OnDisable(){
        if(gameController.curNode == null){
            return;
        }
        gameController.curNode.label = nameWidget.text;
        int value;
        if(int.TryParse(valueWidget.text, out value)){
            gameController.curNode.value = value;
        }else{
            gameController.curNode.value = -1;
        }
        gameController.curNode.SetCost(costWidget.value + 1);
        gameController.curNode = null;
        gameController.CalculateInfo();
        nameWidget.text = "";
        valueWidget.text = "";
    }
    
}
