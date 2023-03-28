using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    public int num;

    // Start is called before the first frame update
    void Start()
    {
        test2.test.print(num, this.gameObject.name);
    }

 
}
