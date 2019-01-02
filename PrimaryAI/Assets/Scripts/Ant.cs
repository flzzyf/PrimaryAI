using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ant : MonoBehaviour
{
    public float speed = 3;
    public float rotateSpeed = 1;

    public Vector2 idleTimeRange = new Vector2(2, 3);

    public Vector2 rotateAngleRange = new Vector2(20, 90);

    public Vector2 speedRange = new Vector2(2, 4);

    float timer;

    public float eyeSight = 3;

    public Transform target;

    public List<Ant> followers = new List<Ant>();

    public float followDistance = 10;

    void Start()
    {
        //初始随机朝向
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        speed = Random.Range(speedRange.x, speedRange.y);
    }
    
    void Update()
    {
        if(target != null)
        {
            return;
        }


        //偶尔随机转向
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = Random.Range(idleTimeRange.x, idleTimeRange.y);

            //随机方向旋转
            RandomRotate();
        }

        if (followers.Count > 1)
        {
            for (int i = followers.Count - 1; i > 0; i--)
            {
                //距离太远脱离
                if(Vector2.Distance(followers[i].transform.position, transform.position) > followDistance)
                {
                    followers[i].target = null;
                    followers.RemoveAt(i);
                }
            }

            return;
        }

        //看见其他人
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, eyeSight);
        foreach (var item in hit)
        {
            if (item.collider.gameObject != gameObject)
            {
                if (item.collider.gameObject.tag == "Bot")
                {
                    FindTarget(item.collider.gameObject);
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        //前进
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    //发现目标
    void FindTarget(GameObject _target)
    {
        target = _target.transform;

        if (_target.tag == "Bot")
        {
            transform.DORotate(_target.transform.eulerAngles, 1);

            _target.GetComponent<Ant>().followers.Add(this);
        }
    }

    //随机旋转
    void RandomRotate()
    {
        int sign = Random.Range(0, 2) == 0 ? -1 : 1;
        float rotateAngle = Random.Range(rotateAngleRange.x, rotateAngleRange.y);
        float rotateTime = rotateAngle * rotateSpeed * Time.deltaTime;
        Vector3 targetRotation = transform.eulerAngles + new Vector3(0, 0, sign * rotateAngle);

        RotateTo(targetRotation, rotateTime);
    }

    public void RotateTo(Vector3 _rotation, float _time)
    {
        transform.DORotate(_rotation, _time);

        //如果有追随者，让其也跟着转
        if (followers.Count > 0)
        {
            foreach (var item in followers)
            {
                item.RotateTo(_rotation, _time);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * eyeSight);

        if(target != null)
        {
            Gizmos.DrawWireSphere(transform.position, .5f);
        }

        Gizmos.color = Color.green;
        if (followers.Count > 0)
        {
            foreach (var item in followers)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }

}
