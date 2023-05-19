using System.Configuration;
using SynoDuplicateFolders.Configuration;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Extensions;
using System.Drawing;
using System.Linq;
using System;

namespace SynoDuplicateFolders.Properties
{
    internal class ChartLegend : ConfigurationElement, IElementProvider, IChartLegend
    {
        private static readonly string _defaultColorName = (string)typeof(ChartLegend).GetProperty("ColorName").CustomAttributes.First().NamedArguments.Single(a => a.MemberName.Equals("DefaultValue")).TypedValue.Value;
        private static readonly Color _defaultColor = Color.FromName(_defaultColorName);
        private string _dcn = null;
        private Color _dc = _defaultColor;
        private Color _color;
        private bool _sync = true;
        public ChartLegend() : base()
        {
            _color = ColorTranslator.FromHtml((string)this["Color"]);
        }
        public ChartLegend(KnownColor k) : this()
        {
            Color = Color.FromKnownColor(k);
        }
        public ChartLegend(string key, KnownColor k) : this(k)
        {
            Key = key;
        }
        public ChartLegend(string key, Color k, bool forceKnownColor = false) : this()
        {
            Key = key;
            ColorName = forceKnownColor ? k.ToClosestKnownColor().ToString() : ColorTranslator.ToHtml(k);
        }

        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get => (string)this["Key"];

            set => this["Key"] = value;
        }

        [ConfigurationProperty("Color", IsRequired = false, DefaultValue = "Black")]
        public string ColorName
        {
            get => (string)this["Color"] ?? DefaultColorName;

            set
            {
                this["Color"] = value;
                try
                {
                    if (ColorTranslator.ToHtml(ColorTranslator.FromHtml(value)) != value)
                    {
                        this["Color"] = DefaultColorName;
                    }
                }
                catch (Exception)
                {
                    this["Color"] = DefaultColorName;
                }
                _color = ColorTranslator.FromHtml((string)this["Color"]);
            }
        }
        public Color Color
        {
            get {
                if (_sync)
                {
                    _color = ColorTranslator.FromHtml(ColorName);
                    _sync = false;
                }
                return _color; 
            }

            set
            {
                if (_color != value)
                {
                    _color = value;
                    ColorName = ColorTranslator.ToHtml(_color);
                }
            }
        }
        public string DefaultColorName
        {
            get { return _dcn == null ? _defaultColorName : _dcn; }
            set
            {
                if (Enum.TryParse(value, out KnownColor k))
                {
                    _dc = Color.FromKnownColor(k);
                    _dcn = value;
                }
                else
                {
                    _dc = _defaultColor;
                    _dcn = _defaultColorName;
                }
            }
        }
        public Color DefaultColor => _dc;

        object IElementProvider.GetElementKey()
        {
            return Key.ToString();
        }

        string IElementProvider.GetElementName()
        {
            return "ChartLegend";
        }
    }
}