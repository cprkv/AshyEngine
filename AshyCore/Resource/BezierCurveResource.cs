using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AshyCommon.Math;
using IniParser;

namespace AshyCore.Resource
{
    class BezierCurveResource : Resource
    {
        #region Properties

        public static readonly string FileExtension = "ini";

        private static CultureInfo _parsingCulture = null;

        private static CultureInfo GetParsingCulture
        {
            get
            {
                if (_parsingCulture == null)
                {
                    _parsingCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    _parsingCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                }
                return (_parsingCulture);
            }
        }

        #endregion

        #region Constructors

        public BezierCurveResource(string path, ResourceTarget target, VFS.IFileSystem fs)
            : base(path, target, fs)
        {
        }

        #endregion


        #region Methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var confResource = new ConfigResource(path, Target, fs);
            var data = (ConfigTable) confResource.RC;

            var length = int.Parse(data["curve"]["amount"]);
            var  segments = new BezierCurve.Segment[length];

            for (int i = 0; i < length; i++)
            {
                var start = Vec3.Parse(data["s" + i]["start"]);
                var end = Vec3.Parse(data["s" + i]["end"]);
                var dir_start = Vec3.Parse(data["s" + i]["dir_start"]);
                var dir_end = Vec3.Parse(data["s" + i]["dir_end"]);
                segments[i] = new BezierCurve.Segment(start, end, dir_start, dir_end);
            }


            return new BezierCurve(segments.ToList(), 10);
        }
        
        #endregion
    }
}
