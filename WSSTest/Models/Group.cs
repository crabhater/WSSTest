using WSSTest.InterfacesLib;

namespace WSSTest.Models
{
    public class Group : IDBEntity
    {
        public int Id { get ; set ; }
        public int? DepartamentId { get ; set ; }
        public string Name { get; set; }
        public string FullName { get {  return Name + Id.ToString(); } }

    }
}
