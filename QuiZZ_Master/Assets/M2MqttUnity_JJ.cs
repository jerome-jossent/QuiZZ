using M2MqttUnity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using uPLibrary.Networking.M2Mqtt.Messages;

public class M2MqttUnity_JJ : M2MqttUnityTest
{
    public enum QOS { QOS_0_LEVEL_AT_MOST_ONCE, QOS_1_LEVEL_AT_LEAST_ONCE, QOS_2_LEVEL_EXACTLY_ONCE }

    byte GetQOS(QOS qos)
    {
        byte _qos;
        switch (qos)
        {
            case QOS.QOS_0_LEVEL_AT_MOST_ONCE: _qos = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE; break;
            case QOS.QOS_1_LEVEL_AT_LEAST_ONCE: _qos = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE; break;
            case QOS.QOS_2_LEVEL_EXACTLY_ONCE: default: _qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE; break;
        }
        return _qos;
    }

    public void Subscribe(string topic, QOS qos)
    {
        if (client == null)
        {
            Debug.Log("ARG");
            return;
        }
        Subscribe(new string[] { topic }, new byte[] { GetQOS(qos) });
    }

    public void Subscribe(string[] topics, QOS[] qos)
    {
        List<byte> _qos = new List<byte>();
        for (int i = 0; i < topics.Length; i++)        
            _qos.Add(GetQOS(qos[i]));        
        Subscribe(topics, _qos.ToArray());
    }

    public void Subscribe(string[] topics, byte[] qos)
    {
        client.Subscribe(topics, qos);
    }
}
