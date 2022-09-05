using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace lib_DataTable_Extension
{
    public class ExtractRows : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Number of rows to skip.")]
        [DisplayName("Rows to Skip")]
        public InArgument<int> Skipnum { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Number of rows to extract.")]
        [DisplayName("Rows to Extract")]
        public InArgument<int> Takenum { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            OutputDataTable.Set(context, InputDataTable.Get(context).AsEnumerable().Skip(Skipnum.Get(context)).Take(Takenum.Get(context)).CopyToDataTable());
        }
    }
}
