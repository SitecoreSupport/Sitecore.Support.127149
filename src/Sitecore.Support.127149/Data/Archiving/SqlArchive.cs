using Sitecore.Data;
using Sitecore.Data.DataProviders.Sql;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using System.Collections.Generic;

namespace Sitecore.Support.Data.Archiving
{
    public class SqlArchive : Sitecore.Data.Archiving.SqlArchive
    {
        public SqlArchive(string name, Database database) : base(name, database)
        {
        }
        protected override int GetEntryCount(User user)
        {
            List<string> list = new List<string>(new string[] { "archiveName", base.Name });
            string sql = "SELECT COUNT(*) FROM {0}Archive{1}\r\n        WHERE {0}ArchiveName{1} = {2}archiveName{3}";
            if (!((user == null) || user.IsAdministrator || Policy.IsAllowed("Recycle Bin/Can See All Items")))
            {
                sql = sql + " AND {0}ArchivedBy{1} = {2}archivedBy{3}";
                list.AddRange(new string[] { "archivedBy", user.Name });
            }
            using (DataProviderReader reader = base.Api.CreateReader(sql, list.ToArray()))
            {
                return (!reader.Read() ? 0 : base.Api.GetInt(0, reader));
            }
        }
    }
}