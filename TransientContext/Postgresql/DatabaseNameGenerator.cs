using System;

namespace TransientContext.Postgresql
{
    class DatabaseNameGenerator : IDatabaseNameGenerator
    {
        public string Prefix { get; set; } = "transient_context_";

        public string Generate()
        {
            return $"{Prefix}{DateTime.UtcNow.Ticks}";
        }
    }
}
