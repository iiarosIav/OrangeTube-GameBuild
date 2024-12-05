using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool isComplete = false;

    [SerializeField] private List<IQuest> _quests;

    private int _index = -1;

    private void Start()
    {
        Run();
    }

    private void Update()
    {
        if (isComplete == false && _index != -1 && _quests.Count > 0) _quests[_index].UpdateQuest();
    }

    public void Run(int index = 0)
    {
        this._index = index;
        _quests[this._index].OnComplete += OnComplete;
        _quests[this._index].RunQuest();
    }

    void OnComplete()
    {
        _quests[this._index].OnComplete -= OnComplete;
        _index++;
        if(this._index < _quests.Count)
        {
            _quests[_index].OnComplete += OnComplete;
            _quests[_index].RunQuest();
        }
        else isComplete = true;
    }
}
