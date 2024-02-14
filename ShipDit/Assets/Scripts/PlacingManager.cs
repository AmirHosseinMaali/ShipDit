using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacingManager : MonoBehaviour
{
    public bool isPlacing;
    bool canPlace;

    Playfield playfield;
    public LayerMask layerToCheck;

    [System.Serializable]
    public class ShipsToPlace
    {
        public GameObject shipGhost;
        public GameObject shipPrefab;
        public int amountToPlace = 1;
        [HideInInspector] public int placedAmount = 0;
    }

    public List<ShipsToPlace> shipList = new List<ShipsToPlace>();
    int currentShip = 1;
    RaycastHit hit;
    Vector3 hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        ActivateShipGhost(-1);
        ActivateShipGhost(currentShip);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerToCheck))
            {
                hitPoint = hit.point;
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceShip();
            }

            if (Input.GetMouseButtonDown(1))
            {
                RotateGhost();
            }
            PlaceGhost();
        }
    }

    void ActivateShipGhost(int index)
    {
        if (index != -1)
        {
            if (shipList[index].shipGhost.activeInHierarchy) return;

        }
        for (int i = 0; i < shipList.Count; i++)
        {
            shipList[i].shipGhost.SetActive(false);
        }
        if (index == -1) return;
        shipList[index].shipGhost.SetActive(true);
    }

    void PlaceGhost()
    {
        if (isPlacing)
        {
            canPlace = CheckForOtherShips();
            shipList[currentShip].shipGhost.transform.position = new Vector3(Mathf.Round(hitPoint.x), 0, Mathf.Round(hitPoint.z));
        }
        else
        {
            ActivateShipGhost(-1);
        }
    }

    void PlaceShip()
    {
        Vector3 pos = new Vector3(MathF.Round(hitPoint.x), 0, Mathf.Round(hitPoint.z));
        Quaternion rot = shipList[currentShip].shipGhost.transform.rotation;
        GameObject newShip = Instantiate(shipList[currentShip].shipPrefab, pos, rot);


    }

    void RotateGhost()
    {
        shipList[currentShip].shipGhost.transform.localEulerAngles += new Vector3(0, 90, 0);
    }
    bool CheckForOtherShips()
    {
        foreach (Transform child in shipList[currentShip].shipGhost.transform)
        {
            GhostBehavior ghost = child.GetComponent<GhostBehavior>();
            if (!ghost.OverTile())
            {
                child.GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 120);
                return false;
            }
            child.GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 0, 100);
        }
        return true;
    }

}
