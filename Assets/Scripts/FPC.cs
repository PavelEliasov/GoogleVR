using UnityEngine;
using System.Collections;

public class FPC : MonoBehaviour {

    GvrHead _head;

    Transform _cameraTrans;
    CharacterController _characterController;
    bool _move;
	// Use this for initialization
	void Start () {
        //_head = FindObjectOfType<StereoController>().GetComponent<GvrHead>();
        _head = Camera.main.GetComponent<GvrHead>();

        Debug.Log(_head);
        _head.trackPosition = true;
        _head.target = this.gameObject.transform;

        _cameraTrans = Camera.main.transform;
        _characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            _move = !_move;
        }

        if (_move) {
            _characterController.SimpleMove((_cameraTrans.forward - new Vector3(0, _cameraTrans.forward.y, 0)).normalized);

           // Debug.Log(_cameraTrans.forward - new Vector3(0, _cameraTrans.forward.y, 0));
        }
	}
}
