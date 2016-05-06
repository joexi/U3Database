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

    public U3DBResultSet Select(string sql)
    {
        U3DBLog.Log("Select " + sql);

        SqliteCommand qry = sqlConnection.CreateCommand();
        qry.CommandText = sql;
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
        SqliteCommand qry = sqlConnection.CreateCommand();
        qry.CommandText = sql;
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
        SqliteCommand qry = sqlConnection.CreateCommand();
        qry.CommandText = sql;
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
        SqliteCommand qry = sqlConnection.CreateCommand();
        qry.CommandText = sql;
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
}
