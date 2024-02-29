using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int xPos;
    public int zPos;

    bool shot;

    public SpriteRenderer sprite;
    public Sprite[] tileHighlights;
    //0-frame,1-crossHair,2-water,3-ship

    public void ActivateHighlight(int index,bool _shot)
    {
        sprite.sprite = tileHighlights[index];

        shot = _shot;
    }
    public void SetTileInfo(int _xPos, int _zPos)
    {
        xPos = _xPos;
        zPos = _zPos;
    }
    private void OnMouseOver()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.SHOOTING)
        {
            if (!shot)
            {
                ActivateHighlight(1,false);
            }
            if(Input.GetMouseButtonDown(0))
            {

            }
        }
    }
    private void OnMouseExit()
    {
        if (!shot)
        {
            ActivateHighlight(0,false);
        }
    }
}
