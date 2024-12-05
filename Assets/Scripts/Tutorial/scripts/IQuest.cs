using System;
using UnityEngine;

[Serializable]
public abstract class IQuest : MonoBehaviour
{
    public abstract event Action OnComplete;

    public abstract void RunQuest();
    public abstract void UpdateQuest();
}
