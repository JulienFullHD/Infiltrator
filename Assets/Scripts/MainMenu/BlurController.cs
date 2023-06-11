using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurController : MonoBehaviour
{
    public static BlurController Instance;
    private DepthOfField dof;

    private void Start()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        DepthOfField tmp;
        if(GetComponent<Volume>().profile.TryGet<DepthOfField>(out tmp))
        {
            dof = tmp;
        }
        else
        {
            Debug.LogError($"No Depth of Field Component on Volume of {gameObject.name}");
        }
    }

    public void SetClear()
    {
        FocalLength = 1;
    }

    public void SetBlurry()
    {
        FocalLength = 200;
    }

    public float FocalLength
    {
        get
        {
            return dof.focalLength.value;
        }
        set
        {
            dof.focalLength.value = value;
        }
    }
}
