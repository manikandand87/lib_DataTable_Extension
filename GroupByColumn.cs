using System.Activities;
using System.ComponentModel;
using System.Data;

namespace lib_DataTable_Extension
{
    public class GroupByColumn : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("Datatable object.")]
        [DisplayName("Input DataTable")]
        public InArgument<DataTable> InputDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("Name of the column for which values will be grouped.")]
        [DisplayName("Group By Column Name")]
        public InArgument<string> GroupByColumnName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("e.g. 'Sum(Field1), Count([Field 2]), Max(FieldN)'")]
        [DisplayName("Aggregate Functions with Column Names")]
        public InArgument<string> AggregateFunctions { get; set; }

        [Category("Output")]
        [Description("Datatable object.")]
        public OutArgument<DataTable> OuputDataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            //returning grouped/counted result
            OuputDataTable.Set(context, DataTableManipulation.GroupByColumn(InputDataTable.Get(context), AggregateFunctions.Get(context), GroupByColumnName.Get(context)));
        }
    }
}
