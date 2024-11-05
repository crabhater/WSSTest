using WSSTest.InterfacesLib;

namespace WSSTest.Models
{
    public class Company : IDBEntity
    {
        public int Id { get ; set ; }
        public List<Departament> Child { get ; set ; }
        public string Name { get; set; }
    }
}
