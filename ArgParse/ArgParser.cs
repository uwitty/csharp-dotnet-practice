using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgParse
{
    public class ArgParser
    {
        public string Desc { get; private set; }
        public Dictionary<string, Argument> Arguments { get; private set; }

        public ArgParser(string desc = "")
        {
            this.Desc = desc;
            this.Arguments = new Dictionary<string, Argument>();
        }

        public void AddArgument(string name, string defaultValue = null, string help = null)
        {
            AddArgument<string>(name, defaultValue, help);
        }

        public void AddArgument<ValueType>(string name, ValueType defaultValue = default(ValueType), string help = null)
        {
            if (Arguments.ContainsKey(name))
            {
                throw new ArgumentException("This argument already exists. ", name);
            }
            if (!name.StartsWith("--") || name.Length <= 2)
            {
                throw new ArgumentException("Invalid option name. ", name);
            }

            Arguments.Add(name, new Argument(name, typeof(ValueType), defaultValue, help));
        }

        public Dictionary<string, object> Parse(string[] args)
        {
            var defaultValues = Arguments.Values.Where(a => a.DefaultValue != null).
                                ToDictionary(x => x.Name.Substring(2), x => x.DefaultValue);

            var dict = new Dictionary<string, object>();
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string value = (i >= args.Length - 1) ? null : args[i+1];

                if (arg.StartsWith("--"))
                {
                    dict[arg.Substring(2)] = GetOptionalArgumentValue(arg, value);
                    i++;    // skip value element
                }
                else
                {
                    throw new ArgumentException("Invalid argument", args.ToString());
                }
            }

            return dict.Concat(defaultValues.Where(e => !dict.ContainsKey(e.Key))).ToDictionary(e => e.Key, e => e.Value);
        }

        public string Help()
        {
            return Desc;
        }

        private object GetOptionalArgumentValue(string arg, string value)
        {
            if (!Arguments.ContainsKey(arg))
            {
                throw new ArgumentException("unknown option", arg);
            }
            if (value == null)
            {
                throw new ArgumentException("requires value", arg);
            }

            return System.Convert.ChangeType(value, Arguments[arg].Type);
        }

        public class Argument
        {
            public string Name { get; private set; }
            public Type   Type { get; private set; }
            public object DefaultValue { get; private set; }
            public string Help { get; private set; }

            public bool IsRequired { get { return DefaultValue == null; } }

            public Argument(string name, Type type, object defaultValue = null, string help=null)
            {
                this.Name         = name;
                this.Type         = type;
                this.DefaultValue = defaultValue;
                this.Help         = help;
            }
        }
    }
}
