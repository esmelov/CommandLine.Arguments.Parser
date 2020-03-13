using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Utils.Arguments.Parser.Attributes;
using CommandLine.Utils.Arguments.Parser.Models;

namespace CommandLine.Utils.Arguments.Parser
{
    public static class OptionsParser
    {
        private static readonly string[] _helpArgs = new string[] { "-h", "--help" };

        public static readonly string InformationString = $"For more information see help({string.Join(", ", _helpArgs)}).";

        public static ParserResult<T> Parse<T>(string[] args)
            where T : class, new()
        {
            if (IsHelp(args))
                return new ParserResult<T>(GetHelp<T>());
            var options = Get<T>(args, out var errors);
            if (errors.Length > 0)
                errors = errors.Concat(new[] { InformationString }).ToArray();
            return new ParserResult<T>(options, errors);
        }

        public static bool IsHelp(string[] args)
        {
            foreach (var arg in args)
            {
                if (_helpArgs.Contains(arg))
                    return true;
            }

            return false;
        }

        public static string[,] GetHelp<T>()
        {
            return new string[0, 0];
        }

        public static T Get<T>(string[] args, out string[] errors)
            where T : class, new()
        {
            var options = new T();
            var properties = options.GetType().GetProperties();
            var err = new List<string>(properties.Length);
            foreach (var property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(ConsoleArgumentAttribute)) as ConsoleArgumentAttribute;
                if (attribute == null)
                    continue;
                if (attribute.TryExctract(args, property.PropertyType, out var value, out var error))
                    property.SetValue(options, value);
                else
                    err.Add(error);
            }

            errors = err.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return options;
        }
    }
}
