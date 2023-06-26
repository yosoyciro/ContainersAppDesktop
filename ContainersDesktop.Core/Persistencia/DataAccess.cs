using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Persistencia;
public static class DataAccess
{
    //private readonly IConfiguration _configuration;

    //public DataAccess(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //}

    public static async void InicializarBase()
    {
        await ApplicationData.Current.LocalFolder.CreateFileAsync("Containers.db", CreationCollisionOption.OpenIfExists);
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using SqliteConnection db = new SqliteConnection($"Filename={dbpath}");
        await db.OpenAsync();

        try
        {
            #region ClaList

            var claListTableCommand = "CREATE TABLE IF NOT EXISTS CLALIST (CLALIST_ID_REG INTEGER NOT NULL UNIQUE, " +
                " CLALIST_ID_ESTADO_REG TEXT NOT NULL DEFAULT 'A', " +
                " CLALIST_DESCRIP TEXT NOT NULL DEFAULT '.', " +
                " CLALIST_FECHA_ACTUALIZACION TEXT NOT NULL DEFAULT '.'," +
                " PRIMARY KEY(CLALIST_ID_REG));";

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

                var insertCmd = "INSERT INTO CLALIST (CLALIST_ID_REG, CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP, CLALIST_FECHA_ACTUALIZACION) " +
                    " VALUES(@CLALIST_ID_REG, @CLALIST_ID_ESTADO_REG, @CLALIST_DESCRIP, @CLALIST_FECHA_ACTUALIZACION)";
                foreach (ClaList claList in clasListLista)
                {
                    using (var cmd = new SqliteCommand(insertCmd, db))
                    {
                        cmd.Parameters.AddWithValue("@CLALIST_ID_REG", claList.CLALIST_ID_REG);
                        cmd.Parameters.AddWithValue("@CLALIST_ID_ESTADO_REG", claList.CLALIST_ID_ESTADO_REG);
                        cmd.Parameters.AddWithValue("@CLALIST_DESCRIP", claList.CLALIST_DESCRIP);
                        cmd.Parameters.AddWithValue("@CLALIST_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());
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
            "LISTAS_FECHA_ACTUALIZACION TEXT NOT NULL DEFAULT '.'," +
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

                var insertCmd = "INSERT INTO LISTAS (LISTAS_ID_REG, LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION) " +
                    " VALUES(@LISTAS_ID_REG, @LISTAS_ID_ESTADO_REG, @LISTAS_ID_LISTA, @LISTAS_ID_LISTA_ORDEN ,@LISTAS_ID_LISTA_DESCRIP, @LISTAS_FECHA_ACTUALIZACION)";
                foreach (Listas lista in listas)
                {
                    using (var cmd = new SqliteCommand(insertCmd, db))
                    {
                        cmd.Parameters.AddWithValue("@LISTAS_ID_REG", lista.LISTAS_ID_REG);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_ESTADO_REG", lista.LISTAS_ID_ESTADO_REG);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
                        cmd.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
                        cmd.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());
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
                "OBJ_FECHA_ACTUALIZACION TEXT NOT NULL DEFAULT '.'," +
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

            #endregion

            #region Movimientos

            var movimientosTableCommand = "CREATE TABLE IF NOT EXISTS MOVIM (MOVIM_ID_REG INTEGER NOT NULL UNIQUE, " +
                "MOVIM_ID_REG_MOBILE INTEGER NOT NULL, " +
                "MOVIM_ID_ESTADO_REG   TEXT NOT NULL DEFAULT 'A', " +
	            "MOVIM_FECHA TEXT NOT NULL DEFAULT 'AAAA-MM-DD', "                               +
	            "MOVIM_ID_OBJETO  INTEGER NOT NULL DEFAULT 1, "                                     +
	            "MOVIM_TIPO_MOVIM_LISTA  INTEGER NOT NULL DEFAULT 3000, "                         +
	            "MOVIM_TIPO_MOVIM  INTEGER NOT NULL DEFAULT 1,       "                              +
	            "MOVIM_PESO_LISTA  INTEGER NOT NULL DEFAULT 3100,    "                              +
	            "MOVIM_PESO  INTEGER NOT NULL DEFAULT 1,     "                                    +
	            "MOVIM_TRANSPORTISTA_LISTA INTEGER NOT NULL DEFAULT 3200, "                         +
	            "MOVIM_TRANSPORTISTA   INTEGER NOT NULL DEFAULT 1, "                                +
	            "MOVIM_CLIENTE_LISTA   INTEGER NOT NULL DEFAULT 3300, "                             +
	            "MOVIM_CLIENTE INTEGER NOT NULL DEFAULT 1,  "                                       +
	            "MOVIM_CHOFER_LISTA   INTEGER NOT NULL DEFAULT 3400, "                             +
	            "MOVIM_CHOFER INTEGER NOT NULL DEFAULT 1,  "                                       +
	            "MOVIM_CAMION_ID   TEXT NOT NULL DEFAULT '.', "                                      +
	            "MOVIM_REMOLQUE_ID TEXT NOT NULL DEFAULT '.', "                                     +
	            "MOVIM_ALBARAN_ID  TEXT NOT NULL DEFAULT '.', "                                     +
	            "MOVIM_OBSERVACIONES   TEXT NOT NULL DEFAULT '.', "                                 +
	            "MOVIM_ENTRADA_SALIDA_LISTA    INTEGER NOT NULL DEFAULT 3500, "                     +
	            "MOVIM_ENTRADA_SALIDA  INTEGER NOT NULL DEFAULT 1, "                                +
	            "MOVIM_ALMACEN_LISTA   INTEGER NOT NULL DEFAULT 3600, "                             +
	            "MOVIM_ALMACEN INTEGER NOT NULL DEFAULT 1, "                                        +
	            "MOVIM_PDF TEXT NOT NULL DEFAULT '.',  "                                            +
                "MOVIM_FECHA_ACTUALIZACION TEXT NOT NULL DEFAULT '.', " +
	            "FOREIGN KEY(MOVIM_CLIENTE) REFERENCES LISTAS(LISTAS_ID_REG), "                +
	            "FOREIGN KEY(MOVIM_CHOFER) REFERENCES LISTAS(LISTAS_ID_REG), "                 +
	            "FOREIGN KEY(MOVIM_ALMACEN) REFERENCES LISTAS(LISTAS_ID_REG), "                +
	            "FOREIGN KEY(MOVIM_ENTRADA_SALIDA) REFERENCES LISTAS(LISTAS_ID_REG), "         +
	            "FOREIGN KEY(MOVIM_TRANSPORTISTA) REFERENCES LISTAS(LISTAS_ID_REG), "          +
	            "FOREIGN KEY(MOVIM_PESO) REFERENCES LISTAS(LISTAS_ID_REG),  "                  +
	            "FOREIGN KEY(MOVIM_ID_OBJETO) REFERENCES OBJETOS(OBJ_ID_REG), "                 +
	            "PRIMARY KEY(MOVIM_ID_REG AUTOINCREMENT), "                                         +
	            "FOREIGN KEY(MOVIM_TIPO_MOVIM) REFERENCES LISTAS(LISTAS_ID_REG))";

            SqliteCommand movimientosCreateTable = new SqliteCommand(movimientosTableCommand, db);

            await movimientosCreateTable.ExecuteReaderAsync();

            //SqliteCommand movimientosSelectCommand = new SqliteCommand("SELECT MOVIM_ID_REG FROM MOVIM", db);

            //SqliteDataReader movimientosQuery = await movimientosSelectCommand.ExecuteReaderAsync();
            //if (!movimientosQuery.HasRows)
            //{
            //    // Crear una lista de objetos Movim
            //    List<Movim> listaMovim = new List<Movim>();
            //    // Agregar objetos a la lista
            //    listaMovim.Add(new Movim() { MOVIM_ID_ESTADO_REG = "A",MOVIM_FECHA = "2023-06-09", MOVIM_ID_OBJETO = 1,MOVIM_TIPO_MOVIM_LISTA = 3000,MOVIM_TIPO_MOVIM = 28,MOVIM_PESO_LISTA = 3100,MOVIM_PESO = 1,MOVIM_TRANSPORTISTA_LISTA = 3200,MOVIM_TRANSPORTISTA = 1,MOVIM_CLIENTE_LISTA = 3300,MOVIM_CLIENTE = 1,MOVIM_CHOFER_LISTA = 3400,MOVIM_CHOFER = 1,MOVIM_CAMION_ID = "Camion 1",MOVIM_REMOLQUE_ID = "Remolque 1",MOVIM_ALBARAN_ID = "Albaran 1",MOVIM_OBSERVACIONES = "Observaciones 1",MOVIM_ENTRADA_SALIDA_LISTA = 3500,MOVIM_ENTRADA_SALIDA = 1,MOVIM_ALMACEN_LISTA = 3600,MOVIM_ALMACEN = 1,MOVIM_PDF = "" });
            //    listaMovim.Add(new Movim() { MOVIM_ID_ESTADO_REG = "A", MOVIM_FECHA = "2023-06-09", MOVIM_ID_OBJETO = 1, MOVIM_TIPO_MOVIM_LISTA = 3000, MOVIM_TIPO_MOVIM = 28, MOVIM_PESO_LISTA = 3100, MOVIM_PESO = 1, MOVIM_TRANSPORTISTA_LISTA = 3200, MOVIM_TRANSPORTISTA = 1, MOVIM_CLIENTE_LISTA = 3300, MOVIM_CLIENTE = 1, MOVIM_CHOFER_LISTA = 3400, MOVIM_CHOFER = 1, MOVIM_CAMION_ID = "Camion 1", MOVIM_REMOLQUE_ID = "Remolque 1", MOVIM_ALBARAN_ID = "Albaran 1", MOVIM_OBSERVACIONES = "Observaciones 1", MOVIM_ENTRADA_SALIDA_LISTA = 3500, MOVIM_ENTRADA_SALIDA = 1, MOVIM_ALMACEN_LISTA = 3600, MOVIM_ALMACEN = 1, MOVIM_PDF = "" });
            //    listaMovim.Add(new Movim() { MOVIM_ID_ESTADO_REG = "A",MOVIM_FECHA = "2023-06-09", MOVIM_ID_OBJETO = 2,MOVIM_TIPO_MOVIM_LISTA = 3000,MOVIM_TIPO_MOVIM = 29,MOVIM_PESO_LISTA = 3100,MOVIM_PESO = 1,MOVIM_TRANSPORTISTA_LISTA = 3200,MOVIM_TRANSPORTISTA = 1,MOVIM_CLIENTE_LISTA = 3300,MOVIM_CLIENTE = 1,MOVIM_CHOFER_LISTA = 3400,MOVIM_CHOFER = 1,MOVIM_CAMION_ID = "Camion 2",MOVIM_REMOLQUE_ID = "Remolque 2",MOVIM_ALBARAN_ID = "Albaran 2",MOVIM_OBSERVACIONES = "Observaciones 2",MOVIM_ENTRADA_SALIDA_LISTA = 3500,MOVIM_ENTRADA_SALIDA = 1,MOVIM_ALMACEN_LISTA = 3600,MOVIM_ALMACEN = 1,MOVIM_PDF = "" });
            //    listaMovim.Add(new Movim() { MOVIM_ID_ESTADO_REG = "A", MOVIM_FECHA = "2023-06-09", MOVIM_ID_OBJETO = 2, MOVIM_TIPO_MOVIM_LISTA = 3000, MOVIM_TIPO_MOVIM = 29, MOVIM_PESO_LISTA = 3100, MOVIM_PESO = 1, MOVIM_TRANSPORTISTA_LISTA = 3200, MOVIM_TRANSPORTISTA = 1, MOVIM_CLIENTE_LISTA = 3300, MOVIM_CLIENTE = 1, MOVIM_CHOFER_LISTA = 3400, MOVIM_CHOFER = 1, MOVIM_CAMION_ID = "Camion 2", MOVIM_REMOLQUE_ID = "Remolque 2", MOVIM_ALBARAN_ID = "Albaran 2", MOVIM_OBSERVACIONES = "Observaciones 2", MOVIM_ENTRADA_SALIDA_LISTA = 3500, MOVIM_ENTRADA_SALIDA = 1, MOVIM_ALMACEN_LISTA = 3600, MOVIM_ALMACEN = 1, MOVIM_PDF = "" });

            //    var insertMovimCmd = "INSERT INTO MOVIM (MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, " +
            //        "MOVIM_PESO_LISTA, MOVIM_PESO, MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, " +
            //        "MOVIM_CHOFER, MOVIM_CAMION_ID, MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, " +
            //        "MOVIM_ALMACEN_LISTA, MOVIM_ALMACEN, MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION) " +
            //        "VALUES(@MOVIM_ID_ESTADO_REG, @MOVIM_FECHA, @MOVIM_ID_OBJETO, @MOVIM_TIPO_MOVIM_LISTA, @MOVIM_TIPO_MOVIM, " +
            //        "@MOVIM_PESO_LISTA, @MOVIM_PESO, @MOVIM_TRANSPORTISTA_LISTA, @MOVIM_TRANSPORTISTA, @MOVIM_CLIENTE_LISTA, @MOVIM_CLIENTE, @MOVIM_CHOFER_LISTA, " +
            //        "@MOVIM_CHOFER, @MOVIM_CAMION_ID, @MOVIM_REMOLQUE_ID, @MOVIM_ALBARAN_ID, @MOVIM_OBSERVACIONES, @MOVIM_ENTRADA_SALIDA_LISTA, @MOVIM_ENTRADA_SALIDA, " +
            //        "@MOVIM_ALMACEN_LISTA, @MOVIM_ALMACEN, @MOVIM_PDF, @MOVIM_FECHA_ACTUALIZACION)";
            //    foreach (Movim movim in listaMovim)
            //    {
            //        using (var cmd = new SqliteCommand(insertMovimCmd, db))
            //        {
            //            cmd.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", movim.MOVIM_ID_ESTADO_REG);
            //            cmd.Parameters.AddWithValue("@MOVIM_FECHA", DateTime.Now.ToShortDateString());
            //            cmd.Parameters.AddWithValue("@MOVIM_ID_OBJETO", movim.MOVIM_ID_OBJETO);
            //            cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM_LISTA", movim.MOVIM_TIPO_MOVIM_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM", movim.MOVIM_TIPO_MOVIM);
            //            cmd.Parameters.AddWithValue("@MOVIM_PESO_LISTA", movim.MOVIM_PESO_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_PESO", movim.MOVIM_PESO);
            //            cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA_LISTA", movim.MOVIM_TRANSPORTISTA_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA", movim.MOVIM_TRANSPORTISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_CLIENTE_LISTA", movim.MOVIM_CLIENTE_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_CLIENTE", movim.MOVIM_CLIENTE);
            //            cmd.Parameters.AddWithValue("@MOVIM_CHOFER_LISTA", movim.MOVIM_CHOFER_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_CHOFER", movim.MOVIM_CHOFER);
            //            cmd.Parameters.AddWithValue("@MOVIM_CAMION_ID", movim.MOVIM_CAMION_ID);
            //            cmd.Parameters.AddWithValue("@MOVIM_ALBARAN_ID", movim.MOVIM_ALBARAN_ID);
            //            cmd.Parameters.AddWithValue("@MOVIM_REMOLQUE_ID", movim.MOVIM_REMOLQUE_ID);
            //            cmd.Parameters.AddWithValue("@MOVIM_OBSERVACIONES", movim.MOVIM_OBSERVACIONES);
            //            cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA_LISTA", movim.MOVIM_ENTRADA_SALIDA_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA", movim.MOVIM_ENTRADA_SALIDA);                        
            //            cmd.Parameters.AddWithValue("@MOVIM_ALMACEN_LISTA", movim.MOVIM_ALMACEN_LISTA);
            //            cmd.Parameters.AddWithValue("@MOVIM_ALMACEN", movim.MOVIM_ALMACEN);
            //            cmd.Parameters.AddWithValue("@MOVIM_PDF", movim.MOVIM_PDF);
            //            cmd.Parameters.AddWithValue("@MOVIM_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());
            //            await cmd.ExecuteNonQueryAsync();
            //        }
            //    }
            //}

            #endregion

            #region Dispositivos
            var dispositivosTableCommand = "CREATE TABLE IF NOT EXISTS DISPOSITIVOS(DISPOSITIVOS_ID_REG INTEGER NOT NULL UNIQUE, " +
                "DISPOSITIVOS_ID_ESTADO_REG TEXT NOT NULL DEFAULT 'A', DISPOSITIVOS_DESCRIP TEXT NOT NULL DEFAULT '.', " +
                "DISPOSITIVOS_CONTAINER TEXT NOT NULL DEFAULT '.', DISPOSITIVOS_FECHA_ACTUALIZACION TEXT NOT NULL DEFAULT '.', PRIMARY KEY(DISPOSITIVOS_ID_REG));";

            SqliteCommand dispositivosCreateTable = new SqliteCommand(dispositivosTableCommand, db);

            await dispositivosCreateTable.ExecuteReaderAsync();

            #endregion

            #region Historial Sincronizaciones
            var histSincTableCommand = "CREATE TABLE IF NOT EXISTS HIST_SINC(HIST_SINC_ID_REG INTEGER NOT NULL UNIQUE, " +
                "HIST_SINC_FECHA_HORA TEXT NOT NULL DEFAULT '.', PRIMARY KEY(HIST_SINC_ID_REG));";

            SqliteCommand histSincCreateTable = new SqliteCommand(histSincTableCommand, db);

            await histSincCreateTable.ExecuteReaderAsync();
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
