using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CCMove : SSAction
{
	public Vector3 target;
	public float speed;
	public bool over;
	public static CCMove GetSSAction(Vector3 target, float speed){
		CCMove action = ScriptableObject.CreateInstance<CCMove> ();
		action.target = target;
		action.speed = speed;
		return action;
	}
	public void move_boat(BoatModel boatModel)
    {	
		if (boatModel.moving || boatModel.roles.Count == 0)
		{
			Debug.Log("cannot move the boat");
			return;
		}
		boatModel.moving = true;
		for (int i = 0; i < boatModel.roles.Count; i++)
		{
			boatModel.roles[i].start_moving();

		}
		Debug.Log("boatModel.moving");
	}
	public void move_role(RoleModel roleModel)
    {
		if (!roleModel.aboard)
		{
			Debug.Log("boarding");

			if (Math.Sign(roleModel.GetPosition().x) != Math.Sign(roleModel.boatModel.GetPosition().x))
			{
				return;
			}
			roleModel.board();
		}
		else
		{
			roleModel.off_board();
		}
	}
	public override void Update ()
	{	 if (this.model is BoatModel)
		{	
			//Debug.Log("model is BoatModel");
		 BoatModel boatModel = (BoatModel)model;
			speed = boatModel.speed;
			if (boatModel.moving == false)
			{
				Debug.Log("boatModel.moving == false");
				return;
			}
			if (boatModel.start2end == true)
			{
				if (Vector3.Distance(boatModel.GetPosition(), boatModel.end) < 0.2f)
				{
					
					boatModel.moving = false;
					boatModel.SetPosition(boatModel.end);
					boatModel.start2end = false;
					this.over = true;
					this.callback.SSActionEvent(this);
					return;
				}
				//Debug.Log("Boat moving right. "+ this.transform.position);
				this.transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;

			}

			else
			{
				if (Vector3.Distance(boatModel.GetPosition(), boatModel.start) < 0.2f)
				{
					Debug.Log("SetPosition start!");
					boatModel.moving = false;
					boatModel.SetPosition(boatModel.start);
					boatModel.start2end = true;
					Debug.Log("boat arrived");
					this.over = true;
					this.callback.SSActionEvent(this);
					return;
				}
				this.transform.position -= new Vector3(1, 0, 0) * speed * Time.deltaTime;
			}
		}


		if (this.model is RoleModel)
			{
				RoleModel role = (RoleModel)model;
				if (role.moving == false)
				{
					return;
				}
				else if (role.boatModel.moving == false)
				{				
					role.moving = false;
					this.transform.position = role.destination_off;
					role.off_board();
					role.sign *= -1;
					this.callback.SSActionEvent(this);
					over = true;
					return;
				}
				else
				{
					float speed = ((RoleModel)model).speed;
					int sign = ((RoleModel)model).sign;
					this.transform.position += new Vector3(1, 0, 0) * (speed * sign) * Time.deltaTime;
				}

			}

		


	}

	public override void Start () {
		over = false;
	}
    private void OnDestroy()
    {

		Debug.Log("ccmove destroyed");
    }
}

