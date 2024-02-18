using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public LayerMask layerToCheck;
    RaycastHit hit;
    TileInfo info;

    Playfield playfield;

    public void SetPlayfield(Playfield _playfield)
    {
        playfield = _playfield;
    }
    public bool OverTile()
    {
        info = GetTileInfo();
        if (info != null)
        {
            return true;
        }
        info = null;
        return false;
    }
    public TileInfo GetTileInfo()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit, 1f, layerToCheck))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            return hit.collider.GetComponent<TileInfo>();
        }
        return null;
    }
}
