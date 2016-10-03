using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    static SceneController _instance;

    [SerializeField]
    int _operations;
    int _movepoints=0;

    float _t;

    public Image _radialLoad;

    [SerializeField]
    float _loadingTime;

    [SerializeField]
    GameObject _sphere;
    Transform _sphereTrans;

    bool isEntered;
    bool _sphereMove;

    [HideInInspector]
    public List<Vector3> _points = new List<Vector3>();//point for move sphere

    CilynderScript[] _cylinders;//Cilynders in scene

    public static SceneController Instance  {
        get  {
            if (_instance==null) {
                _instance = FindObjectOfType<SceneController>();
               
            }
            return _instance;
        }

        set {
            _instance = value;
        }
    }

   

    public float LoadingTime  {
        get {
            return _loadingTime;
        }

        set  {
            _loadingTime = value;
        }
    }

    public int Operations   {
        get  {
            return _operations;
        }

        set {
            _operations = value;
        }
    }

    // Use this for initialization

    void Awake() {

        new Colors(_operations);
    }
    void Start () {
        _sphereTrans = _sphere.GetComponent<Transform>();
        _cylinders = FindObjectsOfType<CilynderScript>();
	//_radialLoad.fil
	}
	
	// Update is called once per frame
	void Update () {
        if (_sphereMove) {
            MoveSphere();
        }
	}

    public void StartSphereMove() {
      var sphere=Instantiate(_sphere,_points[0],Quaternion.identity) as GameObject;
        _sphereTrans = sphere.GetComponent<Transform>();
        _sphereMove = true;
    }

    void MoveSphere() {

        _t += Time.deltaTime *0.5f;
        _t = Mathf.Clamp01(_t);
     

        _sphereTrans.position = QuasiBezietCurve(_points[_movepoints], Vector3.zero, _points[_movepoints+1],_t);

       // Debug.Log((_sphereTrans.position - _points[_movepoints + 1]).magnitude);
        if ((_sphereTrans.position - _points[_movepoints+1]).magnitude <= 0.1f) {
            _movepoints++;
            _t = 0;
            Debug.Log("Next Point");
        }

        if ((_sphereTrans.position-_points[_points.Count-1]).magnitude<=0.1f) {
            _sphereMove = false;
            ResetAll();
            Debug.Log("Reset All");
        }
    }

    //Vector3 Beziet(Vector3 p0,Vector3 p1,Vector3 p2,float t) {

    //    Vector3 q0 = Vector3.Lerp(p0,p1,t);
    //    Vector3 q1 = Vector3.Lerp(p1, p2, t);

    //    return Vector3.Lerp(q0,q1,t);
    //}
    Vector3 QuasiBezietCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t) {//Non-BezietCurve
        p1 = p1 - (p0 + p2)*0.5f;
        float invT = 1 - t;
        return (invT * invT * p0) + (2 * invT * t * p1) + (t * t * p2);
    }

    void ResetAll() {
        _movepoints = 0;
        _t = 0;
        Destroy(_sphereTrans.gameObject);
        _points.Clear();
        CilynderScript.Steps = 0;//return stepcount to default
        foreach (var cil in _cylinders) {
            cil.DefaultState();
        }
    }

}
