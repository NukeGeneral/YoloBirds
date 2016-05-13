using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public CameraBehaviour cameraData;
    private int birdIndex;
    public SlingerBehaviour slingerData;
    private List<GameObject> bricks, birds, pigs;
    [HideInInspector]
    public VariablesAndStates.GameStates gameState;

	void Awake () {
        slingerData = FindObjectOfType<SlingerBehaviour>();
        slingerData.enabled = false;
        slingerData.leftLineRenderer.enabled = false;
        slingerData.rightLineRenderer.enabled = false;
        gameState = VariablesAndStates.GameStates.Start;
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(gameState + this.name);
        switch (gameState)
        {
            case VariablesAndStates.GameStates.Start:
                gameState = VariablesAndStates.GameStates.GetBird;
                GetBird();
                break;
            case VariablesAndStates.GameStates.Playing:
                if(slingerData.slingShotState == VariablesAndStates.SlingshotStates.BirdFlying && ((Time.time - slingerData.timePassed > 10f) || AllObjectsStopped()))
                {
                    slingerData.enabled = false;
                    slingerData.leftLineRenderer.enabled = false;
                    slingerData.rightLineRenderer.enabled = false;
                    ReturnCamera();
                    gameState = VariablesAndStates.GameStates.GetBird;
                }
                break;
            case VariablesAndStates.GameStates.Won:
            case VariablesAndStates.GameStates.Lost:
                if (Input.GetMouseButtonDown(0))
                {
                    Application.LoadLevel(Application.loadedLevelName);
                }
                break;
        }
    }

    bool AllObjectsStopped()
    {
        foreach(var item in pigs.Union(bricks).Union(birds))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < VariablesAndStates.minVelocity)
                return false;
        }
        return true;
    }

    public void GetBird()
    {
        birds[birdIndex].transform.positionTo(1.35f, slingerData.birdWaitPos.position).setOnCompleteHandler((state) =>
        {
            state.complete();
            state.destroy();
            gameState = VariablesAndStates.GameStates.Playing;
            slingerData.enabled = true;
            slingerData.leftLineRenderer.enabled = true;
            slingerData.rightLineRenderer.enabled = true;
            slingerData.birdToThrow = birds[birdIndex];
        });
    }

    private bool AllEnemiesDestroyed()
    {
        return pigs.All(var => var == null);
    }

    public void SlingerThrownBird()
    {
        cameraData.isThereAnyBird = birds[birdIndex].transform;
        cameraData.isFollowingBird = true;
    }

    void ReturnCamera()
    {
        float camMoved = Camera.main.transform.position.x - cameraData.cameraFirstPosition.x;
        if (camMoved == 0.0f)
            camMoved = 0.02f;
        Camera.main.transform.positionTo(camMoved / 10f, cameraData.cameraFirstPosition).setOnCompleteHandler((condition => {
            cameraData.isFollowingBird = false;
            if (AllEnemiesDestroyed())
            {
                gameState = VariablesAndStates.GameStates.Won;
            }

            else if (birdIndex == birds.Count - 1)
            {
                gameState = VariablesAndStates.GameStates.Lost;
            }

            else
            {
                gameState = VariablesAndStates.GameStates.GetBird;
                slingerData.slingShotState = VariablesAndStates.SlingshotStates.Idle;
                birdIndex++;
                GetBird();
            }
        }));
    }
}
