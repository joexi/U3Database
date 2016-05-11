# U3Database
A Unity3D/C# wrapper around SQLite 

## PROVIDE
* Read data from sqlite / Write data into sqlite
* TODO
	* Generate data template depend on sqlite table structure
	* Deserialize sql result into object

## HOW TO USE
### UPDATE
``` c#
U3Database db = U3Database.DatabaseWithPath(Application.dataPath + "/test.db");
db.Open();
db.Update("UPDATE Main SET Value=3 WHERE ID=1");
db.Update("Main", "Value", 5);
db.Update("Main", "Value", 6, "ID=1");
db.Update("Main", "Value", 9, "ID", 2);
```

### SELECT
```c#
U3DBResultSet result = null;
result = db.Select("SELECT * FROM MAIN");
result.Show();
result = db.Select("Main", "ID", 1);
result.Show();
```

### INSERT
```c#
db.Insert("Main", "Value", 888);
db.Insert("Main", "Value", 999, "Description", "???");
```

### TRANSACTION
```c#
db.BeginTransaction(delegate(ref bool rollback)
{
    db.Update("Main", "Value", 0);
    rollback = true;
});
```

## REQUIRE
* Unity 3D 4.x/5.x


## REFERENCE

* [FMDB](https://github.com/ccgus/fmdb)