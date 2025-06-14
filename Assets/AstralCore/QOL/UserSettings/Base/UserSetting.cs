using System.Linq;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>
namespace AstralCore.QOL{
    /// <summary>
    /// Interface that allows us to encode/decode data
    /// </summary>
    public interface IUSetting{
        /// <summary>
        /// The name of this setting
        /// </summary>
        public string Name{ get; }

        /// <summary>
        /// Encodes setting for main script as a string
        /// </summary>
        /// <returns>The encoded string to the main script</returns>
        public string Encode();

        /// <summary>
        /// Decodes setting for caching
        /// </summary>
        /// <param name="data">The data to decode</param>
        public void Decode(string data);

        /// <summary>
        /// Decodes setting for caching
        /// </summary>
        /// <param name="data">The data to decode</param>
        public void Decode<T>(T data);

        /// <summary>
        /// Applies this class's values
        /// </summary>
        public void Apply();

        /// <summary>
        /// Called once, to setup this setting
        /// </summary>
        public void Init();
    }
    /// <summary>
    /// Base class for settings to be built off of
    /// </summary>
    public abstract class UserSetting : IUSetting{
        
        readonly string name;
        public string Name => name;

        public UserSetting(string name) => this.name = name;
        

        public abstract string Encode();
        public abstract void Decode(string data);
        public void Decode<T>(T data){
            var fields = typeof(T).GetFields();
            var values = fields.Select(f => f.GetValue(data)?.ToString());
            Decode(string.Join(",", values));            
        }
        public abstract void Apply();
        public abstract void Init();

        /// <summary>
        /// Allows us to split
        /// </summary>
        /// <param name="rawData">To be to processed</param>
        /// <param name="data">Data array</param>
        protected void Decode(string rawData, out string[] data) => data = rawData.Split(',');
    }
}