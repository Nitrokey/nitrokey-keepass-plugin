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
    /// Structure that provides the parameters to the CKM_CMS_SIG mechanism
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CK_CMS_SIG_PARAMS
    {
        /// <summary>
        /// Object handle for a certificate associated with the signing key
        /// </summary>
        public uint CertificateHandle;

        /// <summary>
        /// Mechanism to use when signing a constructed CMS SignedAttributes value
        /// </summary>
        public IntPtr SigningMechanism;

        /// <summary>
        /// Mechanism to use when digesting the data
        /// </summary>
        public IntPtr DigestMechanism;

        /// <summary>
        /// NULL-terminated string indicating complete MIME Content-type of message to be signed or null if the message is a MIME object
        /// </summary>
        public IntPtr ContentType;

        /// <summary>
        /// Pointer to DER-encoded list of CMS Attributes the caller requests to be included in the signed attributes
        /// </summary>
        public IntPtr RequestedAttributes;

        /// <summary>
        /// Length in bytes of the value pointed to by RequestedAttributes
        /// </summary>
        public uint RequestedAttributesLen;

        /// <summary>
        /// Pointer to DER-encoded list of CMS Attributes (with accompanying values) required to be included in the resulting signed attributes
        /// </summary>
        public IntPtr RequiredAttributes;

        /// <summary>
        /// Length in bytes, of the value pointed to by RequiredAttributes
        /// </summary>
        public uint RequiredAttributesLen;
    }
}
