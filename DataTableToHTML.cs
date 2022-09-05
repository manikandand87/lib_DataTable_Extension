using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class DataTableToHTML : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("Datatable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("e.g. th { background-color: lightblue; }")]
        [DisplayName("CSS Styling")]
        public InArgument<string> CSS { get; set; }

        [Category("Output")]        
        [DisplayName("HTML String")]
        public OutArgument<string> HTMLString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            HTMLString.Set(context, DataTableManipulation.DataTableToHTML(InputDataTable.Get(context), CSS.Get(context)));
        }
    }
}
