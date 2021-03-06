/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *************************************************************************************************/
using System;
using System.Xml;
using OfficeOpenXml.Style;
using System.Globalization;
using OfficeOpenXml.Drawing.Interfaces;
using OfficeOpenXml.Drawing.Style.Effect;
using OfficeOpenXml.Drawing.Style.ThreeD;
using OfficeOpenXml.Utils.Extentions;
namespace OfficeOpenXml.Drawing.Chart
{
    /// <summary>
    /// An axis for a chart
    /// </summary>
    public sealed class ExcelChartAxis : XmlHelper, IDrawingStyle, IStyleMandatoryProperties
    {
        /// <summary>
        /// Type of axis
        /// </summary>
        ExcelChart _chart;
        internal ExcelChartAxis(ExcelChart chart, XmlNamespaceManager nameSpaceManager, XmlNode topNode) :
            base(nameSpaceManager, topNode)
        {
            AddSchemaNodeOrder(new string[] { "axId", "scaling", "delete", "axPos", "majorGridlines", "minorGridlines", "title", "numFmt", "majorTickMark", "minorTickMark", "tickLblPos", "spPr", "txPr", "crossAx", "crosses", "crossesAt", "crossBetween", "auto", "lblOffset", "baseTimeUnit", "majorUnit", "majorTimeUnit", "minorUnit", "minorTimeUnit", "tickLblSkip", "tickMarkSkip", "dispUnits", "noMultiLvlLbl", "logBase", "orientation", "max", "min" },
                ExcelDrawing._schemaNodeOrderSpPr);

            _chart = chart;
        }
        internal string Id
        {
            get
            {
                return GetXmlNodeString("c:axId/@val");
            }
        }
        const string _majorTickMark = "c:majorTickMark/@val";
        /// <summary>
        /// Get or Sets the major tick marks for the axis. 
        /// </summary>
        public eAxisTickMark MajorTickMark
        {
            get
            {
                var v=GetXmlNodeString(_majorTickMark);
                if(string.IsNullOrEmpty(v))
                {
                    return eAxisTickMark.Cross;
                }
                else
                {
                    try
                    {
                        return (eAxisTickMark)Enum.Parse( typeof( eAxisTickMark ), v );
                    }
                    catch
                    {
                        return eAxisTickMark.Cross;
                    }
                }
            }
            set
            {
                SetXmlNodeString( _majorTickMark, value.ToString().ToLower(CultureInfo.InvariantCulture) );
            }
        }

        const string _minorTickMark = "c:minorTickMark/@val";
        /// <summary>
        /// Get or Sets the minor tick marks for the axis. 
        /// </summary>
        public eAxisTickMark MinorTickMark
        {
            get
            {
                var v=GetXmlNodeString(_minorTickMark);
                if(string.IsNullOrEmpty(v))
                {
                    return eAxisTickMark.Cross;
                }
                else
                {
                    try
                    {
                        return (eAxisTickMark)Enum.Parse(typeof(eAxisTickMark), v );
                    }
                    catch
                    {
                        return eAxisTickMark.Cross;
                    }
                }
            }
            set
            {
                SetXmlNodeString(_minorTickMark, value.ToString().ToLower(CultureInfo.InvariantCulture));
            }
        }
         /// <summary>
        /// The type of axis
        /// </summary>
        internal eAxisType AxisType
        {
            get
            {
                try
                {
                    var axType = TopNode.LocalName.Substring(0, TopNode.LocalName.Length - 2);
                    if (axType == "ser") return eAxisType.Serie;
                    return (eAxisType)Enum.Parse(typeof(eAxisType), axType , true);
                }
                catch
                {
                    return eAxisType.Val;
                }
            }
        }
        private string AXIS_POSITION_PATH = "c:axPos/@val";
        /// <summary>
        /// Where the axis is located
        /// </summary>
        public eAxisPosition AxisPosition
        {
            get
            {                
                switch(GetXmlNodeString(AXIS_POSITION_PATH))
                {
                    case "b":
                        return eAxisPosition.Bottom;
                    case "r":
                        return eAxisPosition.Right;
                    case "t":
                        return eAxisPosition.Top;
                    default: 
                        return eAxisPosition.Left;
                }
            }
            internal set
            {
                SetXmlNodeString(AXIS_POSITION_PATH, value.ToString().ToLower(CultureInfo.InvariantCulture).Substring(0,1));
            }
        }
        const string _crossesPath = "c:crosses/@val";
        /// <summary>
        /// Where the axis crosses
        /// </summary>
        public eCrosses Crosses
        {
            get
            {
                var v=GetXmlNodeString(_crossesPath);
                if (string.IsNullOrEmpty(v))
                {
                    return eCrosses.AutoZero;
                }
                else
                {
                    try
                    {
                        return (eCrosses)Enum.Parse(typeof(eCrosses), v, true);
                    }
                    catch
                    {
                        return eCrosses.AutoZero;
                    }
                }
            }
            set
            {
                var v = value.ToString();
                v = v.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + v.Substring(1, v.Length - 1);
                SetXmlNodeString(_crossesPath, v);
            }

        }
        const string _crossBetweenPath = "c:crossBetween/@val";
        /// <summary>
        /// How the axis are crossed
        /// </summary>
        public eCrossBetween CrossBetween
        {
            get
            {
                var v=GetXmlNodeString(_crossBetweenPath);
                if(string.IsNullOrEmpty(v))
                {
                    return eCrossBetween.Between;
                }
                else
                {
                    try
                    {
                        return (eCrossBetween)Enum.Parse(typeof(eCrossBetween), v, true);
                    }
                    catch
                    {
                        return eCrossBetween.Between;
                    }
                }
            }
            set
            {
                var v = value.ToString();
                v = v.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + v.Substring(1);
                SetXmlNodeString(_crossBetweenPath, v);
            }
        }
        const string _crossesAtPath = "c:crossesAt/@val";
        /// <summary>
        /// The value where the axis cross. 
        /// Null is automatic
        /// </summary>
        public double? CrossesAt
        {
            get
            {
                return GetXmlNodeDoubleNull(_crossesAtPath);
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_crossesAtPath, true);
                }
                else
                {
                    SetXmlNodeString(_crossesAtPath, ((double)value).ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        const string _formatPath = "c:numFmt/@formatCode";
        /// <summary>
        /// The Numberformat used
        /// </summary>
        public string Format 
        {
            get
            {
                return GetXmlNodeString(_formatPath);
            }
            set
            {
                SetXmlNodeString(_formatPath,value);
                if(string.IsNullOrEmpty(value))
                {
                    SourceLinked = true;
                }
                else
                {
                    SourceLinked = false;
                }
            }
        }
        const string _sourceLinkedPath = "c:numFmt/@sourceLinked";
        /// <summary>
        /// The Numberformats are linked to the source data.
        /// </summary>
        public bool SourceLinked
        {
            get
            {
                return GetXmlNodeBool(_sourceLinkedPath);
            }
            set
            {
                SetXmlNodeBool(_sourceLinkedPath, value);
            }
        }
        const string _lblPos = "c:tickLblPos/@val";
        /// <summary>
        /// The Position of the labels
        /// </summary>
        public eTickLabelPosition LabelPosition
        {
            get
            {
                var v=GetXmlNodeString(_lblPos);
                if (string.IsNullOrEmpty(v))
                {
                    return eTickLabelPosition.NextTo;
                }
                else
                {
                    try
                    {
                        return (eTickLabelPosition)Enum.Parse(typeof(eTickLabelPosition), v, true);
                    }
                    catch
                    {
                        return eTickLabelPosition.NextTo;
                    }
                }
            }
            set
            {
                string lp = value.ToString();
                SetXmlNodeString(_lblPos, lp.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + lp.Substring(1, lp.Length - 1));
            }
        }
        ExcelDrawingFill _fill = null;
        /// <summary>
        /// Access to fill properties
        /// </summary>
        public ExcelDrawingFill Fill
        {
            get
            {
                if (_fill == null)
                {
                    _fill = new ExcelDrawingFill(_chart, NameSpaceManager, TopNode, "c:spPr", SchemaNodeOrder);
                }
                return _fill;
            }
        }
        ExcelDrawingBorder _border = null;
        /// <summary>
        /// Access to border properties
        /// </summary>
        public ExcelDrawingBorder Border
        {
            get
            {
                if (_border == null)
                {
                    _border = new ExcelDrawingBorder(_chart, NameSpaceManager, TopNode, "c:spPr/a:ln", SchemaNodeOrder);
                }
                return _border;
            }
        }
        ExcelDrawingEffectStyle _effect = null;
        /// <summary>
        /// Effects
        /// </summary>
        public ExcelDrawingEffectStyle Effect
        {
            get
            {
                if (_effect == null)
                {
                    _effect = new ExcelDrawingEffectStyle(_chart, NameSpaceManager, TopNode, "c:spPr/a:effectLst", SchemaNodeOrder);
                }
                return _effect;
            }
        }
        ExcelDrawing3D _threeD = null;
        /// <summary>
        /// 3D properties
        /// </summary>
        public ExcelDrawing3D ThreeD
        {
            get
            {
                if (_threeD == null)
                {
                    _threeD = new ExcelDrawing3D(NameSpaceManager, TopNode, "c:spPr", SchemaNodeOrder);
                }
                return _threeD;
            }
        }

        ExcelTextFont _font = null;
        /// <summary>
        /// Access to font properties
        /// </summary>
        public ExcelTextFont Font
        {
            get
            {
                if (_font == null)
                {
                    _font = new ExcelTextFont(_chart, NameSpaceManager, TopNode, "c:txPr/a:p/a:pPr/a:defRPr", SchemaNodeOrder);
                }
                return _font;
            }
        }
        ExcelTextBody _textBody = null;
        /// <summary>
        /// Access to text body properties
        /// </summary>
        public ExcelTextBody TextBody
        {
            get
            {
                if (_textBody == null)
                {
                    _textBody = new ExcelTextBody(NameSpaceManager, TopNode, "c:txPr/a:bodyPr", SchemaNodeOrder);
                }
                return _textBody;
            }
        }
        void IDrawingStyleBase.CreatespPr()
        {
            CreatespPrNode();
        }

        /// <summary>
        /// If the axis is deleted
        /// </summary>
        public bool Deleted 
        {
            get
            {
                return GetXmlNodeBool("c:delete/@val");
            }
            set
            {
                SetXmlNodeBool("c:delete/@val", value);
            }
        }
        const string _ticLblPos_Path = "c:tickLblPos/@val";
        /// <summary>
        /// Position of the Lables
        /// </summary>
        public eTickLabelPosition TickLabelPosition 
        {
            get
            {
                string v = GetXmlNodeString(_ticLblPos_Path);
                if (v == "")
                {
                    return eTickLabelPosition.None;
                }
                else
                {
                    return (eTickLabelPosition)Enum.Parse(typeof(eTickLabelPosition), v, true);
                }
            }
            set
            {
                string v = value.ToString();
                v=v.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + v.Substring(1, v.Length - 1);
                SetXmlNodeString(_ticLblPos_Path,v);
            }
        }
        const string _displayUnitPath = "c:dispUnits/c:builtInUnit/@val";
        const string _custUnitPath = "c:dispUnits/c:custUnit/@val";
        /// <summary>
        /// The scaling value of the display units for the value axis
        /// </summary>
        public double DisplayUnit
        {
            get
            {
                string v = GetXmlNodeString(_displayUnitPath);
                if (string.IsNullOrEmpty(v))
                {
                    var c = GetXmlNodeDoubleNull(_custUnitPath);
                    if (c == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return c.Value;
                    }
                }
                else
                {
                    try
                    {
                        return (double)(long)Enum.Parse(typeof(eBuildInUnits), v, true);
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            set
            {
                if (AxisType == eAxisType.Val && value>=0)
                {
                    foreach(var v in Enum.GetValues(typeof(eBuildInUnits)))
                    {
                        if((double)(long)v==value)
                        {
                            DeleteNode(_custUnitPath, true);
                            SetXmlNodeString(_displayUnitPath, ((eBuildInUnits)value).ToString());
                            return;
                        }
                    }
                    DeleteNode(_displayUnitPath, true);
                    if(value!=0)
                    {
                        SetXmlNodeString(_custUnitPath, value.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }        
        ExcelChartTitle _title = null;
        /// <summary>
        /// Chart axis title
        /// </summary>
        public ExcelChartTitle Title
        {
            get
            {
                if (_title == null)
                {
                    var node = TopNode.SelectSingleNode("c:title", NameSpaceManager);
                    if (node == null)
                    {
                        CreateNode("c:title");
                        node = TopNode.SelectSingleNode("c:title", NameSpaceManager);
                        node.InnerXml = ExcelChartTitle.GetInitXml();
                    }
                    _title = new ExcelChartTitle(_chart, NameSpaceManager, TopNode);
                }
                return _title;
            }            
        }
        #region "Scaling"
        const string _minValuePath = "c:scaling/c:min/@val";
        /// <summary>
        /// Minimum value for the axis.
        /// Null is automatic
        /// </summary>
        public double? MinValue
        {
            get
            {
                return GetXmlNodeDoubleNull(_minValuePath);
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_minValuePath,true);
                }
                else
                {
                    SetXmlNodeString(_minValuePath, ((double)value).ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        const string _maxValuePath = "c:scaling/c:max/@val";
        /// <summary>
        /// Max value for the axis.
        /// Null is automatic
        /// </summary>
        public double? MaxValue
        {
            get
            {
                return GetXmlNodeDoubleNull(_maxValuePath);
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_maxValuePath, true);
                }
                else
                {
                    SetXmlNodeString(_maxValuePath, ((double)value).ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        const string _majorUnitPath = "c:majorUnit/@val";
        const string _majorUnitCatPath = "c:tickLblSkip/@val";
        /// <summary>
        /// Major unit for the axis.
        /// Null is automatic
        /// </summary>
        public double? MajorUnit
        {
            get
            {
                if (AxisType == eAxisType.Cat)
                {
                    return GetXmlNodeDoubleNull(_majorUnitCatPath);
                }
                else
                {
                    return GetXmlNodeDoubleNull(_majorUnitPath);
                }
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_majorUnitPath, true);
                    DeleteNode(_majorUnitCatPath, true);
                }
                else
                {
                    if (AxisType == eAxisType.Cat)
                    {
                        SetXmlNodeString(_majorUnitCatPath, ((double)value).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        SetXmlNodeString(_majorUnitPath, ((double)value).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }
        const string _majorTimeUnitPath = "c:majorTimeUnit/@val";
        /// <summary>
        /// Major time unit for the axis.
        /// Null is automatic
        /// </summary>
        public eTimeUnit? MajorTimeUnit
        {
            get
            {
                var v=GetXmlNodeString(_majorTimeUnitPath); 
                if(string.IsNullOrEmpty(v))
                {
                    return null;
                }
                else
                {
                    return v.ToEnum(eTimeUnit.Years);
                }
            }
            set
            {
                if (value.HasValue)
                {
                    SetXmlNodeString(_majorTimeUnitPath, value.ToEnumString());
                }
                else
                {
                    DeleteNode(_majorTimeUnitPath, true);
                }
            }
        }
        const string _minorUnitPath = "c:minorUnit/@val";
        const string _minorUnitCatPath = "c:tickMarkSkip/@val";
        /// <summary>
        /// Minor unit for the axis.
        /// Null is automatic
        /// </summary>
        public double? MinorUnit
        {
            get
            {
                if (AxisType == eAxisType.Cat)
                {
                    return GetXmlNodeDoubleNull(_minorUnitCatPath);
                }
                else
                {
                    return GetXmlNodeDoubleNull(_minorUnitPath);
                }
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_minorUnitPath, true);
                    DeleteNode(_minorUnitCatPath, true);
                }
                else
                {
                    if (AxisType == eAxisType.Cat)
                    {
                        SetXmlNodeString(_minorUnitCatPath, ((double)value).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        SetXmlNodeString(_minorUnitPath, ((double)value).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }
        const string _minorTimeUnitPath = "c:minorTimeUnit/@val";
        /// <summary>
        /// Minor time unit for the axis.
        /// Null is automatic
        /// </summary>
        public eTimeUnit? MinorTimeUnit
        {
            get
            {
                var v = GetXmlNodeString(_minorTimeUnitPath);
                if (string.IsNullOrEmpty(v))
                {
                    return null;
                }
                else
                {
                    return v.ToEnum(eTimeUnit.Years);
                }
            }
            set
            {
                if (value.HasValue)
                {
                    SetXmlNodeString(_minorTimeUnitPath, value.ToEnumString());
                }
                else
                {
                    DeleteNode(_minorTimeUnitPath);
                }
            }
        }
        const string _logbasePath = "c:scaling/c:logBase/@val";
        /// <summary>
        /// The base for a logaritmic scale
        /// Null for a normal scale
        /// </summary>
        public double? LogBase
        {
            get
            {
                return GetXmlNodeDoubleNull(_logbasePath);
            }
            set
            {
                if (value == null)
                {
                    DeleteNode(_logbasePath, true);
                }
                else
                {
                    double v = ((double)value);
                    if (v < 2 || v > 1000)
                    {
                        throw(new ArgumentOutOfRangeException("Value must be between 2 and 1000"));
                    }
                    SetXmlNodeString(_logbasePath, v.ToString("0.0", CultureInfo.InvariantCulture));
                }
            }
        }
        const string _orientationPath = "c:scaling/c:orientation/@val";
        /// <summary>
        /// Axis orientation
        /// </summary>
        public eAxisOrientation Orientation
        {
            get
            {
                string v = GetXmlNodeString(_orientationPath);
                if (v == "")
                {
                    return eAxisOrientation.MinMax;
                }
                else
                {
                    return (eAxisOrientation)Enum.Parse(typeof(eAxisOrientation), v, true);
                }
            }
            set
            {
                string s=value.ToString();
                s=s.Substring(0,1).ToLower(CultureInfo.InvariantCulture) + s.Substring(1,s.Length-1);
                SetXmlNodeString(_orientationPath, s);
            }
        }
        #endregion

        #region GridLines 
        const string _majorGridlinesPath = "c:majorGridlines"; 
        ExcelDrawingBorder _majorGridlines = null; 
  
        /// <summary> 
        /// Major gridlines for the axis 
        /// </summary> 
        public ExcelDrawingBorder MajorGridlines
        { 
        get 
            { 
                if (_majorGridlines == null) 
                {  
                    _majorGridlines = new ExcelDrawingBorder(_chart, NameSpaceManager, TopNode,$"{_majorGridlinesPath}/c:spPr/a:ln", SchemaNodeOrder); 
                } 
                return _majorGridlines; 
            } 
        }
        ExcelDrawingEffectStyle _majorGridlineEffects = null;
        /// <summary> 
        /// Effects for major gridlines for the axis 
        /// </summary> 
        public ExcelDrawingEffectStyle MajorGridlineEffects
        {
            get
            {
                if (_majorGridlineEffects == null)
                {
                    _majorGridlineEffects = new ExcelDrawingEffectStyle(_chart, NameSpaceManager, TopNode, $"{_majorGridlinesPath}/c:spPr/a:effectLst", SchemaNodeOrder);
                }
                return _majorGridlineEffects;
            }
        }

        const string _minorGridlinesPath = "c:minorGridlines"; 
        ExcelDrawingBorder _minorGridlines = null; 
  
        /// <summary> 
        /// Minor gridlines for the axis 
        /// </summary> 
        public ExcelDrawingBorder MinorGridlines
        { 
            get 
            { 
                if (_minorGridlines == null) 
                {  
                    _minorGridlines = new ExcelDrawingBorder(_chart, NameSpaceManager, TopNode,$"{_minorGridlinesPath}/c:spPr/a:ln", SchemaNodeOrder); 
                } 
                return _minorGridlines; 
            } 
        }
        ExcelDrawingEffectStyle _minorGridlineEffects = null;
        /// <summary> 
        /// Effects for minor gridlines for the axis 
        /// </summary> 
        public ExcelDrawingEffectStyle MinorGridlineEffects
        {
            get
            {
                if (_minorGridlineEffects == null)
                {
                    _minorGridlineEffects = new ExcelDrawingEffectStyle(_chart, NameSpaceManager, TopNode, $"{_minorGridlinesPath}/c:spPr/a:effectLst", SchemaNodeOrder);
                }
                return _minorGridlineEffects;
            }
        }
        /// <summary>
        /// True if the axis has major Gridlines
        /// </summary>
        public bool HasMajorGridlines
        {
            get
            {
                return ExistNode(_majorGridlinesPath);
            }
        }
        /// <summary>
        /// True if the axis has minor Gridlines
        /// </summary>
        public bool HasMinorGridlines
        {
            get
            {
                return ExistNode(_minorGridlinesPath);
            }
        }        
        /// <summary> 
        /// Removes Major and Minor gridlines from the Axis 
        /// </summary> 
        public void RemoveGridlines()
        { 
            RemoveGridlines(true,true); 
        }
        /// <summary>
        ///  Removes gridlines from the Axis
        /// </summary>
        /// <param name="removeMajor">Indicates if the Major gridlines should be removed</param>
        /// <param name="removeMinor">Indicates if the Minor gridlines should be removed</param>
        public void RemoveGridlines(bool removeMajor, bool removeMinor)
        { 
            if (removeMajor) 
            { 
                DeleteNode(_majorGridlinesPath); 
                _majorGridlines = null; 
            } 
  
            if (removeMinor) 
            { 
                DeleteNode(_minorGridlinesPath); 
                _minorGridlines = null; 
            } 
        }

        #endregion
        void IStyleMandatoryProperties.SetMandatoryProperties()
        {
            TextBody.Anchor = eTextAnchoringType.Center;
            TextBody.AnchorCenter = true;
            TextBody.WrapText = eTextWrappingType.Square;
            TextBody.VerticalTextOverflow = eTextVerticalOverflow.Ellipsis;
            TextBody.ParagraphSpacing = true;
            TextBody.Rotation = 0;

            if (Font.Kerning == 0) Font.Kerning = 12;
            Font.Bold = Font.Bold; //Must be set

            CreatespPrNode();
        }
    }
}
