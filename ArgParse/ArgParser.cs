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
        public string Help
        {
            get
            {
                string help = Desc;
                foreach (var arg in Arguments.Values)
                {
                    var argHelp = (arg.Help != null) ? arg.Help : arg.Name.Substring(2).ToUpper();
                    help += String.Format("\n    {0}\t{1}", arg.Name, argHelp);
                }
                return help;
            }
        }


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
                                ToDictionary(x => x.Name, x => x.DefaultValue);

            var specifiedValues = args.Zip(args.Skip(1).Concat(new string[] { null }),
                                           (first, second) => new KeyValuePair<string, string>(first, second)).
                                  Where(e => e.Key.StartsWith("--")).
                                  Select(e => GetOptionalArgumentValue(e.Key, e.Value)).
                                  ToDictionary(e => e.Key, e => e.Value);

            return specifiedValues.Concat(defaultValues.Where(e => !specifiedValues.ContainsKey(e.Key))).
                   ToDictionary(e => e.Key, e => e.Value);
        }

        private KeyValuePair<string, object> GetOptionalArgumentValue(string arg, string value)
        {
            if (!Arguments.ContainsKey(arg))
            {
                throw new ArgumentException("unknown option", arg);
            }
            if (value == null)
            {
                throw new ArgumentException("requires value", arg);
            }

            return new KeyValuePair<string, object>(arg, System.Convert.ChangeType(value, Arguments[arg].Type));
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
