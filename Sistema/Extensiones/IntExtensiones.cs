
namespace Sistema.Extensiones
{
    public static class IntExtensiones
    {
        public static string ToDayWords(this int day)
        {
            if (day == 0) return string.Empty;

            var dias = new[]
            {
                "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve",
                "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete",
                "dieciocho", "diecinueve", "veinte", "veintiuno","veintidós", "veintitrés", "veinticuatro",
                "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve", "treinta", "treinta y uno"
            };

            return dias[day - 1];
        }

        public static string ToYearWords(this int year)
        {
            var txtYear = year.ToString();

            return "dos mil " + int.Parse(txtYear.Substring(txtYear.Length - 2)).ToDayWords();
        }
    }
}
