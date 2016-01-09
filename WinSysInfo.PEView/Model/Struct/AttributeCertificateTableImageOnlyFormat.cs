﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// Attribute certificates can be associated with an image by adding an attribute certificate table.
    /// The attribute certificate table is composed of a set of contiguous, octaword-aligned attribute
    /// certificate entries. Each attribute certificate entry contains the following fields.
    /// <see cref="http://blogs.msdn.com/b/ieinternals/archive/2014/09/04/personalizing-installers-using-unauthenticated-data-inside-authenticode-signed-binaries.aspx"/>
    /// </summary>
    public class AttributeCertificateTableImageOnlyFormat
    {
        /// <summary>
        /// Specifies the length of Certificate
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// Contains the certificate version number. For details, see the following text.
        /// </summary>
        public ushort Revision { get; set; }

        /// <summary>
        /// Specifies the type of content in Certificate. For details, see the following text.
        /// </summary>
        public ushort CertificateType { get; set; }

        /// <summary>
        /// Contains a certificate, such as an Authenticode signature. Length is specified by <see cref="Length"/>
        /// </summary>
        public char[] Certificate { get; set; }
    }
}
