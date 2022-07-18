using MHamidi;
using Newtonsoft.Json.Linq;

public class CellLayout
{
    public CellType Type;
    public int cellHeight;

    public CellLayout()
    {
        
    }

    public CellLayout(JToken token)
    {
        this.cellHeight=Util.NullabelCaster.CastInt(token["cellHeight"]);
        var castedType = Util.NullabelCaster.CastInt(token["Type"]);
        Type = (CellType)castedType;
    }

    public CellLayout(int heigt, CellType type)
    {
        this.cellHeight = heigt;
        this.Type = type;
    }
}