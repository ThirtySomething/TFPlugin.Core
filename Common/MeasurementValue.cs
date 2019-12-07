﻿using Newtonsoft.Json;
using System;

namespace net.derpaul.tf
{
    /// <summary>
    /// Result of one measurement:
    /// - Measurement value
    /// - Unit of measurement value
    /// - Name/kind of sensor
    /// - Timestamp of recording
    /// </summary>
    public class MeasurementValue
    {
        /// <summary>
        /// Name/kind of sensor
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sort priority
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Timestamp of recording
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Unit of measurement value
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Measurement value
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeasurementValue() : this("", "", 1)
        {
        }

        /// <summary>
        /// Constructor to initialize values
        /// </summary>
        public MeasurementValue(string sensorName, string valueUnit, int sortOrder)
        {
            Name = sensorName;
            Timestamp = DateTime.Now;
            Unit = valueUnit;
            Value = 0.0;
            SortOrder = (sortOrder - 1);
            if (SortOrder < 0)
            {
                SortOrder = 0;
            }
        }

        /// <summary>
        /// Get object as JSON string
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Get object as hash
        /// </summary>
        /// <returns></returns>
        public string ToHash()
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] stringJSON = System.Text.Encoding.UTF8.GetBytes(ToJSON());
                byte[] stringHash = sha.ComputeHash(stringJSON);
                return BitConverter.ToString(stringHash).Replace("-", String.Empty);
            }
        }
    }
}