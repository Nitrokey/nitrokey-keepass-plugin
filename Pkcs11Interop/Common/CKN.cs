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

namespace Net.Pkcs11Interop.Common
{
    /// <summary>
    /// Notifications
    /// </summary>
    public enum CKN : uint
    {
        /// <summary>
        /// Cryptoki is surrendering the execution of a function executing in a session so that the application may perform other operations
        /// </summary>
        CKN_SURRENDER = 0,

        /// <summary>
        /// Cryptoki is informing the application that the OTP for a key on a connected token just changed
        /// </summary>
        CKN_OTP_CHANGED = 1
    }
}
