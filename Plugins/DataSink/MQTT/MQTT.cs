using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace net.derpaul.tf.plugin
{
    /// <summary>
    /// Data sink - sending data using MQTT
    /// </summary>
    public class MQTT : DataSinkBase
    {
        /// <summary>
        /// Instance of M2Mqtt client
        /// </summary>
        private MqttClient MqttClient;

        /// <summary>
        /// Timer to check the acknowledge list
        /// </summary>
        private Timer RunCheck;

        /// <summary>
        /// Memorize event handler
        /// </summary>
        private ElapsedEventHandler RunCheckEvent;

        /// <summary>
        /// Published measurement values waiting for acknowledge
        /// </summary>
        private Dictionary<string, MeasurementValue> AcknowledgeList { get; set; }

        /// <summary>
        /// Disconnect from MQTT broker
        /// </summary>
        public override void Shutdown()
        {
            lock (WriteLock)
            {
                if (MQTTConfig.Instance.Handshake == true)
                {
                    MqttClient.MqttMsgPublishReceived -= MqttAcknowledgeRecieved;
                }
                MqttClient.Disconnect();

                if (MQTTConfig.Instance.Handshake == true)
                {
                    RunCheck.Elapsed -= RunCheckEvent;
                    RunCheck.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Initialize MQTT client and connect to broker
        /// </summary>
        /// <returns>true on success otherwise false</returns>
        public override bool Init()
        {
            bool success = false;

            try
            {
                MQTTConfig.Instance.ShowConfig();

                AcknowledgeList = new Dictionary<string, MeasurementValue>();

                if (MQTTConfig.Instance.Handshake == true)
                {
                    RunCheck = new Timer(MQTTConfig.Instance.TimerDelay);
                    RunCheckEvent = new ElapsedEventHandler(CheckAcknowledgeList);
                    RunCheck.Elapsed += RunCheckEvent;
                    RunCheck.Enabled = true;
                }

                MqttClient = new MqttClient(MQTTConfig.Instance.BrokerIP, MQTTConfig.Instance.BrokerPort, false, null, null, MqttSslProtocols.None);
                MqttClient.Connect(MQTTConfig.Instance.ClientID);
                MqttClient.Subscribe(new string[] { MQTTConfig.Instance.TopicAcknowledge }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                if (MQTTConfig.Instance.Handshake == true)
                {
                    MqttClient.MqttMsgPublishReceived += MqttAcknowledgeRecieved;
                }

                success = MqttClient.IsConnected;
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"{nameof(Init)}: Cannot connect to broker [{MQTTConfig.Instance.BrokerIP}] => [{e.Message}]");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Transform each result in a JSON string and publish string to topic
        /// </summary>
        /// <param name="SensorValue">Sensor value</param>
        public override void HandleValue(MeasurementValue SensorValue)
        {
            lock (WriteLock)
            {
                PublishSingleValue(SensorValue);
            }
        }

        /// <summary>
        /// Publish single measurement data
        /// </summary>
        /// <param name="dataToPublish"></param>
        private void PublishSingleValue(MeasurementValue dataToPublish)
        {
            var dataJSON = dataToPublish.ToJSON();

            if (MQTTConfig.Instance.Handshake == true)
            {
                lock (WriteLock)
                {
                    AcknowledgeList.Add(dataToPublish.ToHash(), dataToPublish);
                }
            }

            MqttClient.Publish(MQTTConfig.Instance.TopicData, Encoding.ASCII.GetBytes(dataJSON));
        }

        /// <summary>
        /// Handles response of remotedevice and removes measurement values from internal acknowledge list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void MqttAcknowledgeRecieved(object sender, MqttMsgPublishEventArgs e)
        {
            string messageHash = Encoding.UTF8.GetString(e.Message);

            lock (WriteLock)
            {
                if (AcknowledgeList.ContainsKey(messageHash))
                {
                    AcknowledgeList.Remove(messageHash);
                }
            }
        }

        /// <summary>
        /// Check in interval the acknowledgelist for not acknowledged entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void CheckAcknowledgeList(object sender, ElapsedEventArgs e)
        {
            lock (WriteLock)
            {
                var NumberOfValues = AcknowledgeList.Count;
                if (NumberOfValues > 0)
                {
                    System.Console.WriteLine($"Try to resend [{NumberOfValues}] measurement values.");
                    foreach (var Data in AcknowledgeList)
                    {
                        AcknowledgeList.Remove(Data.Key);
                        PublishSingleValue(Data.Value);
                    }
                }
            }
        }
    }
}