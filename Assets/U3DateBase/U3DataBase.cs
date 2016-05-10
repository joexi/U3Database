using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class U3Database
{
    private SqliteConnection sqlConnection;
    private SqliteTransaction sqlTrans;
    private string dataPath;

    public static U3Database DatabaseWithPath(string path)
    {
		
        U3Database db = new U3Database(path);
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

    public SqliteCommand CreateCommand(string sql) {
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

    public bool Delete(string sql)
    {
        U3DBLog.Log("Delete " + sql);
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
    public void BeginTransaction(TransactionCallback cb) {
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
