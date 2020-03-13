using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Utils.Arguments.Parser.Models
{
    public class ParseResult<T>
        where T : class
    {
        public ParseResult(T value, string[] errors)
        {
            Errors = errors;
            Help = new Dictionary<string, string>();
            Value = value;
        }

        public ParseResult(string[] errors)
        {
            Errors = errors;
            Help = new Dictionary<string, string>();
            Value = default;
        }

        public ParseResult(IDictionary<string, string> help)
        {
            Help = new Dictionary<string, string>(help);
            Errors = Enumerable.Empty<string>().ToArray();
            Value = default;
        }

        public bool IsHelp => Help.Count > 0;

        public bool HasErrors => Errors.Length > 0;

        public bool HasValue => Value != null;

        public Dictionary<string, string> Help { get; }

        public string[] Errors { get; }

        public T Value { get; }
    }
}