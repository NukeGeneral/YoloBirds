using UnityEngine;
using System.Collections;

public class BirdBehaviour : MonoBehaviour {

    public VariablesAndStates.BirdStates birdState { get; set; }
    private TrailRenderer birdTrailRenderer;
    private Rigidbody2D birdRigidbody;
    private CircleCollider2D birdCollider;
    private AudioSource birdSource;
    public bool getNextBird;

	void Awake () {
        birdTrailRenderer = GetComponent<TrailRenderer>();
        birdRigidbody = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<CircleCollider2D>();
        birdSource = GetComponent<AudioSource>();
        birdTrailRenderer.enabled = false;
        birdCollider.radius = VariablesAndStates.birdColliderBig;
        birdState = VariablesAndStates.BirdStates.Waiting;
    }
	
	void FixedUpdate () {
        if (birdState == VariablesAndStates.BirdStates.Flying && birdRigidbody.velocity.sqrMagnitude <= VariablesAndStates.minVelocity)
            StartCoroutine(DestroyWhenStop(3));
	}

    public void OnThrow()
    {
        birdSource.Play();
        birdTrailRenderer.enabled = true;
        birdCollider.radius = VariablesAndStates.birdColliderNormal;
        birdState = VariablesAndStates.BirdStates.Flying;
        birdRigidbody.isKinematic = false; //TO USE GRAVITY
    }


    IEnumerator DestroyWhenStop(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        getNextBird = true;
    }
}

