using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoPanel : MonoBehaviour
{
    public Text info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    void OnEnable(){
        RectTransform rectTrans = GetComponent<RectTransform>();
        float xPivot = 0, yPivot = 0;
        if(Input.mousePosition.x > Screen.width / 2){
            xPivot = 1.1f;
        }else{
            xPivot = -0.1f;
        }
        if(Input.mousePosition.y > Screen.height / 2){
            yPivot = 1.1f;
        }else{
            yPivot = -0.1f;
        }
        rectTrans.pivot = new Vector2(xPivot, yPivot);
        
    }

    public void SetData(Node node){
        if(node.value == -1)
            info.text = $"Cost:\n{node.cost}\nName:\n{node.label}";
        else
            info.text = $"Cost:\n{node.cost}\nName:\n{node.label}\nValue:{node.value}";
    }
}
