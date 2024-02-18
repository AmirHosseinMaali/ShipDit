public enum OccupaationType
{
    EMPTY,
    CRUISER,
    DESTROYER,
    SUBMARINE,
    BATTLESHIP,
    CARRIER
}
public class Tile
{
    public OccupaationType type;
    public ShipBehavior placedShip;

    public Tile(OccupaationType type, ShipBehavior placedShip)
    {
        this.type = type;
        this.placedShip = placedShip;
    }
    public bool IsOccupied()
    {
        return type == OccupaationType.BATTLESHIP 
            || type == OccupaationType.CARRIER 
            || type == OccupaationType.SUBMARINE 
            || type == OccupaationType.DESTROYER 
            || type == OccupaationType.CRUISER;
    }
}
