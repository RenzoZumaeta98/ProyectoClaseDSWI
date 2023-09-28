using System;

namespace ClaseSemana04.Database
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string Status { get; set; }
    }
}
