using System;

namespace Base
{
    [Serializable]
    public class CalendarioEstructura
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string tooltip { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
    }
}