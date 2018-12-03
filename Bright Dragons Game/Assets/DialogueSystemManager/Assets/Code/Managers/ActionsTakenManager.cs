using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace DialogueSystem.Managers
{
    /// <summary>
    /// ActionsTakenManager is where you will rig up all actions taken from any type of response. If a response needs to tell your game to do something
    /// add it to a response ActionTaken element in your conversation and then name the method the same as that action taken and this will proceed to do whatever you need to do.
    /// This is a singleton because you may need to leverage Unitys API
    /// </summary>
    public class ActionsTakenManager : Singleton<ActionsTakenManager>
    {
        //Normal Handlers -- Add any custom delegates you may need here
        public delegate void GameObjectActionHandler(GameObject goParameter);
        public delegate void FloatActionHandler(float floatParameter);
        public delegate void DoubleActionHandler(double doubleParameter);
        public delegate void IntActionHandler(int intParameter);
        public delegate void NoParameterHandler();

        //Can also do individual Unity Events for each ActionTaken if you wanted to see it visually in inspector
        //public UnityEvent customEvents;

        //Events go here
        
        public void Handshake()
        {
            Debug.Log("This was included specifically for the demo scene. It can be removed for your own work");
        }

    }
}