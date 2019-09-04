using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSimpleMove : MonoBehaviour {

    public float speed = 3;
    public float rotateSpeed = 3;
    CharacterController cc;

	// Use this for initialization
	void Awake () {
        cc = GetComponent<CharacterController>();
	}

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 dir = transform.forward;
        cc.SimpleMove(dir * speed * Input.GetAxis("Vertical"));
	}
}
