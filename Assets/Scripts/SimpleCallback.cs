using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCallback : MonoBehaviour
{
    private Action greetingAction;      //액션 선언
    // Start is called before the first frame update
    void Start()
    {
        greetingAction = SayHello;      //Action 함수 할당
        PerformGreeting(greetingAction);
    }

    void SayHello()
    {
        Debug.Log("Hello, world");
    }
    // Update is called once per frame
    void PerformGreeting(Action greetingFunc)
    {
        greetingFunc?.Invoke();
    }
}
