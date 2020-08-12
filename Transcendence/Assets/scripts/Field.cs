using UnityEngine;
using System.Collections;
using System;

public class Field : MonoBehaviour {

    public int side;

    // Location of the object
    public Location location;

	// Set the enum for this field based on its name
	void Start ()
    {
	    foreach (Location location in Enum.GetValues(typeof(Location)))
        {
            if (location.ToString().Equals(this.gameObject.name))
            {
                this.location = location;
            }
        }
	}

    // Return the location of this object
    public Location getLocation()
    {
        return this.location;
    }
}
