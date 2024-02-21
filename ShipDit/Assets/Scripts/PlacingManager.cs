using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacingManager : MonoBehaviour
{
    public static PlacingManager Instance;

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
        public TMP_Text amountText;
        [HideInInspector] public int placedAmount = 0;
    }

    public List<ShipsToPlace> shipList = new List<ShipsToPlace>();
    public Button readyButton;
    int currentShip;
    RaycastHit hit;
    Vector3 hitPoint;

    private void Awake()
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateAmountText();
        ActivateShipGhost(-1);
    }

    public void SetPlayer(Playfield _playfield,string plyerType)
    {
        playfield = _playfield;
        readyButton.interactable = false;

        ClearAllShips();

        if( plyerType== "AI")
        {

        }
    }

    void Update()
    {
        if (isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerToCheck))
            {
                if (!playfield.RequestTile(hit.collider.GetComponent<TileInfo>()))
                {
                    return;
                }
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

        GameManager.Instance.UpdateGrid(shipList[currentShip].shipGhost.transform, newShip.GetComponent<ShipBehavior>(), newShip);

        shipList[currentShip].placedAmount++;
        isPlacing = false;
        ActivateShipGhost(-1);

        CheckEveryShipPlaced();

        UpdateAmountText();

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

    bool CheckEveryShipPlaced()
    {
        foreach (var item in shipList)
        {
            if (item.placedAmount != item.amountToPlace) return false;
        }
        readyButton.interactable = true;
        return true;
    }

    public void ShipButton(int index)
    {
        if (CheckIfAllShipsPlaced(index))
        {
            Debug.Log("You have placed enough");
            return;
        }
        currentShip = index;
        ActivateShipGhost(currentShip);
        isPlacing = true;
    }

    bool CheckIfAllShipsPlaced(int index)
    {
        return shipList[index].placedAmount == shipList[index].amountToPlace;
    }

    void UpdateAmountText()
    {
        foreach (var item in shipList)
        {
            item.amountText.text = (item.amountToPlace - item.placedAmount).ToString();
        }
    }

    public void ClearAllShips()
    {
        GameManager.Instance.DeleteAllShips();
        foreach (var item in shipList)
        {
            item.placedAmount = 0;
        }
        UpdateAmountText();
    }
}
