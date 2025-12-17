using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public static class JsonNodeBuilder
{
    /// <summary>
    /// Recursively creates the node structure for fetching data from a Json file
    /// </summary>
    /// <param name="json">json file to parse</param>
    /// <param name="nodeSteps">array of strings determining each step to take in the json to reach the data</param>
    /// <param name="nodeStructure">final JSONNode to output back in the system</param>
    /// <param name="step">current step the control is on</param>
    public static void GetCompleteNodeStructure(JSONNode json, string[] nodeSteps, out JSONNode nodeStructure, int index, int step = 0)
    {
        int indexValue;
        JSONNode newNode = json;

        if(json == null)
        {
            Debug.LogError($"PAYLOAD ERROR: The jsonNode is null at the {step} step. Check the payload to ensure the fields exists as expected.");
            nodeStructure = null;
            return;
        }

        //if there is no structure then return the full json
        if (nodeSteps.Length == 0)
        {
            nodeStructure = json;
            return;
        }

        //add node to structure//
        //if the current node is an array
        if (json.IsArray)
        {
            //if the step is 'i' then set the next node as the index value passed
            if(nodeSteps[step] == "i" || nodeSteps[step] == "index")
            {
                newNode = json[index];
            }

            //check to see if the step is an integer and use it as an index
            else if (int.TryParse(nodeSteps[step], out indexValue))
            {
                //if the index is within the length of the node's array add it as the next node
                if (indexValue <= json.Count - 1)
                    newNode = json[indexValue];

                //otherwise the int is not within the expected length so throw an error and return out
                else
                {
                    Debug.LogError($"The submitted index is outside of the step {step} array's bounds.");
                    nodeStructure = null;
                    return;
                }
            }

            //otherwise the step is not an integer so throw and error and return out
            else
            {
                Debug.LogError($"The current step {step} is an array and requires an int value. {nodeSteps[step]} is not an int");
                nodeStructure = null;
                return;
            }
        }

        //otherwise the current node is not an array so fill it with the string
        else
        {
            //if the step has value then add it as the next node
            if (nodeSteps[step] != null && !nodeSteps[step].Contains(" "))
            {
                newNode = json[nodeSteps[step]];
            }

            //otherwise the step is not valude so throw and error and return out
            else
            {
                Debug.LogError($"The current step {step} is empty or contains a space And therefor is not valid.");
                nodeStructure = null;
                return;
            }
        }

        //if there are still steps left to iterate move onto the next step
        if (step < nodeSteps.Length - 1)
        {
            step++;           
            GetCompleteNodeStructure(newNode, nodeSteps, out nodeStructure, index, step);
        }

        //otherwise set the nodeStructure and return out
        else
        {
            nodeStructure = newNode;
            return;
        }
    }
}
