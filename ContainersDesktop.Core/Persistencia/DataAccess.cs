using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Windows.Media.Playlists;
using Windows.Storage;

namespace ContainersDesktop.Core.Persistencia;
public static class DataAccess
{
    public static async void InicializarBase()
    {
        await ApplicationData.Current.LocalFolder.CreateFileAsync("Containers.db", CreationCollisionOption.OpenIfExists);
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using SqliteConnection db = new SqliteConnection($"Filename={dbpath}");
        await db.OpenAsync();

        try
        {
            #region ClaList
            var claListTableCommand = "CREATE TABLE IF NOT EXISTS CLALIST (CLALIST_ID_REG INTEGER NOT NULL UNIQUE, CLALIST_ID_ESTADO_REG TEXT NOT NULL DEFAULT 'A', CLALIST_DESCRIP TEXT NOT NULL DEFAULT '.', PRIMARY KEY(CLALIST_ID_REG));";

            SqliteCommand claListCreateTable = new SqliteCommand(claListTableCommand, db);

            await claListCreateTable.ExecuteReaderAsync();

            SqliteCommand selectCommand = new SqliteCommand("SELECT CLALIST_ID_REG, CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP FROM CLALIST", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();
            if (!query.HasRows)
            {
                var clasListLista = new List<ClaList>
            {
                new ClaList() { CLALIST_ID_REG = 0, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "" },
                new ClaList() { CLALIST_ID_REG = 1000, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "SIGLAS" },
                new ClaList() { CLALIST_ID_REG = 1100, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "MODELO" },
                new ClaList() { CLALIST_ID_REG = 1200, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "VARIANTE" },
                new ClaList() { CLALIST_ID_REG = 1300, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "TIPO" },
                new ClaList() { CLALIST_ID_REG = 1400, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "PROPIETARIO" },
                new ClaList() { CLALIST_ID_REG = 1500, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "TARA" },
                new ClaList() { CLALIST_ID_REG = 1600, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "PMP" },
                new ClaList() { CLALIST_ID_REG = 1700, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "ALTURA EXTERIOR" },
                new ClaList() { CLALIST_ID_REG = 1800, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "CUELLO CISNE" },
                new ClaList() { CLALIST_ID_REG = 1900, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "BARRAS" },
                new ClaList() { CLALIST_ID_REG = 2000, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "CABLES" },
                new ClaList() { CLALIST_ID_REG = 2100, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "LINEA VIDA" },
                new ClaList() { CLALIST_ID_REG = 3000, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "TIPO MOVIMIENTO" },
                new ClaList() { CLALIST_ID_REG = 3100, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "PESO " },
                new ClaList() { CLALIST_ID_REG = 3200, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "TRANSPORTISTA" },
                new ClaList() { CLALIST_ID_REG = 3300, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "CLIENTE" },
                new ClaList() { CLALIST_ID_REG = 3400, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "CHOFER" },
                new ClaList() { CLALIST_ID_REG = 3500, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "ENTRADA SALIDA" },
                new ClaList() { CLALIST_ID_REG = 3600, CLALIST_ID_ESTADO_REG = "A", CLALIST_DESCRIP = "ALMACEN" },
            };

                var insertCmd = "INSERT INTO CLALIST (CLALIST_ID_REG, CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP) " +
                    " VALUES(@CLALIST_ID_REG, @CLALIST_ID_ESTADO_REG, @CLALIST_DESCRIP)";
                foreach (ClaList claList in clasListLista)
                {
                    using (var cmd = new SqliteCommand(insertCmd, db))
                    {
                        cmd.Parameters.AddWithValue("@CLALIST_ID_REG", claList.CLALIST_ID_REG);
                        cmd.Parameters.AddWithValue("@CLALIST_ID_ESTADO_REG", claList.CLALIST_ID_ESTADO_REG);
                        cmd.Parameters.AddWithValue("@CLALIST_DESCRIP", claList.CLALIST_DESCRIP);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            #endregion

            #region Listas
            var listasTableCommand = "CREATE TABLE IF NOT EXISTS LISTAS (LISTAS_ID_REG INTEGER NOT NULL UNIQUE, " +
            "LISTAS_ID_ESTADO_REG TEXT NOT NULL DEFAULT 'A', " +
            "LISTAS_ID_LISTA INTEGER NOT NULL DEFAULT 0, " +
            "LISTAS_ID_LISTA_ORDEN INTEGER NOT NULL DEFAULT 0, " +
            "LISTAS_ID_LISTA_DESCRIP TEXT NOT NULL DEFAULT ''," +
            "FOREIGN KEY(LISTAS_ID_LISTA) REFERENCES CLALIST (CLALIST_ID_REG), PRIMARY KEY(LISTAS_ID_REG AUTOINCREMENT));";

            SqliteCommand listasCreateTable = new SqliteCommand(listasTableCommand, db);

            await listasCreateTable.ExecuteReaderAsync();

            SqliteCommand selectCommandListas = new SqliteCommand("SELECT LISTAS_ID_REG FROM LISTAS", db);

            SqliteDataReader queryListas = await selectCommandListas.ExecuteReaderAsync();
            if (!queryListas.HasRows)
            {
                var listas = new List<Listas>
                {
                    new Listas() { LISTAS_ID_REG = 1, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 0, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "" },
                    new Listas() { LISTAS_ID_REG = 2, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1000, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 3, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1100, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 4, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1200, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 5, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1300, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 6, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1400, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 7, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1500, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 8, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1600, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 9, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1700, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 10, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1800, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 11, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 1900, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 12, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 2000, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 13, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 2100, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 14, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3000, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 15, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3100, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 16, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3200, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 17, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3300, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 18, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3400, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 19, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3500, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                    new Listas() { LISTAS_ID_REG = 20, LISTAS_ID_ESTADO_REG = "A", LISTAS_ID_LISTA = 3600, LISTAS_ID_LISTA_ORDEN = 0, LISTAS_ID_LISTA_DESCRIP = "NO ASIGNADO" },
                };

                var insertCmd = "INSERT INTO LISTAS (LISTAS_ID_REG, LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP) " +
                    " VALUES(@LISTAS_ID_REG, @LISTAS_ID_ESTADO_REG, @LISTAS_ID_LISTA, @LISTAS_ID_LISTA_ORDEN ,@LISTAS_ID_LISTA_DESCRIP)";
                foreach (Listas lista in listas)
                {
                    using (var cmd = new SqliteCommand(insertCmd, db))
                    {
                        cmd.Parameters.AddWithValue("@LISTAS_ID_REG", lista.LISTAS_ID_REG);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_ESTADO_REG", lista.LISTAS_ID_ESTADO_REG);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            #endregion Listas

            #region Objetos
            var objetosTableCommand = "CREATE TABLE IF NOT EXISTS OBJETOS (" +
                "OBJ_ID_REG INTEGER NOT NULL UNIQUE," +
                "OBJ_MATRICULA TEXT NOT NULL UNIQUE," +
                "OBJ_ID_ESTADO_REG TEXT NOT NULL DEFAULT 'A'," +
                "OBJ_SIGLAS_LISTA  INTEGER NOT NULL DEFAULT 1000," +
                "OBJ_SIGLAS  INTEGER NOT NULL DEFAULT 1," +
                "OBJ_MODELO_LISTA  INTEGER NOT NULL DEFAULT 1100," +
                "OBJ_MODELO    INTEGER NOT NULL DEFAULT 1," +
                "OBJ_ID_OBJETO NUMERIC NOT NULL DEFAULT 0," +
                "OBJ_VARIANTE_LISTA    INTEGER NOT NULL DEFAULT 1200," +
                "OBJ_VARIANTE  INTEGER NOT NULL DEFAULT 1," +
                "OBJ_TIPO_LISTA    INTEGER NOT NULL DEFAULT 1300," +
                "OBJ_TIPO  INTEGER NOT NULL DEFAULT 1," +
                "OBJ_INSPEC_CSC    TEXT NOT NULL DEFAULT 'AAAA-MM-DD'," +
                "OBJ_PROPIETARIO_LISTA INTEGER NOT NULL DEFAULT 1400," +
                "OBJ_PROPIETARIO   INTEGER NOT NULL DEFAULT 1," +
                "OBJ_TARA_LISTA    INTEGER NOT NULL DEFAULT 1500," +
                "OBJ_TARA INTEGER NOT NULL DEFAULT 1," +
                "OBJ_PMP_LISTA INTEGER NOT NULL DEFAULT 1600," +
                "OBJ_PMP  INTEGER NOT NULL DEFAULT 1," +
                "OBJ_CARGA_UTIL  INTEGER NOT NULL DEFAULT 0," +
                "OBJ_ALTURA_EXTERIOR_LISTA INTEGER NOT NULL DEFAULT 1700," +
                "OBJ_ALTURA_EXTERIOR   INTEGER NOT NULL DEFAULT 1," +
                "OBJ_CUELLO_CISNE_LISTA    INTEGER NOT NULL DEFAULT 1800," +
                "OBJ_CUELLO_CISNE  INTEGER NOT NULL DEFAULT 1," +
                "OBJ_BARRAS_LISTA  INTEGER NOT NULL DEFAULT 1900," +
                "OBJ_BARRAS    INTEGER NOT NULL DEFAULT 1," +
                "OBJ_CABLES_LISTA  INTEGER NOT NULL DEFAULT 2000," +
                "OBJ_CABLES    INTEGER NOT NULL DEFAULT 1," +
                "OBJ_LINEA_VIDA_LISTA  INTEGER NOT NULL DEFAULT 2100," +
                "OBJ_LINEA_VIDA    INTEGER NOT NULL DEFAULT 1," +
                "OBJ_OBSERVACIONES TEXT NOT NULL DEFAULT '.'," +
                "PRIMARY KEY(OBJ_ID_REG AUTOINCREMENT)," +
                "FOREIGN KEY(OBJ_MODELO) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_TIPO) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_VARIANTE) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_TARA) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_PROPIETARIO) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_CUELLO_CISNE) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_ALTURA_EXTERIOR) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_PMP) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_BARRAS) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_CABLES) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_LINEA_VIDA) REFERENCES LISTAS(LISTAS_ID_REG)," +
                "FOREIGN KEY(OBJ_SIGLAS) REFERENCES LISTAS(LISTAS_ID_REG));";

            SqliteCommand objetosCreateTable = new SqliteCommand(objetosTableCommand, db);

            await objetosCreateTable.ExecuteReaderAsync();

            //db.Open();

            //SqliteCommand insertCommand = new SqliteCommand();
            //insertCommand.Connection = db;

            //insertCommand.CommandText = "INSERT INTO OBJETOS VALUES (@OBJ_ID_REG, @OBJ_MATRICULA, @OBJ_ID_ESTADO_REG, @OBJ_SIGLAS_LISTA, @OBJ_SIGLAS, @OBJ_MODELO_LISTA, " +
            //    "@OBJ_MODELO, @OBJ_ID_OBJETO, @OBJ_VARIANTE_LISTA, @OBJ_VARIANTE, @OBJ_TIPO_LISTA, @OBJ_TIPO, @OBJ_INSPEC_CSC, @OBJ_PROPIETARIO_LISTA, @OBJ_PROPIETARIO, " +
            //    "@OBJ_TARA_LISTA, @OBJ_TARA, @OBJ_PMP_LISTA, @OBJ_PMP, @OBJ_CARGA_UTIL, @OBJ_ALTURA_EXTERIOR_LISTA, @OBJ_ALTURA_EXTERIOR, @OBJ_CUELLO_CISNE_LISTA, " +
            //    "@OBJ_CUELLO_CISNE, @OBJ_BARRAS_LISTA, @OBJ_BARRAS, @OBJ_CABLES_LISTA, @OBJ_CABLES, @OBJ_LINEA_VIDA_LISTA, @OBJ_LINEA_VIDA, @OBJ_OBSERVACIONES);";
            //insertCommand.Parameters.AddWithValue("@OBJ_ID_REG", objeto.OBJ_ID_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_MATRICULA", objeto.OBJ_MATRICULA);
            //insertCommand.Parameters.AddWithValue("@OBJ_ID_ESTADO_REG", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_SIGLAS_LISTA", objeto.OBJ_SIGLAS_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_SIGLAS", objeto.OBJ_SIGLAS);
            //insertCommand.Parameters.AddWithValue("@OBJ_MODELO_LISTA", objeto.OBJ_MODELO_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_MODELO", objeto.OBJ_TARA_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_ID_OBJETO", objeto.OBJ_TARA);
            //insertCommand.Parameters.AddWithValue("@OBJ_VARIANTE_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_VARIANTE", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_TIPO_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_TIPO", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_INSPEC_CSC", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_TARA_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_TARA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_PMP_LISTA", objeto.OBJ_MATRICULA);
            //insertCommand.Parameters.AddWithValue("@OBJ_PMP", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_CARGA_UTIL", objeto.OBJ_SIGLAS_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR_LISTA", objeto.OBJ_SIGLAS);
            //insertCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR", objeto.OBJ_MODELO_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE_LISTA", objeto.OBJ_TARA_LISTA);
            //insertCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE", objeto.OBJ_TARA);
            //insertCommand.Parameters.AddWithValue("@OBJ_BARRAS_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_BARRAS", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_CABLES_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_CABLES", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA_LISTA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA", objeto.OBJ_ID_ESTADO_REG);
            //insertCommand.Parameters.AddWithValue("@OBJ_OBSERVACIONES", objeto.OBJ_ID_ESTADO_REG);

            //await insertCommand.ExecuteReaderAsync();
            #endregion
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
        finally 
        {   
            await db.CloseAsync();
        }
        
    }
}
