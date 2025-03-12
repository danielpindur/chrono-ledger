using System.Data;

namespace ChronoLedger.Common.Database;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}