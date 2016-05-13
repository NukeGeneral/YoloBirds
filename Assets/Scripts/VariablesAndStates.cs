using UnityEngine;
using System.Collections;

public static class VariablesAndStates {
    
    public enum SlingshotStates { Idle,Pulling,BirdFlying}
    public enum GameStates { Start,GetBird,Playing,Won,Lost}
    public enum BirdStates { Waiting,Flying}

    public static float minVelocity = .01f;
    public static float birdColliderNormal = GameObject.FindWithTag("Bird").GetComponent<CircleCollider2D>().radius;
    public static float birdColliderBig = birdColliderNormal + .1f;
}
