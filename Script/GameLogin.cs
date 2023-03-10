using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogin : MonoBehaviour
{
    private iGameCtroller gameCtroller;

    private void Awake()
    {
        gameCtroller = GameController.instance;
        gameCtroller.Start();
    }
    void Start()
    {
        gameCtroller.Init();
    }

    // Update is called once per frame
    void Update()
    {
        gameCtroller.Updata();
    }
    private void FixedUpdate()
    {
        gameCtroller.FixUpdata();
    }
}
