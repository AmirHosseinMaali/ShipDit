using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        [Space]
        public GameObject camPos;
        public GameObject placePanel;
        public GameObject shootPanel;
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

    public enum GameState
    {
        P1_PLACE_SHIPS,
        P2_PLACE_SHIPS,
        SHOOTING,
        IDLE
    }
    public GameState gameState;

    public GameObject battleCamPos;

    bool camIsMoving;

    public GameObject placingCanvas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HideAllPanels();

        players[activePlayer].placePanel.SetActive(true);
        gameState = GameState.IDLE;


    }

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
        DebugGrid();
    }
    public bool CheckIfOccupied(int xPos, int zPos)
    {
        return players[activePlayer].myGrid[xPos, zPos].IsOccupied();
    }
    void DebugGrid()
    {
        string s = "";
        int sep = 0;
        for (int i = 0; i < 10; i++)
        {
            s += "|";
            for (int j = 0; j < 10; j++)
            {
                string t = "0";
                if (players[activePlayer].myGrid[i, j].type == OccupaationType.BATTLESHIP)
                {
                    t = "B";
                }
                if (players[activePlayer].myGrid[i, j].type == OccupaationType.CARRIER)
                {
                    t = "C";
                }
                if (players[activePlayer].myGrid[i, j].type == OccupaationType.CRUISER)
                {
                    t = "R";
                }
                if (players[activePlayer].myGrid[i, j].type == OccupaationType.SUBMARINE)
                {
                    t = "S";
                }
                if (players[activePlayer].myGrid[i, j].type == OccupaationType.DESTROYER)
                {
                    t = "D";
                }
                s += t;
                sep = j % 10;
                if (sep == 9)
                {
                    s += "|";
                }
                s += "\n";
            }
            print(s);
        }
    }

    public void DeleteAllShips()
    {
        foreach (var item in players[activePlayer].placedShipList)
        {
            Destroy(item);
        }
        players[activePlayer].placedShipList.Clear();
        InitGrid();
    }

    void InitGrid()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                OccupaationType type = OccupaationType.EMPTY;
                players[activePlayer].myGrid[x, y] = new Tile(type, null);
            }
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.P1_PLACE_SHIPS:
                players[activePlayer].placePanel.SetActive(false);

                PlacingManager.Instance.SetPlayer(players[activePlayer].playfield, players[activePlayer].playerType.ToString());
                StartCoroutine(MoveCamera(players[activePlayer].camPos));
                gameState = GameState.IDLE;
                break;
            case GameState.P2_PLACE_SHIPS:
                players[activePlayer].placePanel.SetActive(false);

                PlacingManager.Instance.SetPlayer(players[activePlayer].playfield, players[activePlayer].playerType.ToString());
                gameState = GameState.IDLE;
                break;
            case GameState.SHOOTING:
                if (players[activePlayer].playerType == Player.PlayerType.AI)
                {

                }
                break;
            case GameState.IDLE:
                break;
            default:
                break;
        }
    }

    void HideAllPanels()
    {
        players[0].placePanel.SetActive(false);
        players[0].shootPanel.SetActive(false);

        players[1].placePanel.SetActive(false);
        players[1].shootPanel.SetActive(false);
    }

    public void P1PlaceShips()
    {
        gameState = GameState.P1_PLACE_SHIPS;
    }

    public void P2PlaceShips()
    {
        gameState = GameState.P2_PLACE_SHIPS;
    }

    public void PlacingReady()
    {
        if (activePlayer == 0)
        {
            HideAllMyShips();
            SwitchPlayer();
            StartCoroutine(MoveCamera(players[activePlayer].camPos));
            players[activePlayer].placePanel.SetActive(true);
            return;
        }
        if (activePlayer == 1)
        {
            HideAllMyShips();
            SwitchPlayer();
            StartCoroutine(MoveCamera(battleCamPos));
            players[activePlayer].shootPanel.SetActive(true);
            placingCanvas.SetActive(false);
        }
    }

    void HideAllMyShips()
    {
        foreach (var ship in players[activePlayer].placedShipList)
        {
            ship.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void UnHideAllMyShips()
    {
        foreach (var ship in players[activePlayer].placedShipList)
        {
            ship.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void SwitchPlayer()
    {
        activePlayer++;
        activePlayer %= 2;
    }

    IEnumerator MoveCamera(GameObject camObj)
    {
        if (camIsMoving)
        {
            yield break;
        }
        camIsMoving = true;

        float t = 0;
        float duration = 0.5f;

        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;

        Vector3 endPos = camObj.transform.position;
        Quaternion endPosRot = camObj.transform.rotation;

        while (t < duration)
        {
            t += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startPos, endPos, t / duration);
            Camera.main.transform.rotation = Quaternion.Lerp(startRot, endPosRot, t / duration);
            yield return null;
        }
        camIsMoving = false;
    }

    public void ShotButton()
    {
        UnHideAllMyShips();
        players[activePlayer].shootPanel.SetActive(false);
        gameState = GameState.SHOOTING;
    }

    int Opponent()
    {
        int me = activePlayer;
        me++;
        me %= 2;
        int opponent = me;
        return opponent;
    }
}
