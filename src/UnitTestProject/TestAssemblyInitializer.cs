using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class  TestAssemblyInitializer
    {
        //Данный класс нужен для инициализации базы данных
        [AssemblyInitialize]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Удалите неиспользуемый параметр", Justification = "<Ожидание>")]
        public static void Initialize(TestContext context)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());
        }
    }
}
