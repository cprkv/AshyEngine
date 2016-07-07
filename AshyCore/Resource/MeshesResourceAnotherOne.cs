using System;
using System.Globalization;
using AshyCommon.Math;
using IniParser;
using IniParser.Model;

namespace AshyCore.Resource
{
    public class MeshesResourceAnotherOne : Resource
    {
        #region Properties

        public static readonly string FileExtension = "aobj";

        private static CultureInfo _parsingCulture = null;

        private static CultureInfo GetParsingCulture
        {
            get
            {
                if (_parsingCulture == null)
                {
                    _parsingCulture = (CultureInfo) CultureInfo.CurrentCulture.Clone();
                    _parsingCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                }
                return (_parsingCulture);
            }
        }

        #endregion


        #region Constructors

        public MeshesResourceAnotherOne(string path, ResourceTarget target, VFS.IFileSystem fs)
            : base(path, target, fs)
        {
        }

        #endregion


        #region Methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var parser =        new FileIniDataParser();
            var fullPath = $"{path}.{FileExtension}";
            var data =          parser.ReadFile(fullPath);

            var vLen                    = int.Parse(data["props"]["v_len"]);
            var vtLen                   = int.Parse(data["props"]["vt_len"]);
            var vnLen                   = int.Parse(data["props"]["vn_len"]);
            var fLen                    = int.Parse(data["props"]["f_len"]);
            var useTextCoord            = int.Parse(data["props"]["use_text_coord"]) == 1;

            var vert                    = ParseVec3Block(vLen, data, "v");
            var uvw                     = ParseVec3Block(vtLen, data, "vt");
            var normals                 = ParseVec3Block(vnLen, data, "v");
            var vertexIndices           = new uint[fLen];
            var normalIndices           = new uint[fLen];
            var uvwIndices              = (useTextCoord) ? new uint[fLen] : null;
            

            for (int i = 0; i < fLen; i++)
            {
                var str = data["f"]["f" + i]
                    .Split(new[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (useTextCoord)
                {
                    vertexIndices[i] =  (uint.Parse(str[0]) - 1);
                    uvwIndices[i] =     (uint.Parse(str[1]) - 1);
                    normalIndices[i] =  (uint.Parse(str[2]) - 1);

                    vertexIndices[i] =  (uint.Parse(str[3]) - 1);
                    uvwIndices[i] =     (uint.Parse(str[4]) - 1);
                    normalIndices[i] =  (uint.Parse(str[5]) - 1);

                    vertexIndices[i] =  (uint.Parse(str[6]) - 1);
                    uvwIndices[i] =     (uint.Parse(str[7]) - 1);
                    normalIndices[i] =  (uint.Parse(str[8]) - 1);
                }
                else
                {
                    vertexIndices[i] =  (uint.Parse(str[0]) - 1);
                    normalIndices[i] =  (uint.Parse(str[1]) - 1);

                    vertexIndices[i] =  (uint.Parse(str[2]) - 1);
                    normalIndices[i] =  (uint.Parse(str[3]) - 1);

                    vertexIndices[i] =  (uint.Parse(str[4]) - 1);
                    normalIndices[i] =  (uint.Parse(str[5]) - 1);
                }
                
            }

            if (!useTextCoord)
            {
                uvwIndices = vertexIndices;
                uvw = vert;
            }

            return new Mesh(
                vert,
                uvw,
                normals,
                vertexIndices,
                uvwIndices,
                normalIndices
                );
        }

        private Vec3[] ParseVec3Block(int len, IniData data, string tag)
        {
            var saver = new Vec3[len];
            for (int i = 0; i < len; i++)
            {
                var str = data[tag][tag + i]
                    .Split(new[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var x = float.Parse(str[0], NumberStyles.Any, GetParsingCulture);
                var y = float.Parse(str[1], NumberStyles.Any, GetParsingCulture);
                var z = 0.0f;

                if (tag == "vt")
                {
                    z = float.Parse(str[2], NumberStyles.Any, GetParsingCulture);
                }

                saver[i] = new Vec3(x, y, z);
            }
            return saver;
        }

        #endregion

    }
}
