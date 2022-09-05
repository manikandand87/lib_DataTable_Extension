using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class ConcatenateRows : CodeActivity
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
        [Description("Values separator such as comma or semicolon.")]
        [DisplayName("Separator")]
        public InArgument<string> Separator { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Prepend each value with this string.")]
        [DisplayName("Leading String")]
        public InArgument<string> LeadingString { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Append this string to each value.")]
        [DisplayName("Trailing String")]
        public InArgument<string> TrailingString { get; set; }

        [Category("Output")]
        [DisplayName("Concatenated Values")]
        public OutArgument<string> ConcatenatedString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ConcatenatedString.Set(context,
                DataTableManipulation.ConcatenateRows(InputDataTable.Get(context),
                ColumnName.Get(context), Separator.Get(context),
                LeadingString.Get(context),
                TrailingString.Get(context)));
        }
    }
}