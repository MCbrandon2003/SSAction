using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {

	private Dictionary <int, SSAction> actions = new Dictionary <int, SSAction> ();
	private List <SSAction> waitingAdd = new List<SSAction> (); 
	private List<int> waitingDelete = new List<int> ();

	// Update is called once per frame
	protected void Update () {
		;
	}

	public void RunAction(MonoBehaviour model, SSAction action, ISSActionCallback manager) {
	
		if (model is BoatModel)
		{ action.gameobject = ((BoatModel)model).boat;
			Debug.Log("boat action start");
		}
		else if(model is RoleModel)
        {	Debug.Log("role action start");
			action.gameobject = ((RoleModel)model).role;
        }
		action.transform = action.gameobject.transform;
		action.callback = manager;
		action.model = model;
		action.Start ();
	}


	// Use this for initialization
	protected void Start () {
	}
}
