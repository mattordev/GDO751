using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <author>
/// ©️2025 Designed and Programmed by Matthew Roberts. All rights reserved.
/// </author>

namespace mattordev.util
{
    // Need to add to window menu for easy access.
    /// <summary>
    ///  A utility class for managing PlayerPrefs in Unity.
    ///  Provides methods to clear PlayerPrefs and export them to a file.
    /// </summary>
    ///  <remarks>
    ///  This class is designed to be used in the Unity Editor and provides context menu options
    ///   for clearing PlayerPrefs and exporting them to a file.
    ///  It is not intended for runtime use in a built game.
    /// </remarks>
    public class PlayerPrefsTool : MonoBehaviour
    {
        /// <summary>
        /// Clears all PlayerPrefs data.
        ///</summary
        [MenuItem("Tools/PlayerPrefs Tool/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.Debug.Log("All PlayerPrefs cleared.");
        }

        /// <summary>
        /// Exports all PlayerPrefs stored in the Windows Registry to a text file on the user's desktop.
        /// This method is intended for use in the Unity Editor on Windows only.
        /// It uses a system process to query the registry path where Unity stores Editor PlayerPrefs,
        /// and writes all key-value pairs to a file.
        /// </summary>
        /// <returns>True if the export was successful; false otherwise (e.g., if the registry path is not found or an error occurs).</returns>

        [MenuItem("Tools/PlayerPrefs Tool/Export PlayerPrefs to File")]
        public static bool ExportPlayerPrefsToJson()
        {
#if UNITY_EDITOR_WIN
            string filePath = "C:/Users/" + System.Environment.UserName + "/Desktop/PlayerPrefsExport.json";
            string companyName = Application.companyName;
            string productName = Application.productName;
            string registryPath = $@"HKEY_CURRENT_USER\Software\Unity\UnityEditor\{companyName}\{productName}";

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("reg", $"query \"{registryPath}\" /v *")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var jsonDict = new Dictionary<string, Dictionary<string, object>>();

                using (Process process = Process.Start(psi))
                using (StreamReader reader = process.StandardOutput)
                {
                    string line;
                    Regex regLine = new Regex(@"\s{4}([^\s]+)\s+([^\s]+)\s+(.+)", RegexOptions.Compiled);

                    while ((line = reader.ReadLine()) != null)
                    {
                        Match match = regLine.Match(line);
                        if (match.Success)
                        {
                            string key = match.Groups[1].Value;
                            string type = match.Groups[2].Value;
                            string rawValue = match.Groups[3].Value.Trim();

                            object value = rawValue;
                            string valueType = "unknown";

                            if (type == "REG_DWORD" && rawValue.StartsWith("0x"))
                            {
                                value = int.Parse(rawValue.Substring(2), System.Globalization.NumberStyles.HexNumber);
                                valueType = "int";
                            }
                            else if (type == "REG_BINARY")
                            {
                                byte[] bytes = HexStringToBytes(rawValue.Replace(" ", ""));

                                if (bytes.Length == 4)
                                {
                                    float floatVal = System.BitConverter.ToSingle(bytes, 0);
                                    value = floatVal;
                                    valueType = "float";
                                }
                                else
                                {
                                    try
                                    {
                                        string strVal = Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                                        if (!string.IsNullOrWhiteSpace(strVal))
                                        {
                                            value = strVal;
                                            valueType = "string";
                                        }
                                        else
                                        {
                                            value = System.BitConverter.ToString(bytes);
                                            valueType = "binary";
                                        }
                                    }
                                    catch
                                    {
                                        value = System.BitConverter.ToString(bytes);
                                        valueType = "binary";
                                    }
                                }
                            }

                            jsonDict[key] = new Dictionary<string, object>
                        {
                            { "type", valueType },
                            { "value", value }
                        };
                        }
                    }
                }

                string jsonOutput = JsonUtilityWrapper.ToJson(jsonDict, true);
                File.WriteAllText(filePath, jsonOutput);

                UnityEngine.Debug.Log($"PlayerPrefs exported as JSON to: {filePath}");
                return true;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to export PlayerPrefs: {ex.Message}");
                return false;
            }
#else
        UnityEngine.Debug.LogWarning("This export method is Windows Editor-only.");
        return false;
#endif
        }

        private static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length % 2 == 1) hex = "0" + hex;

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = System.Convert.ToByte(hex.Substring(i * 2, 2), 16);

            return result;
        }

        // Unity's JsonUtility doesn't handle dictionaries, so we use a workaround
        public static class JsonUtilityWrapper
        {
            public static string ToJson(Dictionary<string, Dictionary<string, object>> dict, bool pretty)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(dict, pretty
                    ? Newtonsoft.Json.Formatting.Indented
                    : Newtonsoft.Json.Formatting.None);
            }
        }
    }
}