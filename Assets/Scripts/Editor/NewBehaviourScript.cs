using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class NewBehaviourScript
{
    [MenuItem("MyMenu/DoSomething")]
    static void DoSomething()
    {
        Debug.Log("Menu clicked!");
    }
}