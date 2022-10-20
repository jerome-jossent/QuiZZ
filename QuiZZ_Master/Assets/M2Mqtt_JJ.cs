using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt.Messages;

public class M2Mqtt_JJ : MonoBehaviour
{
    [SerializeField] M2MqttUnity_JJ m2MqttUnityClient;
    [SerializeField] UnityEngine.UI.InputField topicInputField;



    public void _Subscribe()
    {
        string topic = topicInputField.text;
        m2MqttUnityClient.Subscribe(topic, M2MqttUnity_JJ.QOS.QOS_2_LEVEL_EXACTLY_ONCE);
    }
}