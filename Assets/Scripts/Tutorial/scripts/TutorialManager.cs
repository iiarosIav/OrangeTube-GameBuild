using System.Collections;
using System.Collections.Generic;
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
        if (isComplete) return;
        _index = index;
        _quests[_index].OnComplete += OnComplete;
        _quests[_index].RunQuest();
    }

    void OnComplete()
    {
        _quests[_index].OnComplete -= OnComplete;
        _index++;
        if(_index < _quests.Count)
        {
            _quests[_index].OnComplete += OnComplete;
            _quests[_index].RunQuest();
        }
        else isComplete = true;
    }
    
    public void Complete() => isComplete = true;
    
    public bool GetIsComplete() => isComplete;

    public int GetTutorialIndex() => _index;

    public void FinishFirstQuest() => _quests[0].UpdateQuest();

}
