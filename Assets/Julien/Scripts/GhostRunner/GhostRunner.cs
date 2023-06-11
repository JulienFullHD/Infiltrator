using UnityEngine;

public class GhostRunner : MonoBehaviour {
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;

    private ReplaySystem _system;

    private void Awake() => _system = new ReplaySystem(this);
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Start Recording");
            _system.StartRun(_recordTarget, _captureEveryNFrames);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Stop Recording");
            _system.FinishRun();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Play Recording");
            _system.StopReplay();
            _system.PlayRecording(RecordingType.Best, Instantiate(_ghostPrefab));
        }
    }
    
}

