using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkAreaController : MonoBehaviour
{
    bool sizeChangeFlag;
    bool RotateChangeFlag;
    bool MoveFlag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SizeChange()
    {
        Debug.Log("SizeChange");
        sizeChangeFlag = !sizeChangeFlag;
    }
    void RotateChange()
    {
        Debug.Log("RotateChange");
        RotateChangeFlag = !RotateChangeFlag;
    }
    void Move()
    {
        Debug.Log("Move");
        MoveFlag = !MoveFlag;
    }
}
