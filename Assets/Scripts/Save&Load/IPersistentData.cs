using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistentData<T> where T: MonoBehaviour
{
    void Initialize(T persistendObject);
}
