﻿using System.IO;

namespace net.derpaul.tf.plugin
{
    /// <summary>
    /// Write collection of Tinkerforge Sensor plugin values to a file
    /// </summary>
    public class File : IDataSink
    {
        /// <summary>
        /// Get the name of class
        /// </summary>
        public string Name { get { return this.GetType().Name; } }

        /// <summary>
        /// Stream to write data to
        /// </summary>
        private StreamWriter Datafile { get; set; }

        /// <summary>
        /// Flags successful initialization
        /// </summary>
        public bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Close the file
        /// </summary>
        public void Shutdown()
        {
            Datafile.Flush();
            Datafile.Close();
        }

        /// <summary>
        /// Write sensor values to file
        /// </summary>
        /// <param name="SensorValue">Tinkerforge Sensor plugin value</param>
        public void HandleValue(MeasurementValue SensorValue)
        {
            Datafile.WriteLine(SensorValue.ToJSON());
        }

        /// <summary>
        /// Console does not require an init
        /// </summary>
        /// <returns>signal success with true</returns>
        public bool Init()
        {
            Datafile = new StreamWriter(FileConfig.Instance.DataFilename, FileConfig.Instance.AppendToFile);

            bool success = System.IO.File.Exists(FileConfig.Instance.DataFilename);
            return success;
        }
    }
}