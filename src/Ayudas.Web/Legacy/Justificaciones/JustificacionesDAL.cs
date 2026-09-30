using System.Data;
using Microsoft.Data.SqlClient;

namespace Ayudas.Web.Legacy.Justificaciones
{
    /// <summary>
    /// Acceso a datos de justificaciones (módulo heredado de la aplicación anterior).
    /// </summary>
    public static class JustificacionesDAL
    {
        public static string CadenaConexion = "";

        public static List<JustificacionFila> ObtenerTodas()
        {
            var lista = new List<JustificacionFila>();
            var tabla = new DataTable();

            using (var cn = new SqlConnection(CadenaConexion))
            {
                var sql = "SELECT j.ID_JUSTIF, j.ID_AYUDA, j.F_PRESENT, j.IMP_JUSTIF, j.COD_ESTADO, j.OBSERV, " +
                          "a.NifBeneficiario, a.NombreBeneficiario, a.ImporteConcedido " +
                          "FROM JUSTIFICACIONES j INNER JOIN Ayudas a ON a.Id = j.ID_AYUDA " +
                          "ORDER BY j.F_PRESENT DESC";
                var cmd = new SqlCommand(sql, cn);
                var da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Convertir(fila));
            }

            return lista;
        }

        public static JustificacionFila? Obtener(int id)
        {
            var tabla = new DataTable();

            using (var cn = new SqlConnection(CadenaConexion))
            {
                var sql = "SELECT j.ID_JUSTIF, j.ID_AYUDA, j.F_PRESENT, j.IMP_JUSTIF, j.COD_ESTADO, j.OBSERV, " +
                          "a.NifBeneficiario, a.NombreBeneficiario, a.ImporteConcedido " +
                          "FROM JUSTIFICACIONES j INNER JOIN Ayudas a ON a.Id = j.ID_AYUDA " +
                          "WHERE j.ID_JUSTIF = " + id;
                var cmd = new SqlCommand(sql, cn);
                var da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Convertir(tabla.Rows[0]);
        }

        public static void CambiarEstado(int id, string estado, string observaciones)
        {
            using (var cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                var cmd = new SqlCommand("UPDATE JUSTIFICACIONES SET COD_ESTADO = @estado, OBSERV = @obs WHERE ID_JUSTIF = @id", cn);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@obs", (object?)observaciones ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static JustificacionFila Convertir(DataRow fila)
        {
            var j = new JustificacionFila();
            j.IdJustificacion = Convert.ToInt32(fila["ID_JUSTIF"]);
            j.IdAyuda = Convert.ToInt32(fila["ID_AYUDA"]);
            j.FechaPresentacion = Convert.ToDateTime(fila["F_PRESENT"]);
            j.ImporteJustificado = Convert.ToDecimal(fila["IMP_JUSTIF"]);
            j.Estado = fila["COD_ESTADO"].ToString()!;
            j.Observaciones = fila["OBSERV"] == DBNull.Value ? "" : fila["OBSERV"].ToString()!;
            j.NifBeneficiario = fila["NifBeneficiario"].ToString()!;
            j.NombreBeneficiario = fila["NombreBeneficiario"].ToString()!;
            j.ImporteConcedido = Convert.ToDecimal(fila["ImporteConcedido"]);
            return j;
        }
    }
}
