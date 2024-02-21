using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public bool fill;
    public GameObject tilePrefab;

    List<GameObject> tileList = new List<GameObject>();
    List<TileInfo> tileInfoList = new List<TileInfo>();

    private void Start()
    {
        tileList.Clear();
        tileInfoList.Clear();


        foreach (Transform item in transform)
        {
            if (item != transform)
            {
                tileList.Add(item.gameObject);
            }
        }

        foreach (GameObject item in tileList)
        {
            tileInfoList.Add(item.GetComponent<TileInfo>());
        }
    }

    public bool RequestTile(TileInfo info)
    {
        return tileInfoList.Contains(info);
    }

    private void OnDrawGizmos()
    {
        if (tilePrefab == null || !fill) return;
        for (int i = 0; i < tileList.Count; ++i)
        {
            DestroyImmediate(tileList[i]);
        }
        tileList.Clear();
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                Vector3 pos = new Vector3(transform.position.x + i, 0, transform.position.z + j);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

                tile.GetComponent<TileInfo>().SetTileInfo(i, j);
                tileList.Add(tile);
            }
        }
    }
}
