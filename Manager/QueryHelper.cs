using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Manager
{
    public class QueryHelper
    {
        public MySqlTransaction Transactions { get; set; }
        private MySqlConnection _connection;
        //string connectionString = ConfigurationManager.ConnectionStrings["LocalMySqlServer"].ConnectionString;
        private string server = "localhost";
        private string database = "emanual_db";
        private string uid = "root";
        private string password = "root";

        private string connectionString = "";

        public QueryHelper()
        {
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            //var setTimeZone = ConfigurationManager.AppSettings["mysqlTimeZoneConnection"];
            //QueryInsertUpdateDelete(setTimeZone);
            Transactions = _connection.BeginTransaction();
        }

        public void Close()
        {
            Commit();
        }

        public bool Rollback(bool close = true)
        {
            bool rtn;
            try
            {
                Transactions.Rollback();
                rtn = true;
            }
            catch (Exception)
            {
                rtn = false;
            }
            finally
            {
                if (close)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            return rtn;
        }

        public bool Commit(bool close = true)
        {
            bool rtn;
            try
            {
                Transactions.Commit();
                rtn = true;
            }
            catch (Exception)
            {
                rtn = false;
            }
            finally
            {
                if (close)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            return rtn;
        }
        public long QueryInsertUpdateDelete(string query, bool getLastId = true)
        {
            var data = QueryInsertUpdateDelete(query, _connection, getLastId);
            return data;
        }

        public List<Dictionary<string, string>> QuerySelect(string query)
        {
            var result = QuerySelect(query, _connection);
            return result;
        }

        private static string XssPrevent(string word)
        {
            // Since angular JS have XSS prevention no prevension will be made here
            return word;
        }
        public static string MysqlEscapeString(string word)
        {
            if (word == null)
            {
                return "";
            }
            if (word.ToLower() == "false")
            {
                return "0";
            }
            if (word.ToLower() == "true")
            {
                return "1";
            }
            return XssPrevent(MySqlHelper.EscapeString(word));
        }
        #region Builder
        public static string BuildCheckDuplicateQuery(string tableName, string idName, List<string> condition,
            List<string> data)
        {
            var selectQuery = new StringBuilder();
            selectQuery.AppendFormat("SELECT {0}", idName);
            selectQuery.AppendFormat(" FROM {0}", tableName);
            for (int i = 0; i < condition.Count; i++)
            {
                selectQuery.AppendFormat(" WHERE {0} = '{1}';", condition[i], MysqlEscapeString(data[i]));
            }
            return selectQuery.ToString();
        }
        public static string BuildDeleteQuery(string table, List<string> deleteCondition,
            List<string> deleteDataCondition)
        {
            var deleteQuery = new StringBuilder();
            deleteQuery.AppendFormat("DELETE FROM {0} WHERE ", table);
            for (int i = 0; i < deleteCondition.Count; i++)
            {
                deleteQuery.AppendFormat("{0} = '{1}' AND ", deleteCondition[i], MysqlEscapeString(deleteDataCondition[i]));
            }
            deleteQuery.Remove(deleteQuery.Length - 4, 4); // Remove the last AND
            return deleteQuery.ToString();
        }
        public static string BuildBulkInsertQuery(string table, List<string> insertHeader, List<List<string>> data, bool onUpdate = false, string idName = "")
        {
            var insertHead = new StringBuilder();
            insertHead.AppendFormat("INSERT INTO `{0}` (", table);
            for (int i = 0; i < insertHeader.Count; i++)
            {
                insertHead.AppendFormat("`{0}`,", insertHeader[i]);
            }
            insertHead.Remove(insertHead.Length - 1, 1); // Remove the last ,
            insertHead.Append(") VALUES ");
            var insertQuery = new StringBuilder(insertHead.ToString());
            for (int i = 0; i < data.Count; i++)
            {
                // Add
                insertQuery.AppendFormat("(");
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] == "null")
                    {
                        insertQuery.AppendFormat("{0},", MysqlEscapeString(data[i][j]));
                    }
                    else
                    {
                        insertQuery.AppendFormat("'{0}',", MysqlEscapeString(data[i][j]));
                    }
                    //insertQuery.AppendFormat(data[i][j] == "null" ? "{0}," : "'{0}',", MysqlEscapeString(data[i][j]));
                }
                insertQuery.Remove(insertQuery.Length - 1, 1); // Remove the last ,
                insertQuery.Append("),");
            }
            insertQuery.Remove(insertQuery.Length - 2, 2); // Remove the last ,
            insertQuery.Append(")");
            if (onUpdate)
            {
                insertQuery.Append(" ON DUPLICATE KEY UPDATE ");
                for (int i = 0; i < insertHeader.Count; i++)
                {
                    insertQuery.AppendFormat("{0}=values({0}),", insertHeader[i]);
                }
                insertQuery.Remove(insertQuery.Length - 1, 1); // Remove the last ,
                if (!string.IsNullOrWhiteSpace(idName))
                    insertQuery.AppendFormat(" ,{0}=LAST_INSERT_ID({0})", idName);
            }
            insertQuery.Append(";");
            return insertQuery.ToString();
        }

        public static string BuildSelectQuery(string tableName, List<string> selectWhat, List<string> filterBy = null,
            List<string> filterValue = null, string sortby = null, bool semiColen = true)
        {
            var selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            for (int i = 0; i < selectWhat.Count; i++)
            {
                selectQuery.AppendFormat("{0},", selectWhat[i]);
            }
            selectQuery.Remove(selectQuery.Length - 1, 1);
            selectQuery.AppendFormat(" FROM {0}", tableName);
            if (filterBy != null && filterBy.Count > 0)
            {
                selectQuery.Append(" WHERE ");
                for (int i = 0; i < filterBy.Count; i++)
                {
                    selectQuery.AppendFormat("`{0}` = '{1}' AND ", filterBy[i], MysqlEscapeString(filterValue[i]));
                }
                selectQuery.Remove(selectQuery.Length - 4, 4); // Remove the last AND
            }
            if (!string.IsNullOrWhiteSpace(sortby))
            {
                selectQuery.AppendFormat(" ORDER BY {0}", sortby);
            }
            if (semiColen)
                selectQuery.Append(";");
            return selectQuery.ToString();
        }

        public static string BuildUpdateQuery(string tableName, List<string> updateWhat, List<string> updateData, List<string> filterBy,
            List<string> filterValue)
        {
            var updateQuery = new StringBuilder();
            updateQuery.AppendFormat("UPDATE {0} SET ", tableName);
            for (int i = 0; i < updateWhat.Count; i++)
            {
                updateQuery.AppendFormat(" `{0}`='{1}',", updateWhat[i], QueryHelper.MysqlEscapeString(updateData[i]));
            }
            updateQuery.Remove(updateQuery.Length - 1, 1);
            if (filterBy.Count > 0)
            {
                updateQuery.AppendFormat(" WHERE ");
                for (int i = 0; i < filterBy.Count; i++)
                {
                    updateQuery.AppendFormat("`{0}`='{1}' AND ", filterBy[i], QueryHelper.MysqlEscapeString(filterValue[i]));
                }
                updateQuery.Remove(updateQuery.Length - 4, 4); // Remove the last AND
            }
            updateQuery.Append(";");
            return updateQuery.ToString();
        }
        public static List<string> GenerateListIds(long lastId, int size)
        {
            var ids = new List<string>();
            for (var i = lastId - size; i < size; i++)
            {
                ids.Add(i.ToString());
            }
            return ids;
        }

        #endregion

        private static long QueryInsertUpdateDelete(string query, MySqlConnection connection, bool getLastId = true)
        {

            long lastInsert = 0;
            var command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            if (getLastId)
                lastInsert = command.LastInsertedId;
            command.Dispose();
            return lastInsert;
        }

        private static List<Dictionary<string, string>> QuerySelect(string query, MySqlConnection connection)
        {
            var value = new List<Dictionary<string, string>>();
            using (var command = new MySqlCommand(query, connection))
            {
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var rowValue = new object[result.FieldCount];
                        var v = result.GetValues(rowValue);
                        var dict = new Dictionary<string, string>();
                        for (int i = 0; i < rowValue.Length; i++)
                        {
                            dict.Add(result.GetName(i), rowValue[i].ToString());
                        }
                        value.Add(dict);
                    }
                }
            }
            return value;
        }

    }
}
