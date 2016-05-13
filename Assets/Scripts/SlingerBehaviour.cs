using UnityEngine;
using System.Collections;

public class SlingerBehaviour : MonoBehaviour {

    [HideInInspector]
    public Vector3 birdInitPosition;
    public Transform birdWaitPos, slingshotLineRendererLeft, slingshotLineRendererRight;
    public LineRenderer leftLineRenderer, rightLineRenderer;
    [HideInInspector]
    public GameObject birdToThrow;
    private float throwSpeed = 5f;
    [HideInInspector]
    public float timePassed;
    [HideInInspector]
    public VariablesAndStates.SlingshotStates slingShotState;
    public GameManager gameManager;
    

	// Use this for initialization
	void Awake () {
        slingShotState = VariablesAndStates.SlingshotStates.Idle;
        leftLineRenderer.SetPosition(0, slingshotLineRendererLeft.transform.position);
        rightLineRenderer.SetPosition(0, slingshotLineRendererRight.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(slingShotState + this.name);
        switch (slingShotState)
        {
            case VariablesAndStates.SlingshotStates.Idle:
                {
                    if (Input.GetMouseButtonDown(0)) //IF CLICKED ON THEN MOVE IT TO SLINGER(ADD STATE TO DO IT ONCE PER BIRD)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction);
                        if(rayHit.collider != null && rayHit.collider == birdToThrow.GetComponent<CircleCollider2D>())
                        {
                             slingShotState = VariablesAndStates.SlingshotStates.Pulling;
                        }
                    }
                    break;
                }
            case VariablesAndStates.SlingshotStates.Pulling:
                {
                    if (Input.GetMouseButton(0))
                    {
                        Vector3 point = FindObjectOfType<Camera>().ScreenToWorldPoint(Input.mousePosition);
                        point.z = 0f;
                        if(Vector3.Distance(point,birdWaitPos.position) > 0.5f) //MIN TENSION
                        {
                            if (Vector3.Distance(point, birdWaitPos.position) > 15f) // MAX TENSION
                            {
                                var maxTension = (point - birdWaitPos.position).normalized * 15f - birdWaitPos.position;
                                birdToThrow.transform.position = maxTension;
                            }
                            else
                            {
                                leftLineRenderer.SetPosition(1, birdToThrow.transform.position);
                                rightLineRenderer.SetPosition(1, birdToThrow.transform.position);
                                birdToThrow.transform.position = point;
                            }
                        }
                    }
                    else
                    {
                        var distance = Vector3.Distance(birdWaitPos.position, birdToThrow.transform.position);
                        if(distance > 1f)
                        {
                            slingShotState = VariablesAndStates.SlingshotStates.BirdFlying;
                            ThrowBird(distance, birdToThrow, throwSpeed);
                            timePassed = Time.time;
                        }

                        else
                        {
                            // GET NEW BIRD
                        }
                    }
                    break;
                }
        }
	}

    public void ThrowBird(float distance, GameObject throwedBird, float throwSpeed)
    {
        Vector3 gap = birdWaitPos.position - throwedBird.transform.position;
        birdToThrow.GetComponent<BirdBehaviour>().OnThrow();
        gameManager.SlingerThrownBird();
        birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(gap.x, gap.y) * distance / throwSpeed;
    }
}
