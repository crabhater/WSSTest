using System.Diagnostics.CodeAnalysis;
using WSSTest.InterfacesLib;

namespace WSSTest.DataContext
{
    public class DBEntityComparer : IEqualityComparer<IDBEntity>
    {
        public bool Equals(IDBEntity? x, IDBEntity? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IDBEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
