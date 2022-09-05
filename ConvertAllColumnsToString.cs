using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class ConvertAllColumnsToString : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            DataTable dtInput = InputDataTable.Get(context);
            OutputDataTable.Set(context, DataTableManipulation.ConvertColumnsToString(dtInput));
        }
    }
}
