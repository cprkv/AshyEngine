using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCore.Resource
{
    public class MeshesResourceBin : Resource
    {
        #region Properties

        public static readonly string FileExtension = "obj";

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

        public MeshesResourceBin(string path, ResourceTarget target, VFS.IFileSystem fs)
            : base(path, target, fs)
        {
        }

        #endregion


        #region Methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))



            return null;
        }

        #endregion
    }
}
