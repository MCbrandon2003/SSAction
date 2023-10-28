using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Scene Controller
/// Usage: host on a gameobject in the scene   
/// responsiablities:
///   acted as a scene manager for scheduling actors.log something ...
///   interact with the director and players
/// </summary>
class Referee
{
	public static bool IsGameOver(RoleModel[]priestModel,RoleModel[]devilModel)
	{
		int leftPriests = 0, leftDevils = 0, rightPriests = 0, rightDevils = 0;

		for (int i = 0; i < 3; i++)
		{
			if (priestModel[i].sign > 0) leftPriests++;
			else rightPriests++;

			if (devilModel[i].sign > 0) leftDevils++;
			else rightDevils++;
		}
		if (rightDevils == 3 && rightPriests == 3)
		{
			Debug.Log("WIN!");
			return true;
		}

		return (leftPriests > 0 && leftPriests < leftDevils) || (rightPriests > 0 && rightPriests < rightDevils);
	}
	public static bool win(RoleModel[] priestModel, RoleModel[] devilModel)
    {
		int leftPriests = 0, leftDevils = 0, rightPriests = 0, rightDevils = 0;

		for (int i = 0; i < 3; i++)
		{
			if (priestModel[i].sign > 0) leftPriests++;
			else rightPriests++;

			if (devilModel[i].sign > 0) leftDevils++;
			else rightDevils++;
		}
		if (rightDevils == 3 && rightPriests == 3)
		{
			return true;
		}

		return false;
	}

}
public class FirstController : MonoBehaviour, ISceneController, IUserAction {
	BoatModel boatModel;
	RoleModel[] priestModel;
	RoleModel[] devilModel;
	public bool isgameover;
	public bool iswin;
	public CCActionManager actionManager { get; set; }
	public GameObject GM;
	// the first scripts
	void Awake()
	{
		isgameover = false;
		iswin = false;
		SSDirector director = SSDirector.getInstance();
		director.setFPS(60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources();
		Camera mainCamera = Camera.main;
		mainCamera.transform.position = new Vector3(-0.1f, 17.1f, -24.2f); // 设置相机的位置
		mainCamera.transform.rotation = Quaternion.Euler(19.85f, 0, 0); // 设置相机的旋转角度
		mainCamera.fieldOfView = 80; // 设置相机的视野
	}

	// loading resources for the first scence
	public void LoadResources () {
		//动作管理
		GM = new GameObject("GM");
		this.actionManager = GM.AddComponent<CCActionManager>();
		//创建船只
		GameObject boatObject = new GameObject("Boat");
		boatModel = boatObject.AddComponent<BoatModel>();
		// 创建两岸
		GameObject leftBank = GameObject.CreatePrimitive(PrimitiveType.Plane);
		leftBank.transform.position = new Vector3(30, 5, 0);
		Renderer leftRenderer = leftBank.GetComponent<Renderer>();
		leftRenderer.material.color = Color.gray; // 设置两岸颜色为深灰色

		GameObject rightBank = GameObject.CreatePrimitive(PrimitiveType.Plane);
		rightBank.transform.position = new Vector3(-30, 5, 0);
		Renderer rightRenderer = rightBank.GetComponent<Renderer>();
		rightRenderer.material.color = Color.gray; // 设置两岸颜色为深灰色

		//创建牧师和魔鬼
		GameObject [] priest = new GameObject[3];
		priestModel = new RoleModel[3];
		for (int i=0; i<3; i++)
        {
			priest[i] = new GameObject("Priest" + i);
			priest[i].transform.position = new Vector3(-(27 + 2 * i), 6.0f, -1.5f);
			priestModel[i] = priest[i].AddComponent<RoleModel>();
			priestModel[i].Initialize(true,ref boatModel);
			priestModel[i].SetPosition(new Vector3(-(27 + 2 * i), 6.0f, -1.5f));

        }

		GameObject[] devil = new GameObject[3];
		devilModel = new RoleModel[3];
		for (int i = 0; i < 3; i++)
		{
			devil[i] = new GameObject("devil" + i);
			devil[i].transform.position = new Vector3(-(27 + 2 * i), 7.5f, 1.5f);
			devilModel[i] = devil[i].AddComponent<RoleModel>();
			devilModel[i].Initialize(false,ref boatModel);
			devilModel[i].SetPosition(new Vector3(-(27 + 2 * i), 7.5f, 1.5f));
		}

		// 创建水池
		GameObject waterPool = GameObject.CreatePrimitive(PrimitiveType.Cube);
		waterPool.transform.position = new Vector3(0, 0, 0);
		waterPool.transform.localScale = new Vector3(40, 0.1f, 10);

		// 添加水材质
		Renderer waterRenderer = waterPool.GetComponent<Renderer>();
		Material waterMaterial = new Material(Shader.Find("Standard"));
		waterMaterial.color = Color.blue; // 设置水的颜色
		waterRenderer.material = waterMaterial;

		// 创建斜坡1
		GameObject slope1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		slope1.transform.position = new Vector3(22.5f, 2.5f, 0);
		slope1.transform.localScale = new Vector3(7, 0.1f, 10);
		slope1.transform.rotation = Quaternion.Euler(0, 0, 45); // 设置斜坡的旋转角度

		// 斜坡1和水池使用相同的材质
		Renderer slope1Renderer = slope1.GetComponent<Renderer>();
		slope1Renderer.material.color = Color.gray;

		// 创建斜坡2
		GameObject slope2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		slope2.transform.position = new Vector3(-22.5f, 2.5f, 0);
		slope2.transform.localScale = new Vector3(7, 0.1f, 10);
		slope2.transform.rotation = Quaternion.Euler(0, 0, -45); // 设置斜坡的旋转角度

		// 斜坡2和水池使用相同的材质
		Renderer slope2Renderer = slope2.GetComponent<Renderer>();
		slope2Renderer.material.color = Color.gray;

		StartCoroutine(CheckGameState());

	}

	private IEnumerator CheckGameState()
	{
		while (true)
		{
			if (Referee.IsGameOver(priestModel, devilModel))
			{
				isgameover = true;
				iswin = Referee.win(priestModel,devilModel);
				Debug.Log("Game Over!");
				yield break;
			}
			yield return new WaitForSeconds(1);
			
		}
	}
	


	

	public void Pause ()
	{
		throw new System.NotImplementedException ();
	}

	public void Resume ()
	{
		throw new System.NotImplementedException ();
	}

	#region IUserAction implementation
	public void GameOver ()
	{
		SSDirector.getInstance ().NextScene ();
	}
	#endregion
	public void RestartGame()
    {
		for (int i = 0; i < 3; i++)
		{
			priestModel[i].SetPosition(new Vector3(-(27 + 2 * i), 6.0f, -1.5f));
			priestModel[i].sign = 1;

		}

		for (int i = 0; i < 3; i++)
		{
			devilModel[i].SetPosition(new Vector3(-(27 + 2 * i), 7.5f, 1.5f));
			devilModel[i].sign = 1;
		}

		boatModel.SetPosition(new Vector3(-17.2f, 0.3f, 0));
		isgameover = false;
		StartCoroutine(CheckGameState());

	}

	// Use this for initialization
	void Start () {
		//give advice first
	}
	
	// Update is called once per frame
	void Update () {
		//give advice first
	}

}
