namespace Datos.DTO.Infraestructura.ViewModels
{
    public class PaginacionViewModel
    {
        public int Paginas { get; set; }
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalEncontrados { get; set; }

        public PaginacionViewModel()
        {
            PaginaActual = 1;
            TamanoPagina = 10;
        }
    }
}
