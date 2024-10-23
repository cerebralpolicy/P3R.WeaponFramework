using Reloaded.Hooks.Definitions.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.ObjectsEmitter.Interfaces;
using Unreal.ObjectsEmitter.Interfaces.Types;

namespace P3R.WeaponFramework.Debugging
{
    internal class DebugService
    {
        public const string WEAPON_DT = "DT_Weapon";
        public const string COSTUME_DT = "DT_Costume";

        private readonly IUnreal unreal;
        private readonly IDataTables dataTables;

        DataTable[] tables;

        public DebugService(IUnreal unreal, IDataTables dataTables)
        {
            this.unreal = unreal;
            this.dataTables = dataTables;
            tables = dataTables.GetDataTables();

            dataTables.FindDataTable(COSTUME_DT, OnDTFind);
            dataTables.FindDataTable(WEAPON_DT, OnDTFind);

        }
        public void DoubleCheckTables()
        {
            if (tables.Any(x => x.Name == WEAPON_DT))
            {
                Log.Debug("Weapon DT exists");
            }
            else
            {
                Log.Debug("Weapon DT does not exist");
            }
        }

        private void OnDTFind(DataTable table)
        {
            var sb = new StringBuilder();
            sb.Append($"Found {table.Name}");
            var rowStruct = UnsafeUtils.GetRowStructName(table, unreal);
            sb.AppendLine($"Row structure: {rowStruct}");
            Log.Debug(sb.ToString());
        }
    }
}
