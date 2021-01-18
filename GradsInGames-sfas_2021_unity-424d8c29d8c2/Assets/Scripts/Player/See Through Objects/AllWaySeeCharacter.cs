using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWaySeeCharacter : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public GameObject SphereIntercent;

    public float MaxScale, MinScale;
    public float speedOfScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Linecast(Camera.transform.position, Player.transform.position, out hit))
        {
            //- Scale up and Down The Sphere Radius -//
            if (hit.transform.gameObject != SphereIntercent && hit.transform.gameObject != Player && hit.transform.gameObject != Camera)
                SphereIntercent.transform.localScale += new Vector3(Time.deltaTime * speedOfScale, Time.deltaTime * speedOfScale, Time.deltaTime * speedOfScale);
            else
                SphereIntercent.transform.localScale -= new Vector3(Time.deltaTime * speedOfScale, Time.deltaTime * speedOfScale, Time.deltaTime * speedOfScale);

            //- Clamp Spheres Radius -//
            if (SphereIntercent.transform.localScale.x > MaxScale)
                SphereIntercent.transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
            else if (SphereIntercent.transform.localScale.x < MinScale)
                SphereIntercent.transform.localScale = new Vector3(MinScale, MinScale, MinScale);
        }
    }
}
