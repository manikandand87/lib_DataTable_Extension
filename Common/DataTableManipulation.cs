using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace lib_DataTable_Extension
{
    public static class DataTableManipulation
    {
        public static DataTable GroupByColumn(DataTable input, string aggregateFunctions, string GroupByField)
        {
            // getting distinct values for grouped column
            DataView dv = new DataView(input);            
            DataTable dtGroup = dv.ToTable(true, new string[] { GroupByField });
            
            // split aggregate columns
            string[] aggregateColumns = aggregateFunctions.Split(',');

            // clean column names
            string[] cleanedAggregateColumns = new string[aggregateColumns.Length];
            for (int i = 0; i < aggregateColumns.Length; i++)
            {
                int indexOfLeftBracket = aggregateColumns[i].IndexOf("(");
                cleanedAggregateColumns[i] = aggregateColumns[i].Substring(indexOfLeftBracket + 1, -1 + aggregateColumns[i].Length - (indexOfLeftBracket + 1))
                    + " (" + aggregateColumns[i].Substring(0, indexOfLeftBracket) + ")";
                cleanedAggregateColumns[i] = cleanedAggregateColumns[i].Replace("[", "").Replace("]", "");
            }

            // add aggregate columns
            foreach (string colName in cleanedAggregateColumns)
            {   
                dtGroup.Columns.Add(colName);
            }

            // looping through distinct values for the group and counting
            foreach (DataRow dr in dtGroup.Rows)
            {
                for (int i = 0; i < aggregateColumns.Length; i++)
                {
                    dr[cleanedAggregateColumns[i]] = input.Compute(aggregateColumns[i], "[" + GroupByField + "]" + " = '" + dr[GroupByField] + "'");
                }

            }

            return dtGroup;
        }

        public static string DataTableToHTML(DataTable dt, string css)
        {
            StringBuilder html = new StringBuilder();// replace with StringBuilder
            html.Append("<table>");
            //add header row
            html.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
                html.Append("<th>" + dt.Columns[i].ColumnName + "</th>");
            html.Append("</tr>");
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                    html.Append("<td>" + dt.Rows[i][j].ToString() + "</td>");
                html.Append("</tr>");
            }
            html.Append("</table>");

            if (!string.IsNullOrWhiteSpace(css))
            {
                string tempStr = "<html><head><style>" + css + "</style></head>" + html.ToString() + "</html>";
                html.Clear();
                html.Append(tempStr);
            }
            return html.ToString();
        }

        public static DataTable ConvertColumnsToString(DataTable InputDataTable)
        {
            DataTable dtOutput = new DataTable();

            foreach (DataColumn col in InputDataTable.Columns)
                dtOutput.Columns.Add(col.ColumnName);

            DataRow newRow;
            foreach (DataRow row in InputDataTable.Rows)
            {
                newRow = dtOutput.NewRow();
                foreach (DataColumn col in InputDataTable.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName].ToString();
                }
                dtOutput.Rows.Add(newRow);
            }
            return dtOutput;
        }

        public static DataTable SplitDistinctRows(DataTable Collection_In, string FieldName, out DataTable Collection_Out_Dulplicate)
        {
            DataTable Collection_Out;

            //Type t = Collection_In.Columns[FieldName].DataType;
            var duplicates = Collection_In.AsEnumerable()
                .GroupBy(i => new { CustomerNumber = i.Field<dynamic>(FieldName) })
                .Where(g => g.Count() > 1)
                .Select(g => g.Key.CustomerNumber)
                .ToList();

            var outtable = Collection_In.AsEnumerable()
                            .Where(t => duplicates.Contains(t.Field<dynamic>(FieldName)))
                            .OrderByDescending(t => t.Field<dynamic>(FieldName))
                            .ToList();

            if (outtable.Count == 0)
            {
                Collection_Out_Dulplicate = Collection_In.Clone();
            }
            else
            {
                Collection_Out_Dulplicate = outtable.CopyToDataTable();
            }

            var NonDuplicates = Collection_In.AsEnumerable()
                           .GroupBy(i => new { CustomerNumber = i.Field<dynamic>(FieldName) })
                           .Where(g => g.Count() == 1)
                           .Select(g => g.Key.CustomerNumber)
                           .ToList();

            outtable = Collection_In.AsEnumerable().Where(t => NonDuplicates.Contains(t.Field<dynamic>(FieldName))).ToList();

            if (outtable.Count == 0)
            {
                Collection_Out = Collection_In.Clone();
            }
            else
            {
                Collection_Out = outtable.CopyToDataTable();
            }

            return Collection_Out;

        }

        public static DataTable ConcatenateColumns(DataTable InputDataTable, string Separator, string colName)
        {
            DataTable dtOutput = new DataTable();
            dtOutput.Columns.Add(colName);

            DataRow newDr = null;

            string concatenatedString = string.Empty;

            foreach (DataRow dr in InputDataTable.Rows)
            {
                newDr = dtOutput.NewRow();
                concatenatedString = string.Join(Separator, InputDataTable.Columns.Cast<DataColumn>().Select(x => dr[x.ColumnName]));
                newDr[0] = concatenatedString;
                dtOutput.Rows.Add(newDr);
            }
            return dtOutput;
        }

        public static string ConcatenateRows(DataTable InputDataTable, string ColumnName, string Separator, string LeadingString, string TrailingString)
        {

            string ConcatenatedString = string.Empty; 

            if (InputDataTable != null & InputDataTable.Rows.Count > 0)
            {
                IEnumerable<string> result = null;

                result = InputDataTable.AsEnumerable().Select(x => LeadingString + x.Field<object>(ColumnName).ToString() + TrailingString);

                if (result != null)
                    ConcatenatedString = string.Join(Separator, result);
            }
            return ConcatenatedString;
        }

        public static DataTable ExtractColumns(DataTable InputDataTable, string DesiredColumns)
        {
            string[] columns = DesiredColumns.Split(',');

            List<string> collColumns = InputDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

            foreach (string col in collColumns)
            {
                if (!columns.Any(x => x.ToLower() == col.ToLower()))
                {
                    InputDataTable.Columns.Remove(col);
                }
            }

            DataColumn colm;

            for (int i = 0; i < columns.Length; i++)
            {
                colm = InputDataTable.Columns[columns[i]];
                colm.SetOrdinal(i);
            }

            return InputDataTable;
        }

        public static DataTable RegexReplace(DataTable Input, string ColumnName, string RegExStr, string ReplaceText)
        {
            var regex = new Regex(RegExStr);
            foreach (var row in Input.AsEnumerable())
            {
                if (regex.IsMatch(row[ColumnName] as string))
                {
                    row[ColumnName] = regex.Replace(row[ColumnName].ToString(), ReplaceText);
                }
            }

            return Input;
        }
    }
}
