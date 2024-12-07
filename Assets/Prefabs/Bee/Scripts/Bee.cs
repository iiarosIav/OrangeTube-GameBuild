using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    public float MaxPollenAmount;

    [SerializeField]private int _level;
    [SerializeField]private float _speed;
    private float _pollenAmount;
    private float _collectAtTime;

    public float PollenAmount
    {
        get { return _pollenAmount; }
        set
        {
            _pollenAmount = Mathf.Min(value, MaxPollenAmount);
        }
    }
    public int Level
    {
        get{ return _level; }
        set
        {
            if (value >= 1) _level = value;
            else _level = 1;
            ChangeStats(_level);
        }
    }

    public void SetSpeed(float speed) => _speed = speed;

    private void ChangeStats(int _level)
    {
        _speed = Mathf.Pow(_level, 1.25f);
        if(_agent) _agent.speed = _speed;
        MaxPollenAmount = _level;
        _collectAtTime = MaxPollenAmount / 5;
    }

    private NavMeshAgent _agent;

    public HiveBehaviour ParentHive { private get; set; }
    private float _target_height;

    public GameObject _model;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _speed = _agent.speed;
        SetNewDestination();
        _target_height = Random.Range(0, 3f);
        Level = 4;
    }

    private void Update()
    {
        _agent.speed = _speed;
        try
        {
            if (_agent.remainingDistance < 0.1f)
            {
                SetNewDestination();
                if (Vector3.Distance(_agent.transform.position, ParentHive.transform.position) < 0.5f)
                {
                    ConvertPollen();
                }
            }
        } catch { }
    }

    void ConvertPollen()
    {
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;
        Invoke(nameof(UnfreezeMove), 5f);
    }

    void UnfreezeMove()
    {
        _agent.isStopped = false;
        ParentHive.HoneyCapacity += PollenAmount;
        PollenAmount = 0;
    }

    private void Pollination()
    {
        if (PollenAmount >= MaxPollenAmount) return;
        PollenAmount += _collectAtTime;
    }

    void SetNewDestination()
    {
        if (!ParentHive) return;

        Vector3 _random_dir = Random.insideUnitSphere * 10 + transform.position;
        NavMeshHit _hit;

        NavMesh.SamplePosition(_random_dir, out _hit, 10, NavMesh.AllAreas);

        if (PollenAmount >= MaxPollenAmount) _agent.SetDestination(ParentHive.transform.position);
        else _agent.SetDestination(_hit.position);

        Pollination();
    }
}
