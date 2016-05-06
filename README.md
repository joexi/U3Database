# U3Database
A Unity3D/C# wrapper around SQLite 

## PROVIDE
* Read data from sqlite / Write data into sqlite
* TODO
	* Generate data template depend on sqlite table structure
	* Deserialize sql result into object

## HOW TO USE
```
U3Database db = U3Database.DatabaseWithPath(Application.dataPath + "/test.db");
db.Open();
db.Update("UPDATE Main SET Value=3 WHERE ID=1");
db.Update("Main", "Value", 5);
db.Update("Main", "Value", 6, "ID=1");
db.Update("Main", "Value", 9, "ID", 2);
```

## REQUIRE
* Unity 3D 4.x/5.x


## REFERENCE

* [FMDB](https://github.com/ccgus/fmdb)