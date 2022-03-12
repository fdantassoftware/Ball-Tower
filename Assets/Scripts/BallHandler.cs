using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour {
    // Start is called before the first frame update

    private Camera mainCamera;
    private Rigidbody2D ballRb;
    private SpringJoint2D ballJoint;
    [SerializeField] private float detachDelay;
    [SerializeField] GameObject theBall;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float SpawnDelay;

    
     void Awake() {
        mainCamera = Camera.main;
    }
    void Start() {
        SpawnBall();
    }

    private Vector3 touchWorldPos {
        get {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
            return worldPos;
        }
    }

    private bool isTouching {
        get {
            return Touchscreen.current.primaryTouch.press.isPressed;
        }
    }
    private bool isDragging;    
    

    // Update is called once per frame
    void Update() {

        if (ballRb == null) {return;}
 
        if (!isTouching) {
            if (isDragging) {
                LaunchBall();
            }
            isDragging = false;
            return;
        }
        isDragging = true;
        ballRb.isKinematic = true;
        ballRb.position = touchWorldPos;
    }

    private void SpawnBall() {
        GameObject ball = Instantiate(theBall, pivot.position, Quaternion.identity);
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballJoint = ball.GetComponent<SpringJoint2D>();
        ballJoint.connectedBody = pivot;
    }

    private void LaunchBall() {
        ballRb.isKinematic = false;
        ballRb = null; 
        Invoke(nameof(DetachBall), detachDelay);
       
    }

    private void DetachBall() {
        ballJoint.enabled = false;
        ballJoint = null;
        Invoke(nameof(SpawnBall), SpawnDelay);
    }
}
