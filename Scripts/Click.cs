using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    IUserAction action;
    RoleModel role = null;
    BoatModel boat = null;
    private CCActionManager manager;
    public void SetRole(RoleModel role)
    {
        this.role = role;
        Debug.Log("Click:"+this.role.Is_priest());
    }
    public void SetBoat(BoatModel boat)
    {
        this.boat = boat;
    }
    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
        manager = ((FirstController)SSDirector.getInstance().currentSceneController).actionManager;
        Debug.Log("manager set :" + (manager == null));
        if (action == null)
        {
            Debug.Log("Failed to get IUserAction from current scene controller");
        }
    }
    void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (boat == null && role == null) return;//被点击的对象为空时返回
        if (boat != null)
        {//点击的对象为船，触发船进行移动
            Debug.Log("Action moving boat");
            this.manager.Move(boat);
        }
        else if (role != null)
        {//点击的角色为角色，触发角色进行移动
            Debug.Log("moving"+this.role.Is_priest());
            this.manager.Move(role);
        }
    }
}
