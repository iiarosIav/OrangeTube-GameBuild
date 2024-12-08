using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool isComplete = false;

    [SerializeField] private List<IQuest> _quests;

    private int _index = -1;

    public void Starter()
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
        
        Progress progress = Progress.Instance;
        
        string comment = $"Игрок {progress.GetUsername()} завершил квест номер {_index++}";
        Task.Run(() => progress.Save(comment));
        
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

    public void FinishFirstQuest() => _quests[0].CompleteQuest();

}
