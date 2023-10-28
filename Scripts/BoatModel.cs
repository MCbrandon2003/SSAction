using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoatModel : MonoBehaviour
{
    public GameObject boat;
    Click click;
    public Vector3 start;
    public Vector3 end;
    public float speed;
    public bool start2end;
    public bool moving;
    Vector3[] vacancy_for_priest;
    Vector3[] vacancy_for_devil;
    public bool[] vacant;
    public List<RoleModel>roles;
    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boat.transform.position = new Vector3(-17.2f, 0.3f, 0);
        boat.transform.localScale = new Vector3(5, 0.3f, 5);
        Renderer boatRenderer = boat.GetComponent<Renderer>();
        boatRenderer.material.color = Color.white;

        roles = new List<RoleModel>();

        start = new Vector3(-17.2f, 0.3f, 0);
        end = new Vector3(17.2f, 0.3f, 0);

        click = boat.AddComponent(typeof(Click)) as Click;
        click.SetBoat(this);
        speed = 10.0f;

        start2end = true;
        moving = false;

        vacancy_for_priest = new Vector3[2];
        vacancy_for_devil = new Vector3[2];

        vacancy_for_priest[0] = new Vector3(-18.2f, 1, 1.5f);
        vacancy_for_priest[1] = new Vector3(-18.2f, 1, -1.5f);

        vacancy_for_devil[0] = new Vector3(-18.2f, 3.0f, 1.5f);
        vacancy_for_devil[1] = new Vector3(-18.2f, 3.0f, -1.5f);

        vacant = new bool[2];
        vacant[0] = true; vacant[1] = true;
    }
    public void off_board(RoleModel roleModel)
    {
        for(int i=0;i<2;i++)
        {
            if(roleModel == roles[i])
            {
                roles.RemoveAt(i);
                if (vacant[i] == true)
                { vacant[1-i] = true;
                }
                else
                {
                    vacant[i] = true;
                }
                Vector3 pos = roleModel.transform.position;
                pos.x = Math.Sign(roleModel.GetPosition().x) * Math.Abs(pos.x);
                roleModel.SetPosition(pos);
                roleModel.aboard = false;
                break;
            }

        }
    }
    public void SetPosition(Vector3 v)
    {
        boat.transform.position = v;
    }
    public Vector3 GetPosition()
    {
        return boat.transform.position;
    }
    public void board(RoleModel roleModel)
    {if (roles.Count >= 2)
        {
            return;
        }
     else
        {
            roles.Add(roleModel);          
            roleModel.destination_off = Vector3.Scale(roleModel.GetPosition(), new Vector3(-1, 1, 1));
       
        }
       for(int i=0;i<2;i++)
        {
            if (vacant[i] == true)
            {if (roleModel.Is_priest() == true)
                {
                    Debug.Log(vacancy_for_devil[i]);
                    vacant[i] = false;
                    if (Vector3.Distance(boat.transform.position, start) < 0.2f)
                    {
                        roleModel.SetPosition(vacancy_for_priest[i]);
                        return;
                    }

                    if (Vector3.Distance(boat.transform.position,end) < 0.2f)
                    {
                        Vector3 dir = new Vector3(-1,1,1);
                        Vector3 pos = Vector3.Scale(dir,vacancy_for_priest[i]);
                        roleModel.SetPosition(pos);
                        return;
                    }
                    
                }
             else
                {
                    vacant[i] = false;
                    if (Vector3.Distance(boat.transform.position, start) < 0.2f)
                    {
                        Debug.Log(vacancy_for_devil[i]);
                        roleModel.SetPosition(vacancy_for_devil[i]);
                        return;
                    }

                    if (Vector3.Distance(boat.transform.position, end) < 0.2f)
                    {
                        Vector3 dir = new Vector3(-1, 1, 1);
                        Vector3 pos = Vector3.Scale(dir, vacancy_for_devil[i]);
                        roleModel.SetPosition(pos);
                        return;
                    }
                }
                
            }

        }
        Debug.Log("The boat is full, boarding failed.");
    }

    //void reset()
    //{
    //    roles.Clear();
    //    vacant[0] = true; vacant[1] = true;
    //}

    // Update is called once per frame
    void Update()
    {
        ;
    }

    public void Move()
    {
        if (moving || roles.Count == 0)
        {
            Debug.Log("cannot move the boat");
            return;
        }
        moving = true;
        for (int i=0;i<roles.Count;i++)
        {
            roles[i].start_moving();

        }

        if (Vector3.Distance(boat.transform.position, start) < 0.2f)
        {
            start2end = true;

            //reverse_vacancy();
        }
        else if (Vector3.Distance(boat.transform.position, end) < 0.2f)
        {
            start2end = false;
            //reverse_vacancy();

        }
    }

    
}