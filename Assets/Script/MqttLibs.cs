using M2MqttUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MqttLibs : M2MqttUnityClient
{
    [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
    public bool autoTest = false;
    [Header("User Interface")]
    public InputField consoleInputField;
    public Toggle encryptedToggle;
    public InputField addressInputField;
    public InputField portInputField;
    public Button connectButton;
    public Button disconnectButton;
    public Button testPublishButton;
    public Button clearButton;

    private List<string> eventMessages = new List<string>();
    private bool updateUI = false;

	public TextMeshProUGUI objText;

    public void TestPublish()
    {
        client.Publish("/innovation/valvecontroller/eHealthAR", System.Text.Encoding.UTF8.GetBytes("Hello may cung"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("Test message published");
        AddUiMessage("Test message published.");
    }

    public void Pub_WAIT()
    {
        client.Publish("/innovation/valvecontroller/eHealthAR", System.Text.Encoding.UTF8.GetBytes("WAIT: 0968910017"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("WAIT message published");
        AddUiMessage("WAIT message published.");
    }

    // protected override void DecodeMessageKiosk(string topic, byte[] message)
    // {
    //     string msg = System.Text.Encoding.UTF8.GetString(message);
    //     Debug.Log("***Received: " + msg);
    //     objText.SetText(msg);
    //     StoreMessage(msg);

    //     // Split message based on colon (":")
    //     string[] parts = msg.Split(':');
    //     if (parts.Length == 2)
    //     {
    //         string id = parts[0];
    //         string sensorData = parts[1];

    //         // Split sensor data by comma (",")
    //         string[] sensorValues = sensorData.Split(',');
    //         float[] parsedValues = new float[sensorValues.Length];

    //         // Parse each sensor value to float
    //         if (sensorValues.Length == 6)
    //         {
    //             for (int i = 0; i < sensorValues.Length; i++)
    //             {
    //                 parsedValues[i] = float.Parse(sensorValues[i]);
    //             }

    //             // Create a JSON object
    //             var jsonObject = new Dictionary<string, object>()
    //             {
    //                 { "phone_number", id },
    //                 { "weight", parsedValues[0] },
    //                 { "height", parsedValues[1] },
    //                 { "temp", parsedValues[2] },
    //                 { "pulse", parsedValues[3] },
    //                 { "breath", parsedValues[4] },
    //                 { "blood", parsedValues[5] },
    //             };

    //             // Send HTTP POST request with JSON data (assuming you have a library for HTTP requests)
    //             StartCoroutine(SendHttpPostRequest(jsonObject, "https://lhmqk.pythonanywhere.com/records/update"));
    //         }
    //         else
    //         {
    //             Debug.LogError("Invalid number of sensor values. Expected 6, received: " + sensorValues.Length);
    //         }
    //     }
    // }

    IEnumerator SendHttpPostRequest(Dictionary<string, object> jsonObject, string url)
    {
        string jsonData = JsonUtility.ToJson(jsonObject);

        var byteArray = Encoding.UTF8.GetBytes(jsonData);

        using (var www = new WWW(url, byteArray, new Dictionary<string, string>() { { "Content-Type", "application/json" } }))
        {
            yield return www;

            if (www.isDone)
            {
                if (www.isSuccess)
                {
                    Debug.Log("HTTP POST successful: " + www.text);
                }
                else
                {
                    Debug.LogError("HTTP POST failed: " + www.error);
                }
            }
        }
    }


    public void ResetPublish()
    {
        client.Publish("/innovation/valvecontroller/eHealthAR", System.Text.Encoding.UTF8.GetBytes("idle"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("Reset publish");
    }


    public void SetBrokerAddress(string brokerAddress)
    {
        if (addressInputField && !updateUI)
        {
            this.brokerAddress = brokerAddress;
        }
    }

    public void SetBrokerPort(string brokerPort)
    {
        if (portInputField && !updateUI)
        {
            int.TryParse(brokerPort, out this.brokerPort);
        }
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

    public void AddUiMessage(string msg)
    {
        if (consoleInputField != null)
        {
            consoleInputField.text += msg + "\n";
            updateUI = true;
        }
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

        if (autoTest)
        {
            TestPublish();
        }
    }

    protected override void SubscribeTopics()
    {
        Debug.Log("Subcribed");
        client.Subscribe(new string[] { "/innovation/valvecontroller/eHealthAR" }, 
            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        
        
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { "innovation/valvecontroller/eHealthAR" });
        
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        AddUiMessage("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        AddUiMessage("Disconnected.");
    }

    protected override void OnConnectionLost()
    {
        AddUiMessage("CONNECTION LOST!");
    }

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
    }

    
    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("***Received: " + msg);
        objText.SetText(msg);
        StoreMessage(msg);

        // Split message based on colon (":")
        string[] parts = msg.Split(':');
        if (parts.Length == 2)
        {
            string id = parts[0];
            string sensorData = parts[1];

            // Split sensor data by comma (",")
            string[] sensorValues = sensorData.Split(',');
            float[] parsedValues = new float[sensorValues.Length];

            // Parse each sensor value to float
            if (sensorValues.Length == 6)
            {
                for (int i = 0; i < sensorValues.Length; i++)
                {
                    parsedValues[i] = float.Parse(sensorValues[i]);
                }

                // Create a JSON object
                var jsonObject = new Dictionary<string, object>()
                {
                    { "phone_number", id },
                    { "record_weight", parsedValues[0] },
                    { "record_height", parsedValues[1] },
                    { "record_temp", parsedValues[2] },
                    { "record_pulse", parsedValues[3] },
                    { "record_breath", parsedValues[4] },
                    { "record_blood", parsedValues[5] },
                };

                // Send HTTP POST request with JSON data (assuming you have a library for HTTP requests)
                StartCoroutine(SendHttpPostRequest(jsonObject, "https://lhmqk.pythonanywhere.com/records/update_kiosk"));
            }
            else
            {
                Debug.LogError("Invalid number of sensor values. Expected 6, received: " + sensorValues.Length);
            }
        }
    }

    private void StoreMessage(string eventMsg)
    {
        eventMessages.Add(eventMsg);
    }

    private void ProcessMessage(string msg)
    {
        AddUiMessage("Received: " + msg);
    }

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
        
        if (eventMessages.Count > 0)
        {
            foreach (string msg in eventMessages)
            {
                ProcessMessage(msg);
            }
            eventMessages.Clear();
        }
        if (updateUI)
        {
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }
}