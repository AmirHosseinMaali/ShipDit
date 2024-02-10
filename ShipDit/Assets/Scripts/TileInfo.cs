using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int xPos;
    public int zPos;

    bool shot;

    public SpriteRenderer sprite;
    public Sprite[] tileHighlights;
    //0-frame,1-crossHair,2-water,3-ship

    public void ActivateHighlight(int index)
    {
        sprite.sprite = tileHighlights[index];
    }
    public void SetTileInfo(int _xPos, int _zPos)
    {
        xPos = _xPos;
        zPos = _zPos;
    }
    private void OnMouseOver()
    {
        ActivateHighlight(1);
    }
    private void OnMouseExit()
    {
        ActivateHighlight(0);
    }
}
