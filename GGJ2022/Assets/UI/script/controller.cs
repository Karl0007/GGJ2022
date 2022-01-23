using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIController
{
    public class controller : MonoBehaviour
    {
        private string Horizontal = "Horizontal_";
        private string Vertical = "Vertical_";
        private string Jump = "Jump_";
        private string Attack = "Attack_";
        private string Dash = "Dash_";
        private string Atokin = "Atokin_";

        void Update()
        {
            /*for (int i = 1; i < 5; i++)
            {
                if (Mathf.Abs(Input.GetAxis(Horizontal + i)) > 0.2 || Mathf.Abs(Input.GetAxis(Vertical + i)) > 0.2)
                {
                    Debug.Log(Input.GetJoystickNames()[i - 1]  +  " is moved" + i);
                }
                if (Mathf.Abs(Input.GetAxis(Atokin + i)) > 0.2 )
                {
                    Debug.Log(Input.GetJoystickNames()[i - 1]  +  " is atokin" + i);
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    Debug.Log(Input.GetJoystickNames()[0] + " is jump" + i);
                }
                  if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                {
                    Debug.Log(Input.GetJoystickNames()[0] + " is jump" + i);
                }
                  if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                {
                    Debug.Log(Input.GetJoystickNames()[0] + " is jump" + i);
                }
                  if (Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    Debug.Log(Input.GetJoystickNames()[0] + " is jump" + i);
                }
                 if (Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    Debug.Log(Input.GetJoystickNames()[i - 1] + " is dash" + i);
                }
                 if (Input.GetKeyDown(KeyCode.JoystickButton2))
                {
                    Debug.Log(Input.GetJoystickNames()[i - 1] + " is attack" + i);
                }
            }*/


            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                Debug.Log(Input.GetJoystickNames() + " is jump");
            }
            if (Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                Debug.Log(Input.GetJoystickNames() + " is jump" );
            }
            if (Input.GetKeyDown(KeyCode.Joystick3Button0))
            {
                Debug.Log(Input.GetJoystickNames() + " is jump" );
            }
            if (Input.GetKeyDown(KeyCode.Joystick4Button0))
            {
                Debug.Log(Input.GetJoystickNames() + " is jump" );
            }
        }
    }
}
