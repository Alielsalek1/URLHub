namespace URLshortner.Models
{
    public class URL
    {
        private int _ID;
        private string? _Url;

        public int ID { get { return _ID; } set { _ID = value; } }
        public string? Url { get { return _Url; } set { _Url = value; } }
    }
}
