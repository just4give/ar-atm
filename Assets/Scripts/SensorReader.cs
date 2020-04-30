using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Net;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;


public class SensorReader : MonoBehaviour
{

    private MqttClient client;
    private System.Random random = new System.Random();
    private bool isCoroutineExecuting = false;
    private bool changed = false;
    private string mqttMessage="";

    SensorData sensorData = new SensorData();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to MQTT ");
        client = new MqttClient("broker.hivemq.com",1883 , false , null ); 
		
		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "/ABC/ATM/ACK" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

        //string strValue = Convert.ToString(value); 
 
        // publish a message on "/home/temperature" topic with QoS 2 
        //client.Publish("/home/temperature", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE); 
 
    }

    public void publish(String message)
    {
        Debug.Log("publishing message");
        
        client.Publish("/ABC/ATM/RCV", System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 

        mqttMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log(" message received from MQTT server "+ mqttMessage);
        changed = true;


    } 


    // Update is called once per frame
    void Update()
    {

        //StartCoroutine(UpdateWithDelay(5));
        if (changed)
        {

            changed = false;
            gameObject.GetComponent<VirtualButtons>().updateMessageFromMQTT(mqttMessage);
        }


    }

    IEnumerator UpdateWithDelay(float time) {
        if (isCoroutineExecuting)
         yield break;
        
        isCoroutineExecuting = true;
 
        yield return new WaitForSeconds(time);

        UpdateComponent();

        isCoroutineExecuting = false;
    }

    void UpdateComponent(){
        if (changed)
        {
            changed = false;
            gameObject.GetComponent<VirtualButtons>().updateMessageFromMQTT(mqttMessage);
        }
       

    } 
}
