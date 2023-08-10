using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDoppler : MonoBehaviour
{
    private Transform player;
    public float SpeedOfSound = 343.3f;
    public float DopplerFactor = 1.0f;
    Vector3 emitterLastPosition = Vector3.zero;
    Vector3 listenerLastPosition = Vector3.zero;



    private void Start()
    {
        // get the player object handy for the rest of the script!
        player = GameObject.Find("Player").transform;
    
    }
    // Update is called once per frame
    void FixedUpdate()
{  

        // get velocity of source/emitter manually
        Vector3 emitterSpeed = (emitterLastPosition - transform.position) / Time.fixedDeltaTime;
        emitterLastPosition = transform.position;

        // get velocity of listener/player manually
        Vector3 listenerSpeed = (listenerLastPosition - player.position) / Time.fixedDeltaTime;
        listenerLastPosition = player.position;

        // do doppler calc - see http://i.imgur.com/h5BMRmr.png or http://redmine.spatdif.org/projects/spatdif/wiki/Doppler_Extension (OpenAL's implementation of doppler)
        var distance = (player.position - transform.position); // source to listener vector
        var listenerRelativeSpeed = Vector3.Dot(distance, listenerSpeed) / distance.magnitude;
        var emitterRelativeSpeed = Vector3.Dot(distance, emitterSpeed) / distance.magnitude;
        listenerRelativeSpeed = Mathf.Min(listenerRelativeSpeed, (SpeedOfSound / DopplerFactor));
        emitterRelativeSpeed = Mathf.Min(emitterRelativeSpeed, (SpeedOfSound / DopplerFactor));
        var dopplerPitch = (SpeedOfSound + (listenerRelativeSpeed * DopplerFactor)) / (SpeedOfSound + (emitterRelativeSpeed * DopplerFactor));
        // pass the dopplerPitch through to an RTPC in Wwise (or do whatever you want with the value!)
        AkSoundEngine.SetRTPCValue("DopplerParam", dopplerPitch); // "DopplerParam" is the name of the RTPC in the Wwise project :)
                                                                  // uncomment the line below to see the numbers that are being passed through so you can adjust your RTPC values if necessary.
                                                                  //Debug.Log (dopplerPitch);

    }
}