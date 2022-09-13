using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LimpArena.Models;
using System.Data.Entity;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace LimpArena.Controllers
{
    public class HomeController : Controller
    {        

        arenaEntities db = new Models.arenaEntities();
        [HttpPost]
        public ActionResult Index(Pesaje ps, InspeccionTamañoGrano it, Triturado tr, Almacenamiento al,Almacenamiento_Inicial an)
        {
            variables objvariables = new variables();

            List<decimal> listIndexPeso = new List<decimal>();
            List<string> listIndexFechaPeso = new List<string>();
            List<decimal> listIndexInspeccion = new List<decimal>();
            List<string> listIndexFechaInspeccion = new List<string>();
            List<decimal> listIndexTiempo = new List<decimal>();
            List<decimal> lstIndexAlmacenamientoF = new List<decimal>();
            List<string> lstIndexpallet = new List<string>();

            DateTime fechapeso = DateTime.Now;

            var peso = db.Pesaje.Where(x => x.Fecha.Value.Day >= fechapeso.Day && x.Fecha.Value.Day <= fechapeso.Day).ToList();
            var ultimopeso = peso.OrderByDescending(x => x.Id).FirstOrDefault();
            var Inspeccion = db.InspeccionTamañoGrano.Where(x => x.Fecha.Value.Day >= fechapeso.Day && x.Fecha.Value.Day <= fechapeso.Day).ToList();
            var Tiempo = db.Triturado.Where(x => x.Fecha.Value.Day >= fechapeso.Day && x.Fecha.Value.Day <= fechapeso.Day).ToList();
            var ultimoTiempo = Tiempo.OrderByDescending(x => x.Id).FirstOrDefault();
            var total = db.Almacenamiento.Where(x => x.FechaSalida == null).ToList();
            var almacenamiento = db.Almacenamiento_Inicial.OrderByDescending(x => x.Id).FirstOrDefault();

            decimal Acomulado = 0;

            listIndexFechaPeso.Add("");
            foreach (var item in peso)
            {
                listIndexPeso.Add(Convert.ToDecimal(item.Peso));
                listIndexFechaPeso.Add(item.Fecha.Value.ToShortDateString());
            }
            foreach (var item in Inspeccion)
            {
                listIndexInspeccion.Add(Convert.ToDecimal(item.TamanoGrano));
                listIndexFechaInspeccion.Add(item.Fecha.Value.ToShortDateString());
            }
            foreach (var item in Tiempo)
            {
                listIndexTiempo.Add(Convert.ToDecimal(item.TiempoCiclo));
            }
            foreach (var item in total)
            {
                lstIndexpallet.Add(Convert.ToString("Pallet " + item.pallet));
                lstIndexAlmacenamientoF.Add(Convert.ToDecimal(item.Peso));
                Acomulado += Convert.ToDecimal(item.Peso);
            }


            objvariables.indexPesaje = listIndexPeso;
            objvariables.indexFechaPesaje = listIndexFechaPeso;
            objvariables.indexInspeccion = listIndexInspeccion;
            objvariables.indexFechaInspeccion = listIndexFechaInspeccion;
            objvariables.indexTiempo = listIndexTiempo;
            objvariables.IndexAlmacenamientoF = lstIndexAlmacenamientoF;
            objvariables.Indexpallet = lstIndexpallet;
            objvariables.VarIndexAlmacenamiento = Acomulado;
            objvariables.temAlmacenamiento = Convert.ToDecimal(almacenamiento.Temperatura);
            objvariables.Nivel = Convert.ToDecimal(almacenamiento.Nivel);

            if (ultimopeso != null) { objvariables.VarUltimoP = Convert.ToDecimal(ultimopeso.Peso); }
            if (ultimoTiempo != null) { objvariables.VarUltimoT = Convert.ToDecimal(ultimoTiempo.TiempoCiclo); }
            



            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult About(Pesaje ps)
        {
            variables objvariables = new variables();

            DateTime fechapeso = DateTime.Now;
            
            var peso = db.Pesaje.Where(x => x.Fecha.Value.Day >= fechapeso.Day && x.Fecha.Value.Day <= fechapeso.Day).ToList();

            VariablesContador.contadorPesaje = peso.Count;
         
            List<decimal> listIndexPeso = new List<decimal>();
            List<string> listIndexFechaPeso = new List<string>();

           
            foreach (var item in peso)
            {
                listIndexPeso.Add(Convert.ToDecimal(item.Peso));
                listIndexFechaPeso.Add(item.Fecha.Value.ToShortDateString());
            }


            objvariables.indexPesaje = listIndexPeso;
            objvariables.indexFechaPesaje = listIndexFechaPeso;
       

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ValidarPunto(Pesaje rd)
        {            
            variables objvariables = new variables();

            DateTime fecha = Convert.ToDateTime(rd.Fecha);
            var fechaf = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            var peso1 = db.Pesaje.Where(x => x.Fecha.Value >= rd.Fecha && x.Fecha.Value <= fechaf).ToList();

            int i = peso1.Count;
         
            if (i > VariablesContador.contadorPesaje)
            {
               VariablesContador.contadorPesaje = i;
                var validarPunto = db.Pesaje.OrderByDescending(x => x.Id).FirstOrDefault();

                foreach (var item in peso1)
                {
                    objvariables.Varpeso = Convert.ToDecimal(item.Peso);
                    objvariables.VarFechaPeso = Convert.ToString(item.Fecha.Value.ToShortTimeString());                   
                }
                
            }            

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public ActionResult pesaje(Pesaje rd)
        {
            
            variables objvariables = new variables();
            List<decimal> listPeso = new List<decimal>();
            List<string> listFecha = new List<string>();
            DateTime fecha = Convert.ToDateTime(rd.Fecha);
            var fechaf = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            var peso = db.Pesaje.Where(x => x.Fecha.Value >= rd.Fecha && x.Fecha.Value <= fechaf).ToList();

            VariablesContador.contadorPesaje = peso.Count;
            
            foreach (var item in peso)
            {
                listPeso.Add(Convert.ToDecimal(item.Peso));
                listFecha.Add(item.Fecha.Value.ToShortDateString());
            }

            objvariables.peso = listPeso;
            objvariables.fecha = listFecha;
            return Json(objvariables, JsonRequestBehavior.AllowGet);

        }
        public ActionResult pesaje()
        {
            List<SelectListItem> lstYear = new List<SelectListItem>();

            int year = DateTime.Now.Year;

            for (int i = year; i >= 2022; i--)
            {
                lstYear.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.year = lstYear;
            return View();
        }
        [HttpPost]
        public ActionResult pesajeMensual(Pesaje rd)
        {
            variables objvariables = new variables();
            List<decimal> listPeso = new List<decimal>();
            List<string> listFecha = new List<string>();

            decimal to = 0;
            //DateTime fecha = Convert.ToDateTime(rd.Fecha);
            //var fechaf = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            int[] mes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var total = db.Pesaje.Where(x => x.Fecha.Value.Year == rd.Fecha.Value.Year).ToList();

            listFecha.Add("");
            foreach (var item in mes)
            {
                foreach (var item1 in total)
                {
                    if (item1.Fecha.Value.Month == item)
                    {
                        to += Convert.ToDecimal(item1.Peso);
                    }

                }
                listPeso.Add(to);
                to = 0;
            }

            objvariables.pesoMensual = listPeso;

            return Json(objvariables, JsonRequestBehavior.AllowGet);


        }    
       
        [HttpPost]
        public ActionResult almacenamiento(Almacenamiento_Inicial rd)
        {
            variables objvariables = new variables();

            var almacenamiento = db.Almacenamiento_Inicial.OrderByDescending(x => x.Id).FirstOrDefault();

            objvariables.Nivel = Convert.ToDecimal(almacenamiento.Nivel);
            objvariables.temAlmacenamiento = Convert.ToDecimal(almacenamiento.Temperatura);
            objvariables.Estado = Convert.ToBoolean(almacenamiento.Estado);

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult almacenamiento()
        {
            return View();
        }
        [HttpPost]
        public ActionResult triturado(Triturado rd)
        {
            variables objvariables = new variables();

            List<decimal> Tiempot = new List<decimal>();
            List<string> Fechat = new List<string>();
            List<int> id = new List<int>();
            // var tiempo = db.Triturado.OrderByDescending(x => x.Id).Take(10).ToList(); //Tomando los ultimos 10 datos            
            var ultimo = db.Triturado.OrderByDescending(x => x.Id).FirstOrDefault();//tomando el ultimo dato
            DateTime Hora = Convert.ToDateTime(ultimo.Fecha);
            var datos = db.Triturado.Where(x => x.Fecha.Value.Day >= rd.Fecha.Value.Day && x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();

            id.Add(0);
            foreach (var item in datos)
            {
                Tiempot.Add(Convert.ToDecimal(item.TiempoCiclo));
                Fechat.Add(item.Fecha.Value.ToShortDateString());
                id.Add(item.Id);
            }

            objvariables.tiempoCiclo = Tiempot;
            objvariables.fechaTriturado = Fechat;
            objvariables.idTriturado = id;

            objvariables.tiempoTriturado = Convert.ToDecimal(ultimo.TiempoCiclo);
            objvariables.horaTriturado = Convert.ToString(Hora);
            objvariables.contaminacionT = Convert.ToDecimal(ultimo.Contaminacion);
            objvariables.granoT = Convert.ToInt32(ultimo.TamañoGrano);
            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult triturado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cribado(Cribado rd)
        {
            variables objvariables = new variables();
            List<decimal> contamCribado = new List<decimal>();
            List<decimal> residCribado = new List<decimal>();
            List<string> fechaCribado = new List<string>();

            var ultimo = db.Cribado.OrderByDescending(x => x.Id).FirstOrDefault();//tomando el ultimo dato
            DateTime Hora = Convert.ToDateTime(ultimo.Fecha);
            var data = db.Cribado.Where(x =>
            x.Fecha.Value.Day >= rd.Fecha.Value.Day
            &&
            x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();


            foreach (var item in data)
            {
                contamCribado.Add(Convert.ToDecimal(item.Contaminacion));
                residCribado.Add(Convert.ToDecimal(item.residuo));
                fechaCribado.Add(item.Fecha.Value.ToShortDateString());
            }

            objvariables.contaminacionC = contamCribado;
            objvariables.ResiduoC = residCribado;
            objvariables.fechaC = fechaCribado;

            objvariables.ResiduoCribado = Convert.ToDecimal(ultimo.residuo);
            objvariables.fechaCribado = Convert.ToString(Hora);
            objvariables.contaminacionCribado = Convert.ToDecimal(ultimo.Contaminacion);
            objvariables.granoCribado = Convert.ToInt32(ultimo.TamanoGrano);

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Cribado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult separacionMagneticos(SeparacionMagneticos rd)
        {
            variables objvariables = new variables();
            List<decimal> lstContamSM = new List<decimal>();
            List<int> lstPartiSM = new List<int>();
            List<string> lstFechaSM = new List<string>();

            List<decimal> lstMinSM = new List<decimal>();
            List<decimal> lstMaxSmResiduo = new List<decimal>();
            List<decimal> rangosContaminacion = new List<decimal>();
            List<decimal[]> datosF = new List<decimal[]>();// arrayList           

            var ultimoSM = db.SeparacionMagneticos.OrderByDescending(x => x.Id).FirstOrDefault();//tomando el ultimo dato
            DateTime Hora = Convert.ToDateTime(ultimoSM.Fecha);
            var data = db.SeparacionMagneticos.Where(x =>
            x.Fecha.Value.Day >= rd.Fecha.Value.Day
            &&
            x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();


            foreach (var item in data)
            {
                lstContamSM.Add(Convert.ToDecimal(item.Contaminacion));
                lstPartiSM.Add(Convert.ToInt32(item.Particulas));
                lstFechaSM.Add(item.Fecha.Value.ToShortDateString());
                lstMinSM.Add(0);
                lstMaxSmResiduo.Add(60);

                rangosContaminacion.Add(0);
                rangosContaminacion.Add(5);

                decimal[] array = rangosContaminacion.ToArray();
                datosF.Add(array);
                rangosContaminacion.Clear();
            }

            objvariables.lstcontaminacionSM = lstContamSM;
            objvariables.lstparticulasSM = lstPartiSM;
            objvariables.lstfechasm = lstFechaSM;

            objvariables.varcontamSM = Convert.ToDecimal(ultimoSM.Contaminacion);
            objvariables.varpartiSM = Convert.ToInt32(ultimoSM.Particulas);
            objvariables.varfechaSM = Convert.ToString(Hora);
            objvariables.BarMaxSmResiduo = lstMaxSmResiduo;
            objvariables.BarMinSM = lstMinSM;

            objvariables.BarMINSmContaminacion = datosF;


            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult separacionMagneticos()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SentrifugadodeCargas(CentrifugadorCarga rd)
        {
            variables objvariables = new variables();
            List<string> lstFechaSC = new List<string>();
            List<decimal> lstTemperatura = new List<decimal>();

            var data = db.CentrifugadorCarga.Where(x => x.Fecha.Value.Day >= rd.Fecha.Value.Day &&
            x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();

            foreach (var item in data)
            {
                lstFechaSC.Add(Convert.ToString(item.Fecha.Value.ToShortDateString()));
                lstTemperatura.Add(Convert.ToDecimal(item.TemperaturaAgua));
            }
            objvariables.lstfechaSC = lstFechaSC;
            objvariables.lstTemperaturaSC = lstTemperatura;

            List<SentrifugadoC> datos = new List<SentrifugadoC>();

            foreach (var item in data)
            {

                datos.Add(new SentrifugadoC
                {

                    ID = item.Id,
                    Temperatura = Convert.ToDecimal(item.TemperaturaAgua),
                    Conductividad = Convert.ToDecimal(item.Conductividad),
                    Ph = Convert.ToDecimal(item.Ph),
                    Dureza = Convert.ToDecimal(item.DurezaAgua),
                    Fosfato = Convert.ToDecimal(item.Fosfato),
                    Silice = Convert.ToDecimal(item.Silice),
                    Fecha = Convert.ToString(item.Fecha)
                });
            }
            objvariables.lista = datos;
            return Json(objvariables, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SentrifugadodeCargas()
        {

            return View();
        }
        [HttpPost]
        public ActionResult InspeccionTamañoGrano(InspeccionTamañoGrano rd)
        {
            variables objvariables = new variables();
            List<String> lstFechaITG = new List<String>();
            List<decimal> lstITM = new List<decimal>();

            DateTime fecha = Convert.ToDateTime(rd.Fecha);
            var fechaTG = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            var dataTG = db.InspeccionTamañoGrano.Where(x => 
            
            x.Fecha.Value >= rd.Fecha &&  x.Fecha.Value <= fechaTG).ToList();

            VariablesContador.contadorInspTGrano = dataTG.Count;

            foreach (var item in dataTG)
            {
                lstFechaITG.Add(Convert.ToString(item.Fecha.Value.ToLongTimeString()));
                lstITM.Add(Convert.ToDecimal(item.TamanoGrano));
            }

            objvariables.lstFechaTG = lstFechaITG;
            objvariables.lstTamanoTG = lstITM;

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult validarBarras(InspeccionTamañoGrano rd)
        {
            variables objvariables = new variables();

            DateTime fecha = Convert.ToDateTime(rd.Fecha);
            var fechaTG = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            var dataTG = db.InspeccionTamañoGrano.Where(x =>

            x.Fecha.Value >= rd.Fecha && x.Fecha.Value <= fechaTG).ToList();

            int cont = dataTG.Count;

            if (cont > VariablesContador.contadorInspTGrano)
            {
                VariablesContador.contadorInspTGrano = cont;
                var validarPunto = db.InspeccionTamañoGrano.OrderByDescending(x => x.Id).FirstOrDefault();

                foreach (var item in dataTG)
                {
                    objvariables.varpointaddTG = Convert.ToDecimal(item.TamanoGrano);
                    objvariables.varpointaddFechaTG= Convert.ToString(item.Fecha.Value.ToLongTimeString());
                }

            }
            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InspeccionTamañoGrano()
        {

            return View();
        }
        [HttpPost]
        public ActionResult QuemadoResina(QuemadodeRecina rd)
        {
            variables objvariables = new variables();
            List<String> lstFecha = new List<String>();
            List<decimal> lstTempInterna = new List<decimal>();
            List<decimal> lstTempExterna = new List<decimal>();
            List<decimal> lstTempArena = new List<decimal>();
            List<decimal> lstCalibracion = new List<decimal>();
            var ultimoQR = db.QuemadodeRecina.OrderByDescending(x => x.Id).FirstOrDefault();//tomando el ultimo dato
            var data = db.QuemadodeRecina.Where(x => x.Fecha.Value.Day >= rd.Fecha.Value.Day &&
           x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();

            foreach (var item in data)
            {
                lstFecha.Add(Convert.ToString(item.Fecha.Value.ToShortDateString()));
                lstTempInterna.Add(Convert.ToDecimal(item.TempInterna));
                lstTempExterna.Add(Convert.ToDecimal(item.TempExterna));
                lstTempArena.Add(Convert.ToDecimal(item.TempArena));
                lstCalibracion.Add(Convert.ToDecimal(item.AireComprimido));
            }

            objvariables.lstTempInternaQR = lstTempInterna;
            objvariables.lstTempExternaQR = lstTempExterna;
            objvariables.lstTempArenaQR = lstTempArena;
            objvariables.lstCalibracionQR = lstCalibracion;
            objvariables.lstFechaQR = lstFecha;

            objvariables.VarTempInternaQR = Convert.ToDecimal(ultimoQR.TempInterna);
            objvariables.VarTempExternaQR = Convert.ToDecimal(ultimoQR.TempExterna);
            objvariables.VarTempArenaQR = Convert.ToDecimal(ultimoQR.TempArena);
            objvariables.VarCalibracionQR = Convert.ToDecimal(ultimoQR.AireComprimido);
            objvariables.VarFechaQR = Convert.ToString(ultimoQR.Fecha);

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QuemadoResina()
        {

            return View();
        }
        [HttpPost]
        public ActionResult TanqueEnfriamento(TanqueEnfriamento rd)
        {
            variables objvariables = new variables();

            var data = db.TanqueEnfriamento.OrderByDescending(x => x.Id).FirstOrDefault();

            objvariables.NvlTanqEnfriamento = Convert.ToDecimal(data.Nivel);
            objvariables.TempArenaTE = Convert.ToDecimal(data.Temperatura);
            objvariables.EstadoTanqEnfriamento = Convert.ToBoolean(data.Estado);
            objvariables.FechaTE = Convert.ToString(data.Fecha);

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TanqueEnfriamento()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CribadoProceso2(CribadoProceso2 rd)
        {

            variables objvariables = new variables();
            List<decimal> lstContaminacion = new List<decimal>();
            List<decimal> lstReciduo = new List<decimal>();
            List<string> lstFecha = new List<string>();

            var ultimo = db.CribadoProceso2.OrderByDescending(x => x.Id).FirstOrDefault();//tomando el ultimo dato
            DateTime Hora = Convert.ToDateTime(ultimo.Fecha);
            var data = db.CribadoProceso2.Where(x =>
            x.Fecha.Value.Day >= rd.Fecha.Value.Day
            &&
            x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();


            foreach (var item in data)
            {
                lstContaminacion.Add(Convert.ToDecimal(item.Contaminación));
                lstReciduo.Add(Convert.ToDecimal(item.Residuo));
                lstFecha.Add(item.Fecha.Value.ToShortDateString());
            }

            objvariables.lstContaminacionCr = lstContaminacion;
            objvariables.lstResiduoCr = lstReciduo;
            objvariables.lstFechaCr = lstFecha;

            objvariables.VarResiduoCR = Convert.ToDecimal(ultimo.Residuo);
            objvariables.VarFechaCR = Convert.ToString(Hora);
            objvariables.VarContaminacionCR = Convert.ToDecimal(ultimo.Contaminación);
            objvariables.VarTGranoCR = Convert.ToInt32(ultimo.Tamanio);

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CribadoProceso2()
        {

            return View();
        }
        [HttpPost]
        public ActionResult InstpeccionConductividad(InstpeccionConductividad rd)
        {
            variables objvariables = new variables();
            List<int> lstConductividad = new List<int>();
            List<int> lstMaxConductividad = new List<int>();
            List<string> lstFechaCond = new List<string>();


            var data = db.InstpeccionConductividad.Where(x =>
            x.Fecha.Value.Day >= rd.Fecha.Value.Day
            &&
            x.Fecha.Value.Day <= rd.Fecha.Value.Day).ToList();

            lstFechaCond.Add(" ");

            foreach (var item in data)
            {

                lstConductividad.Add(Convert.ToInt32(item.Conductividad));
                lstFechaCond.Add(item.Fecha.Value.ToShortDateString());
                lstMaxConductividad.Add(Convert.ToInt32(500));

            }

            objvariables.lstConductividad = lstConductividad;
            objvariables.lstFechaCond = lstFechaCond;
            objvariables.lstMaxConductividad = lstMaxConductividad;


            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InstpeccionConductividad()
        {

            return View();
        }
        [HttpPost]
        public ActionResult PesajeFinalDiario(PesajeFinal rd)
        {

            variables objvariables = new variables();
            List<decimal> listPesoFinal = new List<decimal>();
            List<string> listFechaPF = new List<string>();
            //DateTime fecha = Convert.ToDateTime(rd.Fecha);
            //var fechaf = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            var pesoFinal = db.PesajeFinal.Where(x => x.Fecha.Value.Month >= rd.Fecha.Value.Month && x.Fecha.Value.Month <= rd.Fecha.Value.Month).ToList();


            foreach (var item in pesoFinal)
            {
                listPesoFinal.Add(Convert.ToDecimal(item.peso));
                listFechaPF.Add(item.Fecha.Value.ToShortDateString());
            }

            objvariables.lstPesoDiarioF = listPesoFinal;
            objvariables.lstFechaPesoDF = listFechaPF;
            return Json(objvariables, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PesajeFinalDiario()
        {
            List<SelectListItem> lstYear = new List<SelectListItem>();

            int year = DateTime.Now.Year;

            for (int i = year; i >= 2022; i--)
            {
                lstYear.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.year = lstYear;
            return View();
        }
        [HttpPost]
        public ActionResult PesajeFinalMensual(PesajeFinal rd)
        {

            variables objvariables = new variables();
            List<decimal> listPesajeMensualF = new List<decimal>();
            List<string> listFechaPesajeMF = new List<string>();

            decimal to = 0;
            //DateTime fecha = Convert.ToDateTime(rd.Fecha);
            //var fechaf = Convert.ToDateTime(fecha.ToString("yyyy-MM-dd 23:59:59"));
            int[] mes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var total = db.PesajeFinal.Where(x => x.Fecha.Value.Year == rd.Fecha.Value.Year).ToList();


            foreach (var item in mes)
            {
                foreach (var item1 in total)
                {
                    if (item1.Fecha.Value.Month == item)
                    {
                        to += Convert.ToDecimal(item1.peso);
                    }

                }
                listPesajeMensualF.Add(to);
                to = 0;
            }

            objvariables.lstPesoMensualF = listPesajeMensualF;

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult AlmacenamientoFinal(Almacenamiento rd)
        {
            variables objvariables = new variables();
            List<decimal> lstAlmacenamientoF = new List<decimal>();
            List<string> lstpallet = new List<string>();
            List<string> lstR = new List<string>();
            List<AlmacenamientoF> datos1 = new List<AlmacenamientoF>();
            Random random = new Random();

            var total = db.Almacenamiento.Where(x => x.FechaSalida == null).ToList();
            decimal Acomulado = 0;

            foreach (var item in total)
            {
                if (item.pallet != null)
                {
                    lstpallet.Add(Convert.ToString("Pallet " + item.pallet));
                    lstAlmacenamientoF.Add(Convert.ToDecimal(item.Peso));

                    Acomulado += Convert.ToDecimal(item.Peso);

                    datos1.Add(new AlmacenamientoF
                    {
                        Peso = Convert.ToDecimal(item.Peso),
                        Pallet = Convert.ToString(item.pallet),
                        Fecha = Convert.ToString(item.FechaEntrada)
                    });

                    lstR.Add("#" + random.Next().ToString("X"));
                }

            }



            objvariables.VarAlmacenamientoF = Acomulado;
            objvariables.lstAlmacenamientoF = lstAlmacenamientoF;
            objvariables.lstpallet = lstpallet;
            objvariables.lstAlmacenamiento = datos1;
            objvariables.lstRandom = lstR;

            return Json(objvariables, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AlmacenamientoFinal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ActualizarTabla(int[] actualizar)
        {
            if (actualizar != null)
            {

                for (int i = 0; i < actualizar.Length; i++)
                {
                    if (actualizar[i] != 0)
                    {
                        ViewBag.RowsAffected = db.Database.ExecuteSqlCommand("UPDATE Almacenamiento SET FechaSalida = CURRENT_TIMESTAMP WHERE pallet='" + actualizar[i] + "'");
                    }
                }
            }

            return Json(actualizar);
        }



        public class variables
        {
            //1 page
            public List<decimal> peso { get; set; }
            public List<String> fecha { get; set; }

            //variables validar
            public decimal Varpeso { get; set; }
            public string VarFechaPeso { get; set; }
            //1-Mensual
            public List<decimal> pesoMensual { get; set; }
            //2 page
            public decimal Nivel { get; set; }
            public decimal temAlmacenamiento { get; set; }
            public bool Estado { get; set; }
            //3 page
            public List<decimal> tiempoCiclo { get; set; }
            public List<String> fechaTriturado { get; set; }
            public List<int> idTriturado { get; set; }
            public decimal tiempoTriturado { get; set; }
            public String horaTriturado { get; set; }
            public int granoT { get; set; }
            public decimal contaminacionT { get; set; }
            //4 page
            public List<decimal> contaminacionC { get; set; }
            public List<decimal> ResiduoC { get; set; }
            public List<String> fechaC { get; set; }
            public int granoCribado { get; set; }
            public decimal contaminacionCribado { get; set; }
            public decimal ResiduoCribado { get; set; }
            public String fechaCribado { get; set; }
            //5 page
            public List<decimal> lstcontaminacionSM { get; set; }
            public List<int> lstparticulasSM { get; set; }
            public List<String> lstfechasm { get; set; }
            public Decimal varcontamSM { get; set; }
            public String varfechaSM { get; set; }
            public int varpartiSM { get; set; }
            public List<decimal> BarMinSM { get; set; }
            public List<decimal> BarMaxSmResiduo { get; set; }
            public List<decimal[]> BarMINSmContaminacion { get; set; }
            //6 page
            public List<String> lstfechaSC { get; set; }
            public List<decimal> lstTemperaturaSC { get; set; }
            public List<SentrifugadoC> lista { get; set; }
            //7 page

            public List<decimal> lstTamanoTG { get; set; }
            public List<String> lstFechaTG { get; set; }
            public decimal varpointaddTG{ get; set; }
            public string varpointaddFechaTG { get; set; }

            ///8 page
            public List<String> lstFechaQR { get; set; }
            public List<decimal> lstTempInternaQR { get; set; }
            public List<decimal> lstTempExternaQR { get; set; }
            public List<decimal> lstTempArenaQR { get; set; }
            public List<decimal> lstCalibracionQR { get; set; }
            public String VarFechaQR { get; set; }
            public Decimal VarTempInternaQR { get; set; }
            public Decimal VarTempExternaQR { get; set; }
            public Decimal VarTempArenaQR { get; set; }
            public Decimal VarCalibracionQR { get; set; }
            //9 page

            public decimal NvlTanqEnfriamento { get; set; }
            public decimal TempArenaTE { get; set; }
            public bool EstadoTanqEnfriamento { get; set; }
            public String FechaTE { get; set; }
            //10 page
            public List<decimal> lstContaminacionCr { get; set; }
            public List<decimal> lstResiduoCr { get; set; }
            public List<String> lstFechaCr { get; set; }
            public decimal VarTGranoCR { get; set; }
            public decimal VarContaminacionCR { get; set; }
            public decimal VarResiduoCR { get; set; }
            public String VarFechaCR { get; set; }
            //11page
            public List<int> lstConductividad { get; set; }
            public List<int> lstMaxConductividad { get; set; }
            public List<String> lstFechaCond { get; set; }
            //12 page
            public List<decimal> lstPesoDiarioF { get; set; }
            public List<String> lstFechaPesoDF { get; set; }
            //12 page-Mensual
            public List<decimal> lstPesoMensualF { get; set; }
            //13 page
            public decimal VarAlmacenamientoF { get; set; }
            public List<decimal> lstAlmacenamientoF { get; set; }
            public List<String> lstpallet { get; set; }
            public List<String> lstRandom { get; set; }
            public List<AlmacenamientoF> lstAlmacenamiento { get; set; }

            //index
            public List<decimal> indexPesaje { get; set; }
            public List<String> indexFechaPesaje { get; set; }
            public List<decimal> indexInspeccion { get; set; }
            public List<String> indexFechaInspeccion { get; set; }
            public List<decimal> indexTiempo { get; set; }
            public List<decimal> IndexAlmacenamientoF { get; set; }
            public List<String> Indexpallet { get; set; }           
            public decimal VarIndexAlmacenamiento { get; set; }
            public decimal VarUltimoP { get; set; }
            public decimal VarUltimoT { get; set; }




        }

    }
}
