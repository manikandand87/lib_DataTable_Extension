using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_DataTable_Extension
{
    public class UtilityJSON
    {
        private class JSON
        {
            public const string Array = "JSON:Array";
            public const string Null = "JSON:Null";
        }

        public string ConvertToJSON(DataTable dt)
        {
            object o = SerialiseGeneric(dt, true);
            return JsonConvert.SerializeObject(o);
        }

        public object SerialiseGeneric(object o, bool removeArray)
        {
            DataTable dt = o as DataTable;
            if (dt != null)
                return SerialiseDataTable(dt);

            DataRow dr = o as DataRow;
            if (dr != null)
                return SerialiseDataRow(dr, removeArray);

            string s = o as string;
            if (s != null && s == JSON.Null)
                return null;

            if (o != null)
                return o;

            return null;
        }

        public object SerialiseDataTable(DataTable dt)
        {
            if (IsSingleRow(dt))
                return SerialiseGeneric(dt.Rows[0], false);
            else
            {
                JArray ja = new JArray();
                foreach (DataRow r in dt.Rows)
                    ja.Add(SerialiseGeneric(r, true));
                return ja;
            }
        }

        public bool IsSingleRow(DataTable dt)
        {
            if (dt.ExtendedProperties.Contains("SingleRow"))
                return System.Convert.ToBoolean(dt.ExtendedProperties["SingleRow"]);
            // Fallback for older versions of blueprism
            return dt.Rows.Count == 1;
        }

        public object SerialiseDataRow(DataRow dr, bool removeArray)
        {
            JObject jo = new JObject();
            foreach (DataColumn c in dr.Table.Columns)
            {
                string s = c.ColumnName;
                if (removeArray && s == JSON.Array)
                    return SerialiseGeneric(dr[s], true);
                jo[s] = JToken.FromObject(SerialiseGeneric(dr[s], false));
            }
            return jo;
        }

        public DataTable ConvertToDataTable(string json)
        {
            object o = JsonConvert.DeserializeObject(json);
            return (DataTable)DeserialiseGeneric(o, true);
        }

        private object DeserialiseGeneric(object o, bool populate)
        {
            JArray a = o as JArray;
            if (a != null)
                return DeserialiseArray(a, populate);

            JObject jo = o as JObject;
            if (jo != null)
                return DeserialiseObject(jo, populate);

            JValue jv = o as JValue;
            if (jv != null)
                return jv.Value;

            return JSON.Null;
        }

        private string GetKey(KeyValuePair<string, JToken> kv)
        {
            if (kv.Key != null)
                return kv.Key.ToString();
            return "";
        }


        private DataTable DeserialiseObject(JObject o, bool populate)
        {
            DataTable dt = new DataTable();

            foreach (KeyValuePair<string, JToken> kv in o)
            {
                Type type = GetTypeOf(DeserialiseGeneric(kv.Value, false));
                dt.Columns.Add(GetKey(kv), type);
            }

            if (populate)
            {
                DataRow dr = dt.NewRow();
                foreach (KeyValuePair<string, JToken> kv in o)
                    dr[GetKey(kv)] = DeserialiseGeneric(kv.Value, true);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable DeserialiseArray(JArray o, bool populate)
        {
            DataTable dt = new DataTable();

            Type first = null;
            foreach (object e in o)
            {
                if (first == null)
                    first = GetTypeOf(DeserialiseGeneric(e, false));
                if (GetTypeOf(DeserialiseGeneric(e, false)) != first)
                    throw new Exception("Data Type mismatch in array");
            }
            if (first != null)
                dt.Columns.Add(JSON.Array, first);

            if (populate)
            {
                foreach (object e in o)
                {
                    DataRow dr = dt.NewRow();
                    dr[JSON.Array] = DeserialiseGeneric(e, true);
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private Type GetTypeOf(object o)
        {
            if (o == null)
                return typeof(string);
            return o.GetType();
        }
    }
}
