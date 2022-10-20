using M2MqttUnity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using UnityEngine.UI;

public class M2MqttUnity_JJ : M2MqttUnityClient
{
    [SerializeField] GameObject buzzers;
    [SerializeField] GameObject buzzerPrefab;
    public Dictionary<string, Buzzer> buzzers_dico = new Dictionary<string, Buzzer>();

    private List<MqttMessage_jj> eventMessages = new List<MqttMessage_jj>();

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

    #region Subscribe
    public void Subscribe(string topic, QOS qos)
    {
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
        if (client == null)
        {
            Debug.Log("Subscribe : pas de client");
            return;
        }
        client.Subscribe(topics, qos);
    }
    #endregion

    public void ProcessMessage(MqttMessage_jj msg)
    {
        switch (msg.topic)
        {
            case MqttMessage_jj.Topic.newPlayer:
                //pour debug
                AddUiMessage("ProcessMessage : " + msg);

                BuzzNew(msg.message);
                break;

            case MqttMessage_jj.Topic.tempo:
                AddUiMessage("ProcessMessage : " + msg);
                break;

            case MqttMessage_jj.Topic.buzz:
                //pour debug
                AddUiMessage("ProcessMessage : " + msg);

                BuzzManagement(msg.message);
                break;

            case MqttMessage_jj.Topic.autre:
                AddUiMessage("ProcessMessage : " + msg);
                break;
        }
    }

    void BuzzNew(string msg)
    {
        if (!buzzers_dico.ContainsKey(msg))
        {
            //créer buzzer
            GameObject b = Instantiate(buzzerPrefab, buzzers.transform);
            b.name = msg;
            Buzzer bz = b.GetComponent<Buzzer>();
            bz._SetNameAndColor(msg);
            buzzers_dico.Add(b.name, bz);
        }
    }

    void BuzzManagement(string msgbuzzer)
    {
        char cara = ';';
        string[] nom_action = msgbuzzer.Split(cara);

        if (!buzzers_dico.ContainsKey(nom_action[0]))
        {
            BuzzNew(nom_action[0]);
        }

        Buzzer bz = buzzers_dico[nom_action[0]];
        bz._Action(nom_action[1]);
    }







    #region LEGACY

    [Header("User Interface")]
    public InputField consoleInputField;
    public Toggle encryptedToggle;
    public InputField addressInputField;
    public InputField portInputField;
    public Button connectButton;
    public Button disconnectButton;
    public Button testPublishButton;
    public Button clearButton;

    private bool updateUI = false;

    public void SetBrokerAddress(string brokerAddress)
    {
        if (addressInputField && !updateUI)
            this.brokerAddress = brokerAddress;
    }

    public void SetBrokerPort(string brokerPort)
    {
        if (portInputField && !updateUI)
            int.TryParse(brokerPort, out this.brokerPort);
    }

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    public void SetUiMessage(string msg)
    {
        if (consoleInputField != null)
        {
            consoleInputField.text = msg;
            updateUI = true;
        }
    }

    List<string> ui_messages = new List<string>();
    int ui_messages_max = 20;

    public void AddUiMessage(string msg)
    {
        if (consoleInputField != null)
        {
            ui_messages.Add(msg);

            while (ui_messages.Count > ui_messages_max)
                ui_messages.RemoveAt(0);

            consoleInputField.text = string.Join("\n", ui_messages);
            updateUI = true;
        }
        Debug.Log("AddUiMessage: " + msg);
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
        SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        SetUiMessage("Connected to broker on " + brokerAddress + "\n");
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "buzzer/BUZZ" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "buzzer/NewPlayer" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "buzzer/BUZZ" });
        client.Unsubscribe(new string[] { "buzzer/NewPlayer" });
    }

    protected override void OnConnectionFailed(string errorMessage) { AddUiMessage("CONNECTION FAILED! " + errorMessage); }
    protected override void OnDisconnected() { AddUiMessage("Disconnected."); }
    protected override void OnConnectionLost() { AddUiMessage("CONNECTION LOST!"); }

    private void UpdateUI()
    {
        if (client == null)
        {
            if (connectButton != null)
            {
                connectButton.interactable = true;
                disconnectButton.interactable = false;
                testPublishButton.interactable = false;
            }
        }
        else
        {
            if (testPublishButton != null)
            {
                testPublishButton.interactable = client.IsConnected;
            }
            if (disconnectButton != null)
            {
                disconnectButton.interactable = client.IsConnected;
            }
            if (connectButton != null)
            {
                connectButton.interactable = !client.IsConnected;
            }
        }
        if (addressInputField != null && connectButton != null)
        {
            addressInputField.interactable = connectButton.interactable;
            addressInputField.text = brokerAddress;
        }
        if (portInputField != null && connectButton != null)
        {
            portInputField.interactable = connectButton.interactable;
            portInputField.text = brokerPort.ToString();
        }
        if (encryptedToggle != null && connectButton != null)
        {
            encryptedToggle.interactable = connectButton.interactable;
            encryptedToggle.isOn = isEncrypted;
        }
        if (clearButton != null && connectButton != null)
        {
            clearButton.interactable = connectButton.interactable;
        }
        updateUI = false;
    }

    protected override void Start()
    {
        SetUiMessage("Ready.");
        updateUI = true;
        base.Start();

        ClearAllChildren._ClearAllChildren(buzzers);
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("Received: " + msg);
        eventMessages.Add(new MqttMessage_jj(topic, msg));
    }

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()

        if (eventMessages.Count > 0)
        {
            foreach (MqttMessage_jj msg in eventMessages)
                ProcessMessage(msg);

            eventMessages.Clear();
        }
        if (updateUI)
            UpdateUI();
    }

    private void OnDestroy()
    {
        Disconnect();
    }
    #endregion
}