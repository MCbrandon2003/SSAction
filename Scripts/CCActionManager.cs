using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback {
	
	private FirstController sceneController;
	private List<CCMove> ccmoves;
	protected new void Start() {
		sceneController = (FirstController)SSDirector.getInstance ().currentSceneController;
		sceneController.actionManager = this;
		ccmoves = new List<CCMove>();
	}

	// Update is called once per frame
	protected new void Update ()
	{
		for (int i = ccmoves.Count - 1; i >= 0; i--)
		{	
			ccmoves[i].Update();  
		}
	}

	public void Move(MonoBehaviour model)
	{
		for (int i = 0; i < ccmoves.Count; i++)
		{
			if (model is BoatModel && ccmoves[i].transform == ((BoatModel)model).boat.transform)
			{
				Debug.Log("repeat");
				ccmoves[i].move_boat((BoatModel)model);
				this.RunAction(model, ccmoves[i], this);
				return;
			}
			else if (model is RoleModel && ccmoves[i].transform == ((RoleModel)model).role.transform)
            {
				Debug.Log("repeat");
				ccmoves[i].move_role((RoleModel)model);
				return;
			}
		}
		CCMove ccMove = ScriptableObject.CreateInstance<CCMove>();
		ccmoves.Add(ccMove);

		if (model is BoatModel)
		{
			ccMove.move_boat((BoatModel)model);
			this.RunAction(model, ccMove, this);
		}
		else
		{
			ccMove.move_role((RoleModel)model);
			this.RunAction(model, ccMove, this);
		}
	}
	#region ISSActionCallback implementation
	public void SSActionEvent (SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null)
	{
		Debug.Log("Move successfully conducted");
		ccmoves.Remove((CCMove)source);
		ScriptableObject.Destroy(source);
		Debug.Log("CCMove destroyed");
	}
	#endregion
}

