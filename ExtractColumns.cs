using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class ExtractColumns : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Comma-separated names of columns that will remain in the table.")]
        [DisplayName("Desired Columns")]
        public InArgument<string> DesiredColumns { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            OutputDataTable.Set(context,
                DataTableManipulation.ExtractColumns(InputDataTable.Get(context), DesiredColumns.Get(context)));
        }
    }
}
