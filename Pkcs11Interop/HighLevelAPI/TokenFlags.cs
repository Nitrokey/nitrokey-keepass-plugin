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

using Net.Pkcs11Interop.Common;

namespace Net.Pkcs11Interop.HighLevelAPI
{
    /// <summary>
    /// Flags indicating capabilities and status of the device
    /// </summary>
    public class TokenFlags
    {
        /// <summary>
        /// Bits flags indicating capabilities and status of the device
        /// </summary>
        private uint _flags;

        /// <summary>
        /// Bits flags indicating capabilities and status of the device
        /// </summary>
        public uint Flags
        {
            get
            {
                return _flags;
            }
        }

        /// <summary>
        /// True if the token has its own random number generator
        /// </summary>
        public bool Rng
        {
            get
            {
                return ((_flags & CKF.CKF_RNG) == CKF.CKF_RNG);
            }
        }

        /// <summary>
        /// True if the token is write-protected
        /// </summary>
        public bool WriteProtected
        {
            get
            {
                return ((_flags & CKF.CKF_WRITE_PROTECTED) == CKF.CKF_WRITE_PROTECTED);
            }
        }

        /// <summary>
        /// True if there are some cryptographic functions that a user must be logged in to perform
        /// </summary>
        public bool LoginRequired
        {
            get
            {
                return ((_flags & CKF.CKF_LOGIN_REQUIRED) == CKF.CKF_LOGIN_REQUIRED);
            }
        }

        /// <summary>
        /// True if the normal user's PIN has been initialized
        /// </summary>
        public bool UserPinInitialized
        {
            get
            {
                return ((_flags & CKF.CKF_USER_PIN_INITIALIZED) == CKF.CKF_USER_PIN_INITIALIZED);
            }
        }

        /// <summary>
        /// True if a successful save of a session's cryptographic operations state always contains all keys needed to restore the state of the session
        /// </summary>
        public bool RestoreKeyNotNeeded
        {
            get
            {
                return ((_flags & CKF.CKF_RESTORE_KEY_NOT_NEEDED) == CKF.CKF_RESTORE_KEY_NOT_NEEDED);
            }
        }

        /// <summary>
        /// True if token has its own hardware clock
        /// </summary>
        public bool ClockOnToken
        {
            get
            {
                return ((_flags & CKF.CKF_CLOCK_ON_TOKEN) == CKF.CKF_CLOCK_ON_TOKEN);
            }
        }
        
        /// <summary>
        /// True if token has a “protected authentication path”, whereby a user can log into the token without passing a PIN through the Cryptoki library
        /// </summary>
        public bool ProtectedAuthenticationPath
        {
            get
            {
                return ((_flags & CKF.CKF_PROTECTED_AUTHENTICATION_PATH) == CKF.CKF_PROTECTED_AUTHENTICATION_PATH);
            }
        }

        /// <summary>
        /// True if a single session with the token can perform dual cryptographic operations
        /// </summary>
        public bool DualCryptoOperations
        {
            get
            {
                return ((_flags & CKF.CKF_DUAL_CRYPTO_OPERATIONS) == CKF.CKF_DUAL_CRYPTO_OPERATIONS);
            }
        }

        /// <summary>
        /// True if the token has been initialized using C_InitializeToken or an equivalent mechanism
        /// </summary>
        public bool TokenInitialized
        {
            get
            {
                return ((_flags & CKF.CKF_TOKEN_INITIALIZED) == CKF.CKF_TOKEN_INITIALIZED);
            }
        }

        /// <summary>
        /// True if the token supports secondary authentication for private key objects
        /// </summary>
        public bool SecondaryAuthentication
        {
            get
            {
                return ((_flags & CKF.CKF_SECONDARY_AUTHENTICATION) == CKF.CKF_SECONDARY_AUTHENTICATION);
            }
        }

        /// <summary>
        /// True if an incorrect user login PIN has been entered at least once since the last successful authentication
        /// </summary>
        public bool UserPinCountLow
        {
            get
            {
                return ((_flags & CKF.CKF_USER_PIN_COUNT_LOW) == CKF.CKF_USER_PIN_COUNT_LOW);
            }
        }

        /// <summary>
        /// True if supplying an incorrect user PIN will make it to become locked
        /// </summary>
        public bool UserPinFinalTry
        {
            get
            {
                return ((_flags & CKF.CKF_USER_PIN_FINAL_TRY) == CKF.CKF_USER_PIN_FINAL_TRY);
            }
        }

        /// <summary>
        /// True if the user PIN has been locked. User login to the token is not possible.
        /// </summary>
        public bool UserPinLocked
        {
            get
            {
                return ((_flags & CKF.CKF_USER_PIN_LOCKED) == CKF.CKF_USER_PIN_LOCKED);
            }
        }

        /// <summary>
        /// True if the user PIN value is the default value set by token initialization or manufacturing, or the PIN has been expired by the card
        /// </summary>
        public bool UserPinToBeChanged
        {
            get
            {
                return ((_flags & CKF.CKF_USER_PIN_TO_BE_CHANGED) == CKF.CKF_USER_PIN_TO_BE_CHANGED);
            }
        }

        /// <summary>
        /// True if an incorrect SO login PIN has been entered at least once since the last successful authentication
        /// </summary>
        public bool SoPinCountLow
        {
            get
            {
                return ((_flags & CKF.CKF_SO_PIN_COUNT_LOW) == CKF.CKF_SO_PIN_COUNT_LOW);
            }
        }

        /// <summary>
        /// True if supplying an incorrect SO PIN will make it to become locked.
        /// </summary>
        public bool SoPinFinalTry
        {
            get
            {
                return ((_flags & CKF.CKF_SO_PIN_FINAL_TRY) == CKF.CKF_SO_PIN_FINAL_TRY);
            }
        }

        /// <summary>
        /// True if the SO PIN has been locked. User login to the token is not possible.
        /// </summary>
        public bool SoPinLocked
        {
            get
            {
                return ((_flags & CKF.CKF_SO_PIN_LOCKED) == CKF.CKF_SO_PIN_LOCKED);
            }
        }

        /// <summary>
        /// True if the SO PIN value is the default value set by token initialization or manufacturing, or the PIN has been expired by the card.
        /// </summary>
        public bool SoPinToBeChanged
        {
            get
            {
                return ((_flags & CKF.CKF_SO_PIN_TO_BE_CHANGED) == CKF.CKF_SO_PIN_TO_BE_CHANGED);
            }
        }

        /// <summary>
        /// Initializes new instance of TokenFlags class
        /// </summary>
        /// <param name="flags">Bits flags indicating capabilities and status of the device</param>
        internal TokenFlags(uint flags)
        {
            _flags = flags;
        }
    }
}
