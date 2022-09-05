using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class ReplaceValuesInColumn : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("DataTable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Name fo the column to loop rows in.")]
        [DisplayName("Column Name")]
        public InArgument<string> ColumnName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Value to search for.")]
        [DisplayName("Search Value")]
        public InArgument<string> SearchValue { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Replace by this value.")]
        [DisplayName("Replace By")]
        public InArgument<string> ReplaceBy { get; set; }

        [Category("Output")]
        [DisplayName("Output DataTable")]
        public OutArgument<DataTable> OutputDataTable { get; set; }  

        protected override void Execute(CodeActivityContext context)
        {
            DataTable dt = InputDataTable.Get(context);
            string columnName = ColumnName.Get(context);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][columnName] = dt.Rows[i][columnName].ToString()
                    .Replace(SearchValue.Get(context), ReplaceBy.Get(context));
            }

            OutputDataTable.Set(context, dt);
        }
    }
}
