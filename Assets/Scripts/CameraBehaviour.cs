using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    private float cameraMinX = -7.0f;
    private float cameraMaxX = 30.0f;
    [HideInInspector]public Vector3 cameraFirstPosition;
    [HideInInspector]public Transform isThereAnyBird;
    public bool isFollowingBird;
    public SlingerBehaviour slingerData;

    // Use this for initialization
    void Awake () {
        cameraFirstPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (isFollowingBird)
        {
            if (isThereAnyBird != null)
            {
                var birdTransform = isThereAnyBird.position.x;
                transform.position = new Vector3(Mathf.Clamp(birdTransform, cameraMinX, cameraMaxX), cameraFirstPosition.y, cameraFirstPosition.z);
            }
            else
                isFollowingBird = false;
        }
	}
}
