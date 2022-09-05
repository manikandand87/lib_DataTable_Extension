using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class JSONToDatatable : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("JSON String")]
        [DisplayName("JSON String")]
        public InArgument<string> JSONString { get; set; }       

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            UtilityJSON utilityJSON = new UtilityJSON();
            OutputDataTable.Set(context, utilityJSON.ConvertToDataTable(JSONString.Get(context)));
        }     
    }
}
