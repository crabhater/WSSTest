using WSSTest.InterfacesLib;

namespace WSSTest.Models
{
    public class Departament : IDBEntity
    {
        public int Id { get ; set ; }
        public List<Group> Child { get ; set ; }
        public int? CompanyId { get ; set ; }
        public string Name { get; set; }
    }
}
