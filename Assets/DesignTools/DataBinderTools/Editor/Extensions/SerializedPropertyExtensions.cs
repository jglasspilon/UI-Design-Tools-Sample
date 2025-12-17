using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class SerializedPropertyExtensions
{
    /// <summary>
    /// Add an element to the array property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayProperty"></param>
    /// <param name="elementToAdd"></param>
    public static void AddToObjectArray<T> (this SerializedProperty arrayProperty, T elementToAdd)
        where T : Object
    {
        // If the SerializedProperty this is being called from is not an array, throw an exception.
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        // Pull all the information from the target of the serializedObject.
        arrayProperty.serializedObject.Update();

        // Add a null array element to the end of the array then populate it with the object parameter.
        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = elementToAdd;

        // Push all the information on the serializedObject back to the target.
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Removes a the serialized property at the specified index
    /// </summary>
    /// <param name="arrayProperty"></param>
    /// <param name="index"></param>
    public static void RemoveFromObjectArrayAt (this SerializedProperty arrayProperty, int index)
    {
        // If the index is not appropriate or the serializedProperty this is being called from is not an array, throw an exception.
        if(index < 0)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " cannot have negative elements removed.");

        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if(index > arrayProperty.arraySize - 1)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " has only " + arrayProperty.arraySize + " elements so element " + index + " cannot be removed.");

        // Pull all the information from the target of the serializedObject.
        arrayProperty.serializedObject.Update();

        // If there is a non-null element at the index, null it.
        if (arrayProperty.GetArrayElementAtIndex(index).GetPropertyObject() is UnityEngine.Object && arrayProperty.GetArrayElementAtIndex(index).objectReferenceValue)
            arrayProperty.DeleteArrayElementAtIndex(index);

        // Delete the null element from the array at the index.
        arrayProperty.DeleteArrayElementAtIndex(index);

        // Push all the information on the serializedObject back to the target.
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }


    /// <summary>
    /// Removes a specific serialized property from the array property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayProperty"></param>
    /// <param name="elementToRemove"></param>
    public static void RemoveFromObjectArray<T> (this SerializedProperty arrayProperty, T elementToRemove)
        where T : Object
    {
        // If either the serializedProperty doesn't represent an array or the element is null, throw an exception.
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if(!elementToRemove)
            throw new UnityException("Removing a null element is not supported using this method.");

        // Pull all the information from the target of the serializedObject.
        arrayProperty.serializedObject.Update();

        // Go through all the elements in the serializedProperty's array...
        for (int i = 0; i < arrayProperty.arraySize; i++)
        {
            SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);

            // ... until the element matches the parameter...
            if (elementProperty.objectReferenceValue == elementToRemove)
            {
                // ... then remove it.
                arrayProperty.RemoveFromObjectArrayAt (i);
                return;
            }
        }

        throw new UnityException("Element " + elementToRemove.name + "was not found in property " + arrayProperty.name);
    }

    /// <summary>
    /// Returns the object the property is referencing
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static object GetPropertyObject(this SerializedProperty property)
    {
        if (property == null)
            return null;

        var path = property.propertyPath.Replace(".Array.data[", "[");
        object obj = property.serializedObject.targetObject;
        var elements = path.Split('.');

        foreach(var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }

        return obj;
    }

    private static object GetValue(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    private static object GetValue(object source, string name, int index)
    {
        var enumerable = GetValue(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }
}
