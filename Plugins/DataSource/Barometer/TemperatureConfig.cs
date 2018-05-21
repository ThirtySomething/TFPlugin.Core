﻿namespace net.derpaul.tf.plugin
{
    /// <summary>
    /// Configuration settings of temperature sensor
    /// </summary>
    public class TemperatureConfig : ConfigLoader<TemperatureConfig>, IConfigSaver
    {
        /// <summary>
        /// Sort order for temperature
        /// </summary>
        public int SortOrder = 0;
    }
}