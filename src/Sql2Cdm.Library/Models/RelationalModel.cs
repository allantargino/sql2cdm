using System.Collections.Generic;

namespace Sql2Cdm.Library.Models
{
    public class RelationalModel
    {
        public IEnumerable<Table> Tables{ get; set; }
        
        public RelationalModel()
        {
            Tables = new List<Table>();
        }
    }
}
