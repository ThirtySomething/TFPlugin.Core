﻿namespace net.derpaul.tf.plugin
{
    /// <summary>
    /// Configuration settings of temperature sensor
    /// </summary>
    public class TemperatureConfig : ConfigLoader<TemperatureConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            ReadDelay = TFUtils.DefaultDelay;
            SortOrder = 1;
        }

        /// <summary>
        /// Delay until next measurement value is read
        /// </summary>
        public int ReadDelay { get; set; }

        /// <summary>
        /// Sort order for temperature
        /// </summary>
        public int SortOrder { get; set; }
    }
}