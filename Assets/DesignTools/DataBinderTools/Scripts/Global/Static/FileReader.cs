using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public static class FileReader
{
    /// <summary>
    /// Reads a text file and parses it into a usable JSON node
    /// </summary>
    /// <param name="path">Location of file</param>
    /// <returns></returns>
    public static JSONNode ReadJSONFromFile(string path)
    {
        if (path.Contains(".json"))
        {
            if (File.Exists(path))
                return JSON.Parse(File.ReadAllText(path));
            else
            {
                Debug.LogError($"File Does not exist: {path}");
                return null;
            }
        }
        else
        {
            Debug.LogError($"File {path} is not a .json file");
            return null;
        }
    }
}
