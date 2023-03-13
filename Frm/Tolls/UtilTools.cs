using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UnitTools
{
   public static GameObject GetChild(GameObject goParents,string name)
    {
        Transform[] transforms = goParents.GetComponentsInChildren<Transform>();
        if (transforms.Length <= 0)
            return null;
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name == name)
                return transforms[i].gameObject;
        }
        return null;
    }
}
