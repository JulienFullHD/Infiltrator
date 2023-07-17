using UnityEngine;
using System.Collections;
using SharpConfig;
using System.IO;

public class SetupConfig : MonoBehaviour
{
	public string GhostDataDevRun;  
	public Configuration cfg = new Configuration();
	[SerializeField]private GhostRunner ghostRunner;

	void Awake()
	{
		if (!File.Exists ("config.cfg"))
		{
			Debug.Log ("Setting up a default config since no file was found!");
			SetupCleanCFG ();
			SaveConfig ();
		}
		//cfg["GhostData"]["Own"].StringValue = "loldubistsoeinsFisch";
		//SaveConfig ();
		// Load the configuration.
		cfg = Configuration.LoadFromFile("config.cfg");
		GhostDataDevRun = cfg["GhostData"]["Developer"].StringValue;
	}

	public void SaveConfig()
	{
		Debug.Log ("Saving Client config...");

		// Save the configuration.
		cfg.SaveToFile ("config.cfg");
	}

	private void SetupCleanCFG()
	{
		cfg["GhostData"]["Developer"].StringValue = "";
        cfg["GhostData"]["Friend"].StringValue = "";
		cfg["GhostData"]["Own"].StringValue = "";
	}
    public void SaveRun(string _runData)
    {
        cfg["GhostData"]["Own"].StringValue = _runData;
        GhostDataDevRun = _runData;
        SaveConfig ();
    }
}