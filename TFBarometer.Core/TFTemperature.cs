﻿using System;

namespace net.derpaul.tf
{
    /// <summary>
    /// Class to read temperature using barometer sensor
    /// </summary>
    public class TFTemperature : TFBarometer
    {
        /// <summary>
        /// Read value from sensor and prepare real value
        /// </summary>
        /// <returns>Air pressure or 0.0</returns>
        protected override Tuple<string, double> ValueGetRaw()
        {
            if (_BrickletBarometer == null)
            {
                return new Tuple<string, double>(Name, 0.0);
            }

            int temperatureRaw = _BrickletBarometer.GetChipTemperature();
            double temperature = temperatureRaw / 100.0;

            return new Tuple<string, double>(Name, temperature);
        }
    }
}