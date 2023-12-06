using Newtonsoft.Json.Linq;

namespace GameSystems.Core
{
    
    [System.Serializable]
    public class CellLayout
    {   
        public bool IsStart { get; set; }
        public CellType Type;
        public int cellHeight;

        public CellLayout()
        {
        }

        public CellLayout(JToken token)
        {
            this.cellHeight = Util.NullableCaster.CastInt(token["cellHeight"]);
            var castedType = Util.NullableCaster.CastInt(token["Type"]);
            Type = (CellType) castedType;
        }

        public CellLayout(int heigt, CellType type)
        {
            this.cellHeight = heigt;
            this.Type = type;
        }

   
    }
}