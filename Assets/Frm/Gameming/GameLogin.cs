using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogin : MonoBehaviour
{
    private static GameLogin RunTime;
    private iGameCtroller gameCtroller;
    private static GameObject rootGo;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        rootGo = new GameObject("rootTree");
        RunTime = rootGo.AddComponent<GameLogin>();
        DontDestroyOnLoad(rootGo);
    }
    private void Awake()
    {
        gameCtroller = GameController.instance;
        gameCtroller.Start();
    }
    void Start()
    {
        gameCtroller.Init();
    }
    void Update()
    {
        gameCtroller.Updata();
        
    }
    private void FixedUpdate()
    {
        gameCtroller.FixUpdata();
    }
    public static new Coroutine StartCoroutine(IEnumerator routine) => ((MonoBehaviour)RunTime).StartCoroutine(routine);
    public static new void StopCoroutine(Coroutine routine) => ((MonoBehaviour)RunTime).StopCoroutine(routine);

    public static void AttachToCirilla(GameObject go) => go.transform.SetParent(rootGo.transform);

    public static GameObject CirillaGiveBirth(GameObject prefabGo) => GameObject.Instantiate(prefabGo, rootGo.transform);
}
