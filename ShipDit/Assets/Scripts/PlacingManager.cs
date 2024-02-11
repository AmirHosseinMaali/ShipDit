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
    int currentShip;
    RaycastHit hit;
    Vector3 hitPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlacing) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerToCheck))
        {
            hitPoint = hit.point;
        }

        if (Input.GetMouseButtonDown(0) && canPlace)
        {

        }

        if (Input.GetMouseButtonDown(1))
        {

        }
    }
}
