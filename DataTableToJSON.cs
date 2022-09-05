using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class DataTable_To_JSON : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("Input in Datatable format")]
        [DisplayName("Input Datatable")]
        public InArgument<DataTable> InputDataTable{ get; set; }

        [Category("Output")]
        [DisplayName("JSON Output")]
        public OutArgument<string> JsonOutput { get; set; }

        protected override void Execute(CodeActivityContext context)
        {            
            UtilityJSON utilityJSON = new UtilityJSON();
            JsonOutput.Set(context, utilityJSON.ConvertToJSON(InputDataTable.Get(context)));
        }
    }
}
