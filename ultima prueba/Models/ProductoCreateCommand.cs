namespace ultima_prueba.Models
{
    public class ProductoCreateCommand
    {
        public string? NombreProd { get; set; }
        public Int32 Cantidad { get; set; }
        public Int32 PrecioUnit { get; set; }
    }
}