using System.Linq;

namespace CommandLine.Utils.Arguments.Parser.Models
{
    public class ParserResult<T>
        where T : class
    {
        public ParserResult(T value, string[] errors)
        {
            Errors = errors;
            Help = new string[0,0];
            Value = value;
        }

        public ParserResult(string[] errors)
        {
            Errors = errors;
            Help = new string[0,0];
            Value = default;
        }

        public ParserResult(string[,] help)
        {
            Help = help;
            Errors = Enumerable.Empty<string>().ToArray();
            Value = default;
        }

        public bool IsHelp => Help.Length > 0;

        public bool HasErrors => Errors.Length > 0;

        public bool HasValue => Value != null;

        public string[,] Help { get; }

        public string[] Errors { get; }

        public T Value { get; }
    }
}