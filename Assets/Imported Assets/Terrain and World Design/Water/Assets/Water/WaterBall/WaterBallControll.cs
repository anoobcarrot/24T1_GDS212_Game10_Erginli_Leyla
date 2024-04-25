using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBallControll : MonoBehaviour
{
    [SerializeField] bool _update;
    [SerializeField] Transform _CreationPoint;
    [SerializeField] WaterBall WaterBallPrefab;
    public KeyCode createWaterBallKey = KeyCode.Mouse0;
    WaterBall waterBall;
    private void Update()
    {
        if (!_update)
        {
            return;
        }

        if (Input.GetKeyDown(createWaterBallKey))
        {
            if (WaterBallCreated())
            {
                CreateWaterBall();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (waterBall != null)
                    {
                        ThrowWaterBall(hit.point);
                    }
                }
            }
        }

        if (Input.GetKeyUp(createWaterBallKey))
        {
            if (WaterBallCreated())
            {
                DestroyWaterBall();
            }
        }
    }

    public bool WaterBallCreated()
    {
        return waterBall != null;
    }
    public void CreateWaterBall()
    {
        waterBall = Instantiate(WaterBallPrefab, _CreationPoint.position, Quaternion.identity);
    }

    public void ThrowWaterBall(Vector3 pos)
    {
        waterBall.Throw(pos);
    }

    public void DestroyWaterBall()
    {
        Destroy(waterBall.gameObject);

        waterBall = null;
    }

    public GameObject GetWaterBall()
    {
        return waterBall.gameObject;
    }
}
