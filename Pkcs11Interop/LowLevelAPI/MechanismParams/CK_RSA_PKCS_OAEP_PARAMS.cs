﻿/*
 *  Pkcs11Interop - Open-source .NET wrapper for unmanaged PKCS#11 libraries
 *  Copyright (c) 2012-2013 JWC s.r.o.
 *  Author: Jaroslav Imrich
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3
 *  as published by the Free Software Foundation.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <http://www.gnu.org/licenses/>.
 * 
 *  You can be released from the requirements of the license by purchasing
 *  a commercial license. Buying such a license is mandatory as soon as you
 *  develop commercial activities involving the Pkcs11Interop software without
 *  disclosing the source code of your own applications.
 * 
 *  For more information, please contact JWC s.r.o. at info@pkcs11interop.net
 */

using System;
using System.Runtime.InteropServices;

namespace Net.Pkcs11Interop.LowLevelAPI.MechanismParams
{
    /// <summary>
    /// Structure that provides the parameters to the CKM_RSA_PKCS_OAEP mechanism
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CK_RSA_PKCS_OAEP_PARAMS
    {
        /// <summary>
        /// Mechanism ID of the message digest algorithm used to calculate the digest of the encoding parameter (CKM)
        /// </summary>
        public uint HashAlg;

        /// <summary>
        /// Mask generation function to use on the encoded block (CKG)
        /// </summary>
        public uint Mgf;
        
        /// <summary>
        /// Source of the encoding parameter (CKZ)
        /// </summary>
        public uint Source;
        
        /// <summary>
        /// Data used as the input for the encoding parameter source
        /// </summary>
        public IntPtr SourceData;
        
        /// <summary>
        /// Length of the encoding parameter source input
        /// </summary>
        public uint SourceDataLen;
    }
}
