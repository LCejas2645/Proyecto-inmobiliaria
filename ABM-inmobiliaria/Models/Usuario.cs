namespace ABM_inmobiliaria.Models
{
    public class Usuario
    {

        public int id { get; set; }
        public string password { get; set; } ="";
        public int rol;

        public override string ToString()
        {
            return id+"";
        }

    }


}