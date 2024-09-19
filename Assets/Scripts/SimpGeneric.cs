using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpGeneric : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PrintValue(42);
        PrintValue("Gello, Generics!");
        PrintValue(3.14f);
    }
    void PrintValue<T>(T value)
    {
        Debug.Log($"value : {value}, Type : {typeof(T)}");
    }


}
