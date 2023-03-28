using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class test2 : MonoBehaviour
{
    public static test2 test;
    private void Awake()
    {
        test = this;
    }
 
    public void print(int num,string name)
    {
        Task task;
  
        task = new Task((name) => { Debug.Log(name); },2);
        task.Start();
       
    }
    public void Show()
    {
        Debug.Log("´ðÓ¦³öshow");
    }
}
