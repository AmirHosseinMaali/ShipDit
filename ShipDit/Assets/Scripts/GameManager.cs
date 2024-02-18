using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class Player
    {
        public enum PlayerType
        {
            Human,
            AI
        }
        public PlayerType playerType;
        public Tile[,] myGrid = new Tile[10, 10];
        public bool[,] revealedGrid = new bool[10, 10];
        public Playfield playfield;
        public LayerMask layerToPlaceOn;
        public Player()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    OccupaationType type = OccupaationType.EMPTY;
                    myGrid[x, y] = new Tile(type, null);
                    revealedGrid[x, y] = false;
                }
            }
        }
        public List<GameObject> placedShipList = new List<GameObject>();
    }
    int activePlayer;
    public Player[] players = new Player[2];
    void AddShipToList(GameObject placedShip)
    {
        players[activePlayer].placedShipList.Add(placedShip);
    }

    public void UpdateGrid(Transform shipTransform, ShipBehavior ship, GameObject placedShip)
    {
        foreach (Transform child in shipTransform)
        {
            TileInfo tInfo = child.GetComponent<GhostBehavior>().GetTileInfo();
            players[activePlayer].myGrid[tInfo.xPos, tInfo.zPos] = new Tile(ship.type, ship);
        }
        AddShipToList(placedShip);
    }
}
