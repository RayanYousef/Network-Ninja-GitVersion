using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExampleArmy : MonoBehaviour {
    private FormationBase _formation;

    public FormationBase Formation {
        get {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private GameObject _unitPrefab_Melee;
    [SerializeField] private GameObject _unitPrefab_Ranged;
    [SerializeField] private GameObject _unitPrefab_Tank;
    [SerializeField] private float _unitSpeed = 2;

    float meleeChance;
    float rangedChance;
    float tankChance;
    float chance;

    float min, mid, max;

    private readonly List<GameObject> _spawnedUnits = new List<GameObject>();
    private List<Vector3> _points = new List<Vector3>();
    private Transform _parent;

    private void Awake() {
        _parent = new GameObject("Unit Parent").transform;
    }

    private void Update() {
        SetFormation();
    }

    public void SetFormation() {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > _spawnedUnits.Count)
        {
            var remainingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < _spawnedUnits.Count) {
            Kill(_spawnedUnits.Count - _points.Count);
        }

        for (var i = 0; i < _spawnedUnits.Count; i++) {
            _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
        }
    }

    public void SetPrefabsTypes(Soldiers soldiersType)
    {
        switch (soldiersType)
        {
            case Soldiers.Melee:
                meleeChance = 1;
                break;
            case Soldiers.Ranged:
                rangedChance = 1;
                break;
            case Soldiers.MeleeRanged:
                meleeChance = 0.6f;
                rangedChance = 0.4f;
                break;
            case Soldiers.MeleeRangedTank:
                meleeChance = 0.5f;
                rangedChance = 0.3f;
                tankChance = 0.2f;
                break;
        }
    }

    private void Spawn(IEnumerable<Vector3> points) {
        foreach (var pos in points)
        {
            chance = Random.Range(0f, 1f);
            min = (meleeChance < rangedChance) ? (meleeChance < tankChance ? meleeChance : tankChance) : (rangedChance < tankChance ? rangedChance : tankChance);
            max = (meleeChance > rangedChance) ? (meleeChance > tankChance ? meleeChance : tankChance) : (rangedChance > tankChance ? rangedChance : tankChance);
            mid = (meleeChance + rangedChance + tankChance) - min - max;

            GameObject unit;
            if (chance < min)
            {
                unit = Instantiate(_unitPrefab_Tank, transform.position + pos, Quaternion.identity, _parent);
            }
            else if (chance < mid)
            {
                unit = Instantiate(_unitPrefab_Ranged, transform.position + pos, Quaternion.identity, _parent);
            }
            else
            {
                unit  = Instantiate(_unitPrefab_Melee, transform.position + pos, Quaternion.identity, _parent);
            }

            if (Random.value <= 0.4f)
            {
                unit.GetComponent<Animator>().SetBool("isSecondIdlePose", true);
            }
            _spawnedUnits.Add(unit);
        }
    }

    private void Kill(int num) {
        for (var i = 0; i < num; i++) {
            var unit = _spawnedUnits.Last();
            _spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }
}