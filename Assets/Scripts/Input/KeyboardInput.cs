using System;
using UnityEngine;

namespace Input
{
    public class KeyboardInput : MonoBehaviour, IInput
    {
        
        public event Action Up;
        public event Action Down;
        public event Action Left;
        public event Action Right;
        
        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                Up();
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                Down();
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                Left();
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                Right();
            }
        }

    }

    public interface IInput
    {
        event Action Up;
        event Action Down;
        event Action Left;
        event Action Right;
    }
}