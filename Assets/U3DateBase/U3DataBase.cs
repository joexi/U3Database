using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class U3Database
{
    public string dataPath;

    private SqliteConnection sqlConnection;
    private SqliteTransaction sqlTrans;


    private static Dictionary<string, U3Database> dbPool = new Dictionary<string, U3Database>();
    public static U3Database DatabaseWithPath(string path)
    {
        U3Database db = null;
        if (dbPool.ContainsKey(path))
        {
            db = dbPool[path];
        }
        else
        {
            db = new U3Database(path);
            dbPool[path] = db;
        }
        return db;
    }

    public U3Database(string path)
    {
        U3DBLog.Log("alloc with path " + path);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            dataPath = String.Format("Data Source={0};", path);
        else
            dataPath = String.Format("URI=file:{0}", path);
        sqlConnection = new SqliteConnection(dataPath);
    }

    public bool Open()
    {
        if (sqlConnection != null)
        {
            sqlConnection.Open();
        }
        return true;
    }

    public bool Close()
    {
        if (sqlConnection != null)
        {
            sqlConnection.Close();
        }
        return true;
    }

    public SqliteCommand CreateCommand(string sql)
    {
        SqliteCommand qry = sqlConnection.CreateCommand();
        qry.CommandText = sql;
        qry.Transaction = sqlTrans;
        return qry;
    }

    public U3DBResultSet Select(string tabelName, string conditionFieldName, object conditionFieldValue)
    {
        string condition = string.Format("{0}={1}", conditionFieldName, conditionFieldValue);
        return Select(tabelName, condition);
    }

    public U3DBResultSet Select(string tabelName, string condition)
    {
        string sql = string.Format("SELECT * FROM {0} WHERE {1}", tabelName, condition ?? "1=1");
        return Select(sql);
    }

    public U3DBResultSet Select(string sql)
    {
        U3DBLog.Log("Select " + sql);

        SqliteCommand qry = CreateCommand(sql);
        try
        {
            SqliteDataReader reader = qry.ExecuteReader();
            U3DBResultSet result = new U3DBResultSet(this, reader);
            return result;
        }
        catch (Exception e)
        {
            U3DBLog.LogError(e.Message);
            return null;
        }
        return null;
    }

    public bool Delete(string tableName, string conditionFieldName, object conditionFieldValue)
    {
        string condition = string.Format("{0}={1}", conditionFieldName, conditionFieldValue);
        return Delete(tableName, condition);
    }

    public bool Delete(string tableName, string condition)
    {
        string sql = string.Format("DELETE FROM {0} WHERE {1}", tableName, condition);
        return Delete(sql);
    }

    public bool Delete(string sql)
    {
        SqliteCommand qry = CreateCommand(sql);
        try
        {
            SqliteDataReader reader = qry.ExecuteReader();
            return true;
        }
        catch (Exception e)
        {
            U3DBLog.LogError(e.Message);
            return false;
        }
        return false;
    }

    public bool Update(string tabelName, string fieldName, object fieldValue, string conditionFieldName, object conditionFieldValue)
    {
        string condition = string.Format("{0}={1}", conditionFieldName, conditionFieldValue);
        return Update(tabelName, fieldName, fieldValue, condition);
    }

    public bool Update(string tabelName, string fieldName, object fieldValue, string condition = null)
    {
        string sql = string.Format("UPDATE {0} SET {1}={2} WHERE {3}", tabelName, fieldName, fieldValue, condition ?? "1=1");
        return Update(sql);
    }

    public bool Update(string sql)
    {
        U3DBLog.Log("Update " + sql);
        SqliteCommand qry = CreateCommand(sql);
        try
        {
            SqliteDataReader reader = qry.ExecuteReader();
        }
        catch (Exception e)
        {
            U3DBLog.LogError(e.Message);
            return false;
        }
        return true;
    }


    public bool Insert(string tableName, string key1, object value1)
    {
        return Insert(tableName, key1, value1, null, null, null, null);
    }

    public bool Insert(string tableName, string key1, object value1, string key2, object value2)
    {
        return Insert(tableName, key1, value1, key2, value2, null, null);
    }

    public bool Insert(string tableName, string key1, object value1, string key2, object value2, string key3, object value3)
    {
        string key = combineKeys(key1, key2, key3);
        string value = combineValues(value1, value2, value3);
        string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, key, value);
        return Insert(sql);
    }


    private string combineKeys(params string[] keys)
    {
        string result = string.Empty;
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != null)
            {
                result += (result.Length > 0 ? "," : "") + keys[i];
            }
        }
        return result;
    }

    private string combineValues(params object[] values)
    {
        string result = string.Empty;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] != null)
            {
                result += (result.Length > 0 ? "," : "") + parseValue(values[i]);
            }
        }
        return result;
    }

    private string parseValue(object value)
    {
        if (value is string)
        {
            return string.Format("\"{0}\"", value);
        }
        return value.ToString();
    }

    public bool Insert(string sql)
    {
        U3DBLog.Log("Insert " + sql);
        SqliteCommand qry = CreateCommand(sql);
        try
        {
            SqliteDataReader reader = qry.ExecuteReader();
        }
        catch (Exception e)
        {
            U3DBLog.LogError(e.Message);
            return false;
        }
        return true;
    }

    public bool Execute(string sql)
    {
        return false;
    }

    public delegate void TransactionCallback(ref bool rollback);

    public void BeginTransaction(TransactionCallback cb)
    {
        U3DBLog.Log("beign transaction");
        if (cb != null)
        {
            sqlTrans = sqlConnection.BeginTransaction();
            bool rollback = false;
            cb(ref rollback);
            if (rollback)
            {
                sqlTrans.Rollback();
                U3DBLog.Log("transaction rollback!");
            }
            else
            {
                sqlTrans.Commit();
                U3DBLog.Log("transaction commit!");
            }
            sqlTrans = null;
        }
    }
}
