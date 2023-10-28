using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    //private bool showRestartButton = false; // 控制是否显示重启按钮

    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    void OnGUI()
    {
        float width = Screen.width / 6;
        float height = Screen.height / 12;
        FirstController controller = SSDirector.getInstance().currentSceneController as FirstController;

        if (controller.isgameover)
        {
            // 游戏结束信息
            if (controller.iswin)
            {
                GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height), "游戏结束,你赢了");
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height), "游戏结束,你输了");
            }
            // 重启游戏按钮
            if (GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2 + 2 * height, width, height), "重新开始"))
            {
                action.RestartGame();


            }
        }
    }
}