using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    [SerializeField] List<SingletonBase> _singletons = new List<SingletonBase>();
    private void Awake() => _singletons.ForEach(s => s?.Init());
}
