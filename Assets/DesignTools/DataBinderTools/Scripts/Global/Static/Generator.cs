using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public static class Generator
{
   public static List<DataBinder> GenerateListFromData(JSONNode json, DataBinder prefab, Transform holder, int maxCount = 0)
    {
        List<DataBinder> generatedList = new List<DataBinder>();

        if (json == null || json.Count == 0)
        {
            Debug.LogWarning("PAYLOAD WARNING: The json does not contain an array to generate from.");
            return generatedList;
        }

        int numberToGenerate = json.Count;

        if (maxCount > 0)
            numberToGenerate = maxCount;
        
        for (int i = 0; i < numberToGenerate; i++)
        {
            JSONNode item = json[i];

            DataBinder newInstance = MonoBehaviour.Instantiate(prefab, holder);
            newInstance.RegisterData(item);
            newInstance.BindData();
            generatedList.Add(newInstance);
        }

        return generatedList;
    }

    public static DataBinder GenerateSingleDataBinder(JSONNode json, DataBinder prefab, Transform holder)
    {
        DataBinder newInstance = MonoBehaviour.Instantiate(prefab, holder);
        newInstance.RegisterData(json);
        newInstance.BindData();

        return newInstance;
    }
}
