using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class FilterDistinctRows : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("Input Collection in DataTable Format")]
        [DisplayName("Input Collection")]
        public InArgument<DataTable> InputCollection { get; set; }     

        [Category("Output")]
        [DisplayName("Output Collection")]
        public OutArgument<DataTable> OutputCollection { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            OutputCollection.Set(context, InputCollection.Get(context).DefaultView.ToTable(true));
        }
    }
}
