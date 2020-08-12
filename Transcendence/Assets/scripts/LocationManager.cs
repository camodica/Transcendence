using UnityEngine;
using System.Collections.Generic;

public class LocationManager : MonoBehaviour {

    private Dictionary<Location, GameObject> locationMap = new Dictionary<Location, GameObject>();

	/// <summary>
    /// Add all of the location's game objects to the dictionary. :)
    /// </summary>
	void Start ()
    {

        this.locationMap.Add(Location.BottomCenter1, GameObject.Find("BottomCenter1"));
        this.locationMap.Add(Location.BottomCenter2, GameObject.Find("BottomCenter2"));
        this.locationMap.Add(Location.BottomCenter3, GameObject.Find("BottomCenter3"));

        this.locationMap.Add(Location.BottomLeft1, GameObject.Find("BottomLeft1"));
        this.locationMap.Add(Location.BottomLeft2, GameObject.Find("BottomLeft2"));
        this.locationMap.Add(Location.BottomLeft3, GameObject.Find("BottomLeft3"));

        this.locationMap.Add(Location.BottomRight1, GameObject.Find("BottomRight1"));
        this.locationMap.Add(Location.BottomRight2, GameObject.Find("BottomRight2"));
        this.locationMap.Add(Location.BottomRight3, GameObject.Find("BottomRight3"));

        this.locationMap.Add(Location.TopCenter1, GameObject.Find("TopCenter1"));
        this.locationMap.Add(Location.TopCenter2, GameObject.Find("TopCenter2"));
        this.locationMap.Add(Location.TopCenter3, GameObject.Find("TopCenter3"));

        this.locationMap.Add(Location.TopLeft1, GameObject.Find("TopLeft1"));
        this.locationMap.Add(Location.TopLeft2, GameObject.Find("TopLeft2"));
        this.locationMap.Add(Location.TopLeft3, GameObject.Find("TopLeft3"));

        this.locationMap.Add(Location.TopRight1, GameObject.Find("TopRight1"));
        this.locationMap.Add(Location.TopRight2, GameObject.Find("TopRight2"));
        this.locationMap.Add(Location.TopRight3, GameObject.Find("TopRight3"));

        this.locationMap.Add(Location.Player1Hand, GameObject.Find("Player1Hand"));
        this.locationMap.Add(Location.Player2Hand, GameObject.Find("Player2Hand"));
    }
	
    public GameObject GetLocationGameObject(Location location)
    {
        return this.locationMap[location];
    }

    public Transform GetLocationTransform(Location location)
    {
        return this.locationMap[location].transform;
    }
}
