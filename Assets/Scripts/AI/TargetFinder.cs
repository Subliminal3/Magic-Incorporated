using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is a singleton that is palced in scene to define the scene to define some of the default target objects 
/// </summary>
public class TargetFinder : MonoBehaviour
{
    public static TargetFinder Instance;

    public UnitController player;
    public UnitController portal;

    //Optional to create list of enemies and allies when they are spawned
    //public List<UnitController> allies = new();
    //public List<UnitController> enemies = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
