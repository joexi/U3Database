using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class U3DBQueue {
    public U3Database database;
    public delegate void U3DBQueueCallback(U3Database db);


    private static object lockObj = new object();


    public static U3DBQueue DatabaseWithPath(string path)
    {
        U3Database db = U3Database.DatabaseWithPath(path);
        U3DBQueue queue = new U3DBQueue(db);
        return queue;
    }

    public U3DBQueue(U3Database db) {
        database = db;
    }

    public void Dispath(U3DBQueueCallback cb) {
        lock(lockObj) {
            if (cb != null)
            {
                cb(database);
            }
        }
    }
}
