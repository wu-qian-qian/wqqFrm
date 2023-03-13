using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
public class AssetInfo 
{
    public Object obj;
    public string bundleName;
    public AssetInfo(Object obj,string bundleName)
    {
        this.obj = obj;
        this.bundleName = bundleName;
    }
}
