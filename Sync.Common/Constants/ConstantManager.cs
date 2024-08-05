namespace Sync.Common.Constants
{
    public class ConstantManager : IConstantManager
    {
        public string QueryFormat() => "yyyy-MM-dd";

        public string Since() => "since";

        public string Until() => "until";

        public string Field() => "field";
    }
}
