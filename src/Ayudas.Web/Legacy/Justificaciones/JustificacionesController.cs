using Microsoft.AspNetCore.Mvc;

namespace Ayudas.Web.Legacy.Justificaciones
{
    public class JustificacionesController : Controller
    {
        public ActionResult Index(string estado)
        {
            var todas = JustificacionesDAL.ObtenerTodas();
            var lista = new List<JustificacionFila>();

            foreach (var j in todas)
            {
                if (String.IsNullOrEmpty(estado) || j.Estado == estado)
                {
                    lista.Add(j);
                }
            }

            ViewBag.Estado = estado;
            ViewBag.Total = lista.Count;
            return View(lista);
        }

        public ActionResult Detalle(int id)
        {
            var j = JustificacionesDAL.Obtener(id);
            if (j == null)
            {
                return NotFound();
            }

            return View(j);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validar(int id, string observaciones)
        {
            var j = JustificacionesDAL.Obtener(id);
            if (j == null)
            {
                return NotFound();
            }

            // Si lo justificado supera lo concedido, se rechaza; si no, se aprueba.
            string nuevoEstado;
            if (j.ImporteJustificado > j.ImporteConcedido)
            {
                nuevoEstado = "R";
                if (String.IsNullOrEmpty(observaciones))
                {
                    observaciones = "Importe justificado superior al concedido";
                }
            }
            else
            {
                nuevoEstado = "A";
            }

            JustificacionesDAL.CambiarEstado(id, nuevoEstado, observaciones);
            TempData["Mensaje"] = "Justificación " + id + " marcada como " + (nuevoEstado == "A" ? "aprobada" : "rechazada");
            return RedirectToAction("Detalle", new { id = id });
        }
    }
}
