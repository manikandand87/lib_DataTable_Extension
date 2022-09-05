using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class ConcatenateColumns : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Text separator such as comma or semicolon.")]
        [DisplayName("Separator")]
        public InArgument<string> Separator { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Column name of the generated DataTable")]
        [DisplayName("Column Name")]
        public InArgument<string> ColumnName { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {          
            OutputDataTable.Set(context, DataTableManipulation.ConcatenateColumns(InputDataTable.Get(context),Separator.Get(context),ColumnName.Get(context)));
        }
    }
}

