
using UnityEngine;

public   interface IGoPoolModule
    {
    GameObject Acquire(string prefab);
    void Recycle(GameObject go);
    void Load(GameObject prefab, int capacity);
    void Unload(GameObject prefab);
    void Shrink();
    void Clear();
}
