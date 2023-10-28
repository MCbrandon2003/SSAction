using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel : MonoBehaviour
{
    public GameObject role;
    Click click;
    public bool aboard;
    bool is_priest;
    public bool moving;
    public float speed;
    public BoatModel boatModel;
    public Vector3 destination_off;
    public int sign;
    // Start is called before the first frame update
    public void Initialize(bool Is_priest, ref BoatModel boatmodel)
    {
        this.is_priest = Is_priest;
        if (Is_priest == true)
        {
            role = Instantiate<GameObject>(
                 Resources.Load<GameObject>("prefabs/priest"),
                     Vector3.zero, Quaternion.identity);
        }
        else
        {
            role = Instantiate<GameObject>(
                 Resources.Load<GameObject>("prefabs/devil"),
                     Vector3.zero, Quaternion.identity);

        }
        this.boatModel = boatmodel;
        sign = 1;

    }
    
    void Start()
    {   

        aboard = false;
        click = role.AddComponent(typeof(Click)) as Click;
        click.SetRole(this);
        moving = false;
        speed = 10.0f;
        sign = 1;
    }

    public void start_moving()
    {
        moving = true;
        Debug.Log("start moving");
    }
    public void SetPosition(Vector3 v)
    {
        role.transform.position = v;

    }
    public Vector3 GetPosition()
    {
        return role.transform.position;
    }
    public void board()
    {
        boatModel.board(this);
        aboard = true;

    }
    public void off_board()
    {
        boatModel.off_board(this);
        Debug.Log("off_board");
        Debug.Log("vacant"+boatModel.vacant[0]+" and "+boatModel.vacant[1]);

    }
    
    public bool Is_priest()
    {
        return this.is_priest;

    }
    // Update is called once per frame
    void Update()
    {
        ;
    }
        

    
}
