using System.Data.Common;

namespace ChronoLedger.Common.Database;

public interface IDbConnectionFactory
{
    DbConnection Create();
}