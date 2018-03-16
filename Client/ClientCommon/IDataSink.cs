﻿using System.Collections.Generic;

namespace net.derpaul.tf
{
    /// <summary>
    /// Interface to deal with a collection of Tinkerforge Sensor plugin values
    /// </summary>
    public interface IDataSink
    {
        /// <summary>
        /// Flags successful initialization
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initialize plugin with loaded config
        /// </summary>
        /// <returns></returns>
        bool Init();

        /// <summary>
        /// Perform action on measurement values
        /// </summary>
        /// <param name="SensorValues">Collection of collected values</param>
        void HandleValues(List<MeasurementValue> SensorValues);

        /// <summary>
        /// Enable plugin to shutdown some resources
        /// </summary>
        void Shutdown();
    }
}