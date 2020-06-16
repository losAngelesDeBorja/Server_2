using System;
using System.Collections.Generic;
using adm;
using System.IO;

namespace adm
{
    public class Database
    {/*
        //Work with databases
        public const string CreateDataBaseSuccess = "DataBase created";
        public const string UpdateDataBaseSuccess = "DataBase updated";
        public const string WrongSyntax = Error + "Syntactical error";
        public const string CreateDatabaseSuccess = "Database created";
        public const string OpenDatabaseSuccess = "Database opened";
        public const string DeleteDatabaseSuccess = "Database deleted";
        public const string BackupDatabaseSuccess = "Database backed up";
        public const string SecurityProfileCreated = "Security profile created";
        public const string SecurityUserCreated = "Security user created";
        public const string SecurityProfileDeleted = "Security profile deleted";
        public const string SecurityUserDeleted = "Security user deleted";
        public const string SecurityPrivilegeGranted = "Security privilege granted";
        public const string SecurityPrivilegeRevoked = "Security privilege revoked";
        public const string SecurityUserAdded = "User added to security profile";
        public const string Error = "ERROR: ";
        public const string DatabaseDoesNotExist = Error + "Database does not exist";
        public const string IncorrectDataType = Error + "Incorrect data type";
        public const string AddedTableSuccess = "Table added successfully";
        public Database db;
        public string dbName;
        public List<Table> listTable = new List<Table>() { };

        public Database()
        {
        }
        public Database(string myDatabaseName, string username, string password)
        {
            dbName = myDatabaseName;
        }
        public List<Table> SelectAllTables(string dbname)
        {
            return listTable;
        }
        public string createDatabase(string myDatabase, string username, string password)
        {
            try
            {
                db = new Database(myDatabase, username, password);

                //TODO save database into disk as TXT or XML file
                return CreateDatabaseSuccess;
            }
            catch
            {
                return Error;
            }
        }
        public string updateDatabase(string myDatabase, string username, string password)
        {

            try
            {
                //TODO load database from disk 
                //TODO update database on Database object 
                //TODO update apply database changes into disk storage
                return UpdateDataBaseSuccess;
            }
            catch
            {
                return Error;
            }
        }
        public string deleteDatabase(string myDatabase, string username, string password)
        {
            try
            {
                //TODO load database from disk 
                return CreateDatabaseSuccess;
            }
            catch
            {
                return Error;
            }
        }
        public void createDatabaseByCommand(string sql)
        {
            //TODO Read sql sentence. Identify its CREATE word and create the database (use Parser)
        }
        public void updateDatabaseByCommand(string sql)
        {
            //TODO Read sql sentence. Identify its UPDATE word and update the database (use Parser)
        }
        public void deleteDatabaseByCommand(string sql)
        {
            //TODO Read sql sentence. Identify its DELETE word and update the database (use Parser)
        }
        public void executeSQLByCommand(string sql, Table table)
        {
            //Select sql query
            ParserSQL parseSQLToTable = new ParserSQL();
            Table selectToTable = parseSQLToTable.parserSentenceSQL(sql, table);

        }
        public Table executeSQLByCommandReturnResult(string sql, Table table)
        {
            //Select sql query
            ParserSQL parseSQLToTable = new ParserSQL();
            Table resultToTable = parseSQLToTable.parserSentenceSQL(sql, table);
            return resultToTable;
        }
        public string addTable(Table newTable, string existingDbName)
        {
            try
            {
                listTable.Add(newTable);
                return AddedTableSuccess;
            }
            catch
            {
                return Error;
            }
        }
        public string useDB(string nameDB)
        {
            //TODO
            return "null";
        }
        */
    }
}
