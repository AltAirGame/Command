namespace MHamidi
{
    public interface ICellEditor:IInteractable
    {
        
        public CellType type { get; set; }

        public int cellType { get; set; }

        //Update Value is For Level Editor 
        void SetValue(CellType type);
        void ChangeValue();

        

    }
}