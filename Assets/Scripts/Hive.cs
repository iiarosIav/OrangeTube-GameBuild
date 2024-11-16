using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : InteractiveObject
{
    private float _honeyLevel; // кол-во меда в улье
    [SerializeField] private float _beeLevel; // уровень пчел в улье
    private float _fillTime; // время накопления меда в улье, будет браться из бд, исходя из уровня пчел
    [SerializeField] private float _maxHoneyLevel; // вместимость улья, будет браться из бд
    
    private bool _fillRoutine;
    
    void Start()
    {
        _fillTime = _beeLevel * 2; // заглушка
    }

    void Update()
    {
        if (_honeyLevel == 0 && !_fillRoutine) // если улей пуст и не наполняется, то начинает наполняться
        {
            _fillRoutine = true;
            StartCoroutine(FillRoutine());
        }
    }

    public override void Interact() // взаимодействие с ульем
    {
        if (_honeyLevel == _maxHoneyLevel) // если полон, то опустошаем
        {
            _honeyLevel = 0;
            Player.Instance.GetHoney(_maxHoneyLevel);
            Debug.Log("Devastated");
        }
        else // если нет, то выводим количество меда
        {
            Debug.Log($"Unfilled, honey level is {Convert.ToInt32(_honeyLevel)}");
        }
    }
    
    private IEnumerator FillRoutine() // наполнение улья за время
    {
        for (float t = 0; t <= 1f; t += (Time.deltaTime / _fillTime))
        {
            _honeyLevel = Mathf.Lerp(0, _maxHoneyLevel, t);
            yield return null;
        }
        
        _honeyLevel = _maxHoneyLevel;
        _fillRoutine = false;
    }
}
