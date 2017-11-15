using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using sqlite3Mt4;

namespace sqlite3Mt4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_openSqlite3Connection()
        {
            SQLite3Mt4.openSqlite3Connection("D:\\app\\plugins4Mt4\\sqlite3Mt4\\test.db");
        }

        [TestMethod]
        public void Test_executeCreateTable()
        {
            int connectionHandler = SQLite3Mt4.openSqlite3Connection("D:\\app\\plugins4Mt4\\sqlite3Mt4\\test.db");
            SQLite3Mt4.execute(connectionHandler, "create table highscores (name varchar(20), score int)");
        }

        [TestMethod]
        public void Test_executeInsert()
        {
            int connectionHandler = SQLite3Mt4.openSqlite3Connection("D:\\app\\plugins4Mt4\\sqlite3Mt4\\test.db");
            string result = SQLite3Mt4.execute(connectionHandler, "insert into highscores (name, score) values ('Me', 3000)");
            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void Test_executeQuery()
        {
            int connectionHandler = SQLite3Mt4.openSqlite3Connection("D:\\app\\plugins4Mt4\\sqlite3Mt4\\test.db");
            int readerHandler = SQLite3Mt4.query(connectionHandler, "select * from highscores order by score desc");

            int hasNext = SQLite3Mt4.step(readerHandler);
            while (hasNext == 1) {
                string name = SQLite3Mt4.getColumnAsString(readerHandler, "name");
                Console.Out.WriteLine(name);
                hasNext = SQLite3Mt4.step(readerHandler);
            }

            Assert.AreEqual(0, 1);
        }
    }
}
