using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class VirtualButtons : MonoBehaviour, IVirtualButtonEventHandler
{
    VirtualButtonBehaviour[] virtualButtonBehaviours;
    string vbName;
    public GameObject canvas;
    public Text passcodeText;
    string passcode="";

    public GameObject[] Buttons =new GameObject[2];
    int counter = 0;
    int setSize = 5;

    public Material[] materials = new Material[10];

    void Start()
    {

        
        //Register with the virtual buttons TrackableBehaviour
        virtualButtonBehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < virtualButtonBehaviours.Length; i++)
        {
            Debug.Log("Registering button: " + i);
            virtualButtonBehaviours[i].RegisterEventHandler(this);
        }


       // GameObject.Find("VB1").GetComponent<MeshRenderer>().material = materials[2];




    }

    void update()
    {
        
    }

    private void showHideButtons()
    {

        GameObject.Find("P1").GetComponent<MeshRenderer>().material = materials[counter*2];
        GameObject.Find("P2").GetComponent<MeshRenderer>().material = materials[counter*2 + 1];
        
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        vbName = vb.VirtualButtonName;
        Debug.Log("OnButtonPressed: "+vbName);
        

        if(vbName == "D")
        {
            if(passcode.Length > 0)
            {
                passcode = passcode.Remove(passcode.Length - 1, 1);
                passcodeText.text = passcode;
            }
                
        }
        else if (vbName =="E")
        {

            gameObject.GetComponent<SensorReader>().publish(passcode);
            passcode = "";
            passcodeText.text = passcode;

        }
        else if(vbName == "F")
        {
            counter = (counter + 1) % setSize;
            showHideButtons();
            

        }
        else if(vbName == "B")
        {
            if(counter == 0)
            {
                counter = setSize-1;
            }
            else
            {
                counter = (counter - 1) % setSize;
            }
            
            showHideButtons();
        }
        else if(vbName == "VB1")
        {
            passcode += counter*2+1;
            passcodeText.text = passcode;
        }
        else if(vbName == "VB2")
        {
            passcode += counter * 2+2;
            passcodeText.text = passcode;
        }
        
        
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased: "+vbName);
        //for (int i = 0; i < Buttons.Length; i++)
            //Buttons[i].SetActive(true);
    }

    public void updateMessageFromMQTT(string message)
    {

        passcodeText.text = message;
    }

    
}
