using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    private float speed;
    public float _PollenAmount;
    public float MaxPollenAmount;
    private float PollenAmount
    {
        get { return _PollenAmount; }
        set
        {
            _PollenAmount = Mathf.Min(value, MaxPollenAmount);
        }
    }

    public NavMeshAgent _agent;

    public HiveBehaviour ParentHive { private get; set; }
    private float _target_height;

    public GameObject _model;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        speed = _agent.speed;
        SetNewDestination();
        _target_height = Random.Range(0, 3f);
    }

    private void Update()
    {
        if (_agent.remainingDistance < 0.5f)
        {
            SetNewDestination();
            if (Vector3.Distance(transform.position, ParentHive.transform.position) < 0.1f)
            {
                ConvertPollen();
            }
        }
        //ChangeBeeHeight();
    }

    void ConvertPollen()
    {
        ParentHive.HoneyAmount += PollenAmount;
        PollenAmount = 0;
        FreezeMove();
        transform.position = ParentHive.transform.position;
    }

    void FreezeMove()
    {
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;
        Invoke(nameof(UnfreezeMove), 5f);
    }

    void UnfreezeMove()
    {
        _agent.isStopped = false;
    }

    private void Pollination()
    {
        if (PollenAmount >= MaxPollenAmount) return;
        PollenAmount += 0.5f;
    }

    private void ChangeBeeHeight()
    {
        if(!_model) return;
        if (Mathf.Abs(_model.transform.position.y - _target_height) > 0.5f)
        {
            float sign = Mathf.Sign(_target_height - transform.position.y);
            _model.transform.position += new Vector3(0, sign * Time.deltaTime, 0);
        }
        else
        {
            _target_height = Random.Range(0, 3f);
        }
            
    }

    void SetNewDestination()
    {
        if (!ParentHive) return;
        Vector3 _random_dir = Random.insideUnitSphere * 10 + transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_random_dir, out _hit, 10, NavMesh.AllAreas);
        _agent.velocity = _agent.desiredVelocity;
        if (PollenAmount >= MaxPollenAmount) _agent.SetDestination(ParentHive.transform.position);
        else _agent.SetDestination(_hit.position);

        Pollination();
    }
}
