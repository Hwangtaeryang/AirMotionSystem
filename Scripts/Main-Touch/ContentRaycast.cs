using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentRaycast : MonoBehaviour
{
    public static string raycastName;   //레이에 맞은 오브젝트 이름 변수

    void Start()
    {
    }

    
    void Update()
    {
        RayCastShow();
    }

    void RayCastShow()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 150, Color.red, 0.3f);

        if(Physics.Raycast(transform.position, transform.forward, out hit, 150f))
        {
            //Debug.Log(":::"+hit.collider.gameObject.name);  //ClickView1
            raycastName = hit.collider.gameObject.name; //hit된 오브젝트 이름
            
        }
    }
}
