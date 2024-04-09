namespace Datos.Repositorios.Firma
{
    public class RepositorioBodevaFiel
    {
        public static void GuardarLoginCall(WSLoginCall wsLoginCall)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.WSLoginCalls.Add(wsLoginCall);

                dc.SaveChanges();
            }
        }

        public static void GuardarToken(token token)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.tokens.Add(token);

                dc.SaveChanges();
            }
        }

        public static void GuardarCadena(cadena cadena)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.cadenas.Add(cadena);

                dc.SaveChanges();
            }
        }

        public static void GuardarOperacion(operacion operacion)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.operaciones.Add(operacion);

                dc.SaveChanges();
            }
        }

        public static void GuardarTimeStamping(timestamping timestamping)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.timestampings.Add(timestamping);

                dc.SaveChanges();
            }
        }

        public static void GuardarXml(xml xml)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.xmls.Add(xml);

                dc.SaveChanges();
            }
        }

        public static void GuardarDecodificacion(decodificacion decodificacion)
        {
            using (var dc = new BovedaFielEntidades())
            {
                dc.decodificaciones.Add(decodificacion);

                dc.SaveChanges();
            }
        }
    }
}
