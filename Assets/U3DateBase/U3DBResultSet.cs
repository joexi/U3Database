using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class U3DBResultSet
{

    public U3Database ParentDB;
    public SqliteDataReader Reader;
    public string Query;

    public U3DBResultSet(U3Database parentDB, SqliteDataReader reader)
    {
        ParentDB = parentDB;
        Reader = reader;
    }

    public bool Next()
    {
        return Reader.Read();
    }

    public void Show()
    {
        while (Reader.Read())
        {
            for (int i = 0; i < Reader.FieldCount; i++)
            {
                Debug.Log(Reader.GetName(i) + " " + Reader.GetValue(i));
            }
        }
    }

    public List<T> Deserialize<T>() where T : class
    {
        return null;
    }
}
