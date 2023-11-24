using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro
{
    public class AppSettings
    {
        private static string DataBaseName = "database.db";
        private static string DataBaseDirectory = FileSystem.AppDataDirectory; //o sistema operacional via me dizer onde eu posos armazenar os dados
        public static string DataBasePath = Path.Combine(DataBaseDirectory, DataBaseName); //concatenação do path. ex: C:/dasd/adadad/database.db

    }
}
