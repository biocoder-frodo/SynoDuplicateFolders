using System.Configuration;
using SynoDuplicateFolders.Configuration;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Extensions;
using System.Drawing;
using System.Linq;

namespace SynoDuplicateFolders.Properties
{
    internal class ChartLegend : ConfigurationElement, IElementProvider, IChartLegend
    {
        public ChartLegend() : base()
        {
        }
        public ChartLegend(KnownColor k) : this()
        {
            ColorName = k.ToString();
        }
        public ChartLegend(string key, KnownColor k) : this()
        {
            Key = key;
            ColorName = k.ToString();
        }
        public ChartLegend(string key, Color k, bool forceKnownColor=false) : this()
        {
            Key = key;
            if (forceKnownColor)
            {
                ColorName = k.ToClosestKnownColor().ToString();
            }
            else
            {
                ColorName = k.ToArgb().ToString();
            }
        }

        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)this["Key"];
            }
            set
            {
                this["Key"] = value;
            }
        }

        public Color DefaultColor
        {
            get
            {
                return Color.FromName(DefaultColorName); 
            }
        }
        [ConfigurationProperty("Color", IsRequired = false, DefaultValue = "Black")]
        public string ColorName
        {
            get
            {

                return (string)this["Color"] ?? DefaultColorName;
            }
            set
            {
                this["Color"] = value;
            }
        }
        public Color Color
        {
            get
            {
                KnownColor k = default(KnownColor);
                if (System.Enum.TryParse(ColorName, out k))
                {
                    return Color.FromKnownColor(k);
                }
                else
                {
                    return DefaultColor;
                }
            }
            set { ColorName = value.ToArgb().ToString(); }
        }

        public object GetElementKey()
        {
            return Key.ToString();
        }

        public string GetElementName()
        {
            return "ChartLegend";
        }
        private string DefaultColorName
        {
            get
            {
                //var test = GetType().GetProperties();//.GetMember("ColorName", System.Reflection.MemberTypes.Property,System.Reflection.BindingFlags.GetProperty);
                return (string)GetType().GetProperty("ColorName").CustomAttributes.First().NamedArguments.Single(a => a.MemberName.Equals("DefaultValue")).TypedValue.Value;
            }
        }

        public string Name
        {
            get
            {
                return ColorName; 
            }

            set
            {
                ColorName = value;
            }
        }
    }


}