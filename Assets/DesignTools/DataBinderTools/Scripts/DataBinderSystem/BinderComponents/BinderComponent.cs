using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BinderComponent : MonoBehaviour
{
    public abstract IDataBindable[] GetAllBinders();
}
