using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeSelector : MonoBehaviour
{

    public Attack.Element startingSchool;
    public Affinity affinity;

    public Attack[] VengeanceFireRoute, VengeanceIceRoute, VengeanceThunderRoute, VengeanceArcaneRoute;
    public Attack[] GuileFireRoute, GuileIceRoute, GuileThunderRoute, GuileArcaneRoute;
    public Attack[] RemissionFireRoute, RemissionIceRoute, RemissionThunderRoute, RemissionArcaneRoute;

    public Attack[] VengeanceUlt, GuileUlt, RemissionUlt;

    public Fighter fighter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
