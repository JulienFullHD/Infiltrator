using UnityEngine;

public class GhostRunner : MonoBehaviour {
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;
    [SerializeField] private Transform player;

    public ReplaySystem _system;
    [SerializeField]private SetupConfig setupConfig;

    private void Awake() => _system = new ReplaySystem(this);
    private void Start() 
    {
        _system.StartRun(_recordTarget, _captureEveryNFrames);
        //Debug.Log("Start Recording");
        //_system.PlayRecording(RecordingType.Last, Instantiate(_ghostPrefab,player.position,player.rotation));
        PlayDeveloperRun(setupConfig.GhostDataDevRun);  
        
    }
    private void Update() 
    {
        // if(Input.GetKeyDown(KeyCode.O))
        // {
        //     Debug.Log("Start Recording");
        //     _system.StartRun(_recordTarget, _captureEveryNFrames);
        // }
        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     Debug.Log("Stop Recording");
        //     _system.FinishRun();
        // }
        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     PlayDeveloperRun(setupConfig.GhostDataDevRun);  
        // }
    }
    public void StopRun()
    {
        _system.FinishRun();
        Debug.Log("Stop Recording");
    }
    public void StartReplay()
    {
        _system.StopReplay();
        _system.PlayRecording(RecordingType.Last, Instantiate(_ghostPrefab));
    }
    public void PlayDeveloperRun(string _data)
    {
        _system.SetSavedRun(new Recording(_data));

        _system.PlayRecording(RecordingType.Saved, Instantiate(_ghostPrefab,player.position,player.rotation));
    }
    
}

