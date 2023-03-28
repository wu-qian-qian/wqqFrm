using System;
using UnityEngine;


  public  class GoPoolEntity:MonoBehaviour
    {
    private Action<GameObject> callback;
    public void Init(Action<GameObject> callback) => this.callback = callback;
    private void OnDestroy() => callback?.Invoke(gameObject);
}
