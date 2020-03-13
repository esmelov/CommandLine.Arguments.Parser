using System;
using System.Linq;

namespace CommandLine.Utils.Arguments.Parser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ConsoleArgumentAttribute : Attribute
    {
        public ConsoleArgumentAttribute(params string[] names)
            : this(null, true, names)
        { }

        public ConsoleArgumentAttribute(object defaultValue, bool requierd, params string[] names)
        {
            if (names == null || names.Length == 0)
                throw new ArgumentNullException(nameof(names));

            DefaultValue = defaultValue;
            Requierd = requierd;
            Names = names;
        }

        public object DefaultValue { get; }

        public bool Requierd { get; }

        public string[] Names { get; }

        public bool TryExctract(string[] args, Type valueType, out object value, out string error)
        {
            error = string.Empty;
            var isFlag = valueType == typeof(bool);
            for (var i = 0; i < args.Length; i++)
            {
                if (Names.Contains(args[i]))
                {
                    value = isFlag ? (object)true : args[++i];
                    if (value != null && value.GetType() != valueType)
                        value = Convert.ChangeType(value, valueType);
                    return true;
                }
            }

            if (Requierd)
            {
                error = string.Join(", ", Names) + " is required";
                value = null;
                return false;
            }

            if (isFlag && (DefaultValue == null || DefaultValue.GetType() != typeof(bool)))
                value = false;
            else
                value = DefaultValue;
            if (value != null && value.GetType() != valueType)
                value = Convert.ChangeType(value, valueType);

            return true;
        }
    }
}