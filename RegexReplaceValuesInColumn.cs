using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class RegexReplaceValuesInColumn : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Column to loop rows in.")]
        [DisplayName("Column Name")]
        public InArgument<string> ColumnName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Regex pattern.")]
        public InArgument<string> RegEx { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Text to replace matched values with.")]
        [DisplayName("Replace Text")]
        public InArgument<string> ReplaceText { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            OutputDataTable.Set(context,
                DataTableManipulation.RegexReplace(InputDataTable.Get(context), ColumnName.Get(context), RegEx.Get(context), ReplaceText.Get(context)));
        }
    }
}
