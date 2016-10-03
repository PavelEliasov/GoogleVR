using UnityEngine;
using System.Collections;
using System;

public class CilynderScript : MonoBehaviour,IGvrGazeResponder{
    MeshRenderer _meshRend;
    bool isEntered;
    static bool isRecieveCasts=true;
    static int _steps;

    int stepNumber;

    Vector3 _spherePosition;

    Color _defaultColor;
    public static int Steps {
        get {
            return _steps;
        }

        set {
            _steps = value;
        }
    }

    // Use this for initialization
    void Start () {
        _spherePosition = transform.position + Vector3.up * transform.localScale.y;//Top position of cilynder
        _meshRend = GetComponent<MeshRenderer>();
        _defaultColor = _meshRend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnGazeEnter() {
        if (isRecieveCasts) {
            PointerEnter();
        }
          
    }

    public void OnGazeExit() {
        if (isRecieveCasts) {
            PointerExit();
        }
     
        //throw new NotImplementedException();
    }

    public void OnGazeTrigger() {
        // throw new NotImplementedException();
    }

    public void PointerEnter() {
        isEntered = true;
        StartCoroutine(RadialLoad());
        Debug.Log("Enter");
    }
    public void PointerExit() {
        isEntered = false;
        StopCoroutine(RadialLoad());
        SceneController.Instance._radialLoad.fillAmount = 0;
        Debug.Log("Exit");
    }

    IEnumerator RadialLoad() {
        // yield return null;
        SceneController.Instance._radialLoad.fillAmount = 0;
        float time = 0;
        while (time <= SceneController.Instance.LoadingTime && isEntered) {
            time += 0.1f;
            SceneController.Instance._radialLoad.fillAmount +=0.1f/SceneController.Instance.LoadingTime;
            yield return new WaitForSeconds(0.1f);
        }
        if (isEntered && _meshRend.material.color != Colors.Instance[stepNumber]) {//Check Cylinder state for paint
            stepNumber = _steps;//check step of paint (A,B,C...)
            SceneController.Instance._points.Add(_spherePosition);//add coordinate-points to global List for sphere movement
            _meshRend.material.color = Colors.Instance[_steps];//paint cilynder by indexer value
            _steps++;//go to next step of paint
            CheckState();
            Debug.Log("EndLoading");
        }

        SceneController.Instance._radialLoad.fillAmount = 0;
        yield return null;

    }


    void CheckState() {
        if (_steps>=SceneController.Instance.Operations) {
            isRecieveCasts = false;//stop recieve raycast for all, when the last cilynder was painted
            StartCoroutine(StartSphere());
        
            Debug.Log("Stop Recieve RayCast");
        }
    }

    IEnumerator StartSphere() {//Delay before instantiate
        yield return new WaitForSeconds(1f);
        SceneController.Instance.StartSphereMove();
    }
  public void DefaultState() {
        isRecieveCasts = true;
        _meshRend.material.color = _defaultColor;
        stepNumber = 0;
        // Debug.Log(_defaultColor);

    }
}
