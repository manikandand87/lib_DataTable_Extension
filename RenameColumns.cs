using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class RenameColumns : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable with same columns as input datable and with correct column names in the first row.")]
        [DisplayName("DataTable with Correct Headers")]
        public InArgument<DataTable> Headers { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DataTable dt = InputDataTable.Get(context);
            DataTable headers = Headers.Get(context);

            for (int i = 0; i < headers.Columns.Count; i++)
            {
                dt.Columns[headers.Columns[i].ColumnName].ColumnName = headers.Rows[0][i].ToString();
            }
            OutputDataTable.Set(context, dt);
        }    
    }
}
