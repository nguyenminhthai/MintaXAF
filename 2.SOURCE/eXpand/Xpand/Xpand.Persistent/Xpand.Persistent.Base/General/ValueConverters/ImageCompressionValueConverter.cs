﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.Metadata;

namespace Xpand.Persistent.Base.General.ValueConverters{
    public class ImageCompressionValueConverter : ValueConverter{
        #region Properties

        public override Type StorageType{
            get { return typeof (byte[]); }
        }

        #endregion

        public override object ConvertToStorageType(object value){
            if (value != null && !(value is Image)){
                throw new ArgumentException();
            }

            if (value == null){
                return null;
            }

            var ms = new MemoryStream();
            ((Image) value).Save(ms, ImageFormat.Jpeg);

            return CompressionUtils.Compress(ms).ToArray();
        }

        public override object ConvertFromStorageType(object value){
            if (value != null && !(value is byte[])){
                throw new ArgumentException();
            }

            if (value == null || ((byte[]) value).Length == 0){
                return value;
            }

            return new ImageConverter().ConvertFrom(
                CompressionUtils.Decompress(new MemoryStream((byte[]) value)).ToArray());
        }
    }
}