using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        RectTransform rectTrans = GetComponent<RectTransform>();
        float xPivot = 0, yPivot = 0;
        if(Input.mousePosition.x > Screen.width / 2){
            xPivot = 1;
        }else{
            xPivot = 0;
        }
        if(Input.mousePosition.y > Screen.height / 2){
            yPivot = 1;
        }else{
            yPivot = 0;
        }
        rectTrans.pivot = new Vector2(xPivot, yPivot);
        transform.position = Input.mousePosition;
    }
}
