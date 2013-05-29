/*
  CryptokiKeyPrivder - A PKCS#11 Plugin for Keepass
  Copyright (C) 2013 Daniel Pieper <daniel.pieper@implogy.de>

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using CK_ULONG = System.UInt32;
using CK_VOID_PTR = System.IntPtr;
using CK_BBOOL = System.Boolean;
using CK_SLOT_ID = System.UInt32;           //CK_ULONG;
using CK_OBJECT_CLASS = System.UInt32;      //CK_ULONG;
using CK_SESSION_HANDLE = System.UInt32;    //CK_ULONG;
using CK_FLAGS = System.UInt32;             //CK_ULONG;
using CK_NOTIFY = System.IntPtr;            //Callback function, not used in this sample
using CK_OBJECT_HANDLE = System.UInt32;     //CK_ULONG;
using CK_ATTRIBUTE_TYPE = System.UInt32;    //CK_ULONG;
using CK_MECHANISM_TYPE = System.UInt32;    //CK_ULONG;
using CK_BYTE = System.Byte;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Reflection;
using CryptokiKeyProvider.Forms;
using System.Windows.Forms;
using KeePass.UI;

namespace cryptotest
{

    class pkcs11
    {
        public enum ReturnValues : uint
        {
            CKR_OK                              = 0x00000000,
            CKR_CANCEL                          = 0x00000001,
            CKR_HOST_MEMORY                     = 0x00000002,
            CKR_SLOT_ID_INVALID                 = 0x00000003,
            CKR_GENERAL_ERROR                   = 0x00000005,
            CKR_FUNCTION_FAILED                 = 0x00000006,
            CKR_ARGUMENTS_BAD                   = 0x00000007,
            CKR_NO_EVENT                        = 0x00000008,
            CKR_NEED_TO_CREATE_THREADS          = 0x00000009,
            CKR_CANT_LOCK                       = 0x0000000A,
            CKR_ATTRIBUTE_READ_ONLY             = 0x00000010,
            CKR_ATTRIBUTE_SENSITIVE             = 0x00000011,
            CKR_ATTRIBUTE_TYPE_INVALID          = 0x00000012,
            CKR_ATTRIBUTE_VALUE_INVALID         = 0x00000013,
            CKR_DATA_INVALID                    = 0x00000020,
            CKR_DATA_LEN_RANGE                  = 0x00000021,
            CKR_DEVICE_ERROR                    = 0x00000030,
            CKR_DEVICE_MEMORY                   = 0x00000031,
            CKR_DEVICE_REMOVED                  = 0x00000032,
            CKR_ENCRYPTED_DATA_INVALID          = 0x00000040,
            CKR_ENCRYPTED_DATA_LEN_RANGE        = 0x00000041,
            CKR_FUNCTION_CANCELED               = 0x00000050,
            CKR_FUNCTION_NOT_PARALLEL           = 0x00000051,
            CKR_FUNCTION_NOT_SUPPORTED          = 0x00000054,
            CKR_KEY_HANDLE_INVALID              = 0x00000060,
            CKR_KEY_SIZE_RANGE                  = 0x00000062,
            CKR_KEY_TYPE_INCONSISTENT           = 0x00000063,
            CKR_KEY_NOT_NEEDED                  = 0x00000064,
            CKR_KEY_CHANGED                     = 0x00000065,
            CKR_KEY_NEEDED                      = 0x00000066,
            CKR_KEY_INDIGESTIBLE                = 0x00000067,
            CKR_KEY_FUNCTION_NOT_PERMITTED      = 0x00000068,
            CKR_KEY_NOT_WRAPPABLE               = 0x00000069,
            CKR_KEY_UNEXTRACTABLE               = 0x0000006A,
            CKR_MECHANISM_INVALID               = 0x00000070,
            CKR_MECHANISM_PARAM_INVALID         = 0x00000071,
            CKR_OBJECT_HANDLE_INVALID           = 0x00000082,
            CKR_OPERATION_ACTIVE                = 0x00000090,
            CKR_OPERATION_NOT_INITIALIZED       = 0x00000091,
            CKR_PIN_INCORRECT                   = 0x000000A0,
            CKR_PIN_INVALID                     = 0x000000A1,
            CKR_PIN_LEN_RANGE                   = 0x000000A2,
            CKR_PIN_EXPIRED                     = 0x000000A3,
            CKR_PIN_LOCKED                      = 0x000000A4,
            CKR_SESSION_CLOSED                  = 0x000000B0,
            CKR_SESSION_COUNT                   = 0x000000B1,
            CKR_SESSION_HANDLE_INVALID          = 0x000000B3,
            CKR_SESSION_PARALLEL_NOT_SUPPORTED  = 0x000000B4,
            CKR_SESSION_READ_ONLY               = 0x000000B5,
            CKR_SESSION_EXISTS                  = 0x000000B6,
            CKR_SESSION_READ_ONLY_EXISTS        = 0x000000B7,
            CKR_SESSION_READ_WRITE_SO_EXISTS    = 0x000000B8,
            CKR_SIGNATURE_INVALID               = 0x000000C0,
            CKR_SIGNATURE_LEN_RANGE             = 0x000000C1,
            CKR_TEMPLATE_INCOMPLETE             = 0x000000D0,
            CKR_TEMPLATE_INCONSISTENT           = 0x000000D1,
            CKR_TOKEN_NOT_PRESENT               = 0x000000E0,
            CKR_TOKEN_NOT_RECOGNIZED            = 0x000000E1,
            CKR_TOKEN_WRITE_PROTECTED           = 0x000000E2,
            CKR_UNWRAPPING_KEY_HANDLE_INVALID   = 0x000000F0,
            CKR_UNWRAPPING_KEY_SIZE_RANGE       = 0x000000F1,
            CKR_UNWRAPPING_KEY_TYPE_INCONSISTENT= 0x000000F2,
            CKR_USER_ALREADY_LOGGED_IN          = 0x00000100,
            CKR_USER_NOT_LOGGED_IN              = 0x00000101,
            CKR_USER_PIN_NOT_INITIALIZED        = 0x00000102,
            CKR_USER_TYPE_INVALID               = 0x00000103,
            CKR_USER_ANOTHER_ALREADY_LOGGED_IN  = 0x00000104,
            CKR_USER_TOO_MANY_TYPES             = 0x00000105,
            CKR_WRAPPED_KEY_INVALID             = 0x00000110,
            CKR_WRAPPED_KEY_LEN_RANGE           = 0x00000112,
            CKR_WRAPPING_KEY_HANDLE_INVALID     = 0x00000113,
            CKR_WRAPPING_KEY_SIZE_RANGE         = 0x00000114,
            CKR_WRAPPING_KEY_TYPE_INCONSISTENT  = 0x00000115,
            CKR_RANDOM_SEED_NOT_SUPPORTED       = 0x00000120,
            CKR_RANDOM_NO_RNG                   = 0x00000121,
            CKR_DOMAIN_PARAMS_INVALID           = 0x00000130,
            CKR_BUFFER_TOO_SMALL                = 0x00000150,
            CKR_SAVED_STATE_INVALID             = 0x00000160,
            CKR_INFORMATION_SENSITIVE           = 0x00000170,
            CKR_STATE_UNSAVEABLE                = 0x00000180,
            CKR_CRYPTOKI_NOT_INITIALIZED        = 0x00000190,
            CKR_CRYPTOKI_ALREADY_INITIALIZED    = 0x00000191,
            CKR_MUTEX_BAD                       = 0x000001A0,
            CKR_MUTEX_NOT_LOCKED                = 0x000001A1,
            CKR_NEW_PIN_MODE                    = 0x000001B0,
            CKR_NEXT_OTP                        = 0x000001B1,
            CKR_FUNCTION_REJECTED               = 0x00000200,
            CKR_VENDOR_DEFINED                  = 0x80000000
        }

        // session flags
        public const uint CKF_RW_SESSION = 2;
        public const uint CKF_SERIAL_SESSION = 4;

        // user
        public const uint CKU_SO = 0;
        public const uint CKU_USER = 1;

        // attribute types
        public const uint CKA_CLASS = 0;
        public const uint CKA_TOKEN = 1;
        public const uint CKA_PRIVATE = 2;
        public const uint CKA_LABEL = 3;
        public const uint CKA_APPLICATION = 16;
        public const uint CKA_VALUE = 17;

        // object classes
        public const uint CKO_DATA = 0;
        public const uint CKO_CERTIFICATE = 1;
        public const uint CKO_PUBLIC_KEY = 2;
        public const uint CKO_PRIVATE_KEY = 3;
        public const uint CKO_SECRET_KEY = 4;
        public const uint CKO_HW_FEATURE = 5;
        public const uint CKO_DOMAIN_PARAMETERS = 6;
        public const uint CKO_VENDOR_DEFINED = 2147483648;

        //token_info flags
        public const uint CKF_RNG                       = 0x00000001;
        public const uint CKF_WRITE_PROTECTED           = 0x00000002;
        public const uint CKF_LOGIN_REQUIRED            = 0x00000004;
        public const uint CKF_USER_PIN_INITIALIZED      = 0x00000008;
        public const uint CKF_RESTORE_KEY_NOT_NEEDED    = 0x00000020;
        public const uint CKF_CLOCK_ON_TOKEN            = 0x00000040;
        public const uint CKF_PROTECTED_AUTHENTICATION_PATH = 0x00000100;
        public const uint CKF_DUAL_CRYPTO_OPERATIONS    = 0x00000200;
        public const uint CKF_TOKEN_INITIALIZED         = 0x00000400;
        public const uint CKF_SECONDARY_AUTHENTICATION  = 0x00000800;
        public const uint CKF_USER_PIN_COUNT_LOW        = 0x00010000;
        public const uint CKF_USER_PIN_FINAL_TRY        = 0x00020000;
        public const uint CKF_USER_PIN_LOCKED           = 0x00040000;
        public const uint CKF_USER_PIN_TO_BE_CHANGED    = 0x00080000;
        public const uint CKF_SO_PIN_COUNT_LOW          = 0x00100000;
        public const uint CKF_SO_PIN_FINAL_TRY          = 0x00200000;
        public const uint CKF_SO_PIN_LOCKED             = 0x00400000;
        public const uint CKF_SO_PIN_TO_BE_CHANGED      = 0x00800000;


        public const bool CK_TRUE = true;
        public const bool CK_FALSE = false;

        public const uint CKA_ID = 0x00000102;

        public const uint CKR_OK = 0x00000000;

        static uint rv = 0;

        private static bool is_initalized = false;
        private static CK_SESSION_HANDLE session;
        private static CK_SLOT_ID[] slotList;
        private static List<keyfile> keyfiles;

        [StructLayout(LayoutKind.Sequential)]
        public struct CK_VERSION
        {
            public byte major;
            public byte minor;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct CK_ATTRIBUTE
        {
            public CK_ATTRIBUTE_TYPE type;
            public IntPtr pValue;
            public CK_ULONG ulValueLen;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CK_SESSION_INFO
        {
            public CK_SLOT_ID slotID;
            public CK_ULONG state;
            public CK_FLAGS flags;
            public CK_ULONG ulDeviceError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CK_INFO
        {
            public CK_VERSION cryptokiVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string manufacturerID;
            public CK_ULONG flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string libraryDescription;
            public CK_VERSION libraryVersion;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CK_SLOT_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string slotDescription; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string manufacturerID;
            public CK_FLAGS flags;
            public CK_VERSION hardwareVersion;
            public CK_VERSION firmwareVersion;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CK_TOKEN_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string label;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string manufacturerID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string serialNumber;
            public CK_FLAGS flags;
            public CK_ULONG ulMaxSessionCount;
            public CK_ULONG ulSessionCount;
            public CK_ULONG ulMaxRwSessionCount;
            public CK_ULONG ulRwSessionCount;
            public CK_ULONG ulMaxPinLen;
            public CK_ULONG ulMinPinLen;
            public CK_ULONG ulTotalPublicMemory;
            public CK_ULONG ulFreePublicMemory;
            public CK_ULONG ulTotalPrivateMemory;
            public CK_ULONG ulFreePrivateMemory;
            public CK_VERSION hardwareVersion;
            public CK_VERSION firmwareVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string utcTime;
        }

        public struct keyfile
        {
            public CK_OBJECT_HANDLE handle;
            public CK_SLOT_ID slotid;
            public string label;
            public string token_name;
        }

        private delegate uint C_Initialize(CK_VOID_PTR param);
        private delegate uint C_Login(CK_SESSION_HANDLE hSession, CK_ULONG userType, string pPin, CK_ULONG ulPinLen);
        private delegate uint C_Logout(CK_SESSION_HANDLE hSession);
        private delegate uint C_CreateObject(CK_SESSION_HANDLE hSession, [MarshalAs(UnmanagedType.LPArray)] CK_ATTRIBUTE[] pTemplate, CK_ULONG ulCount, ref CK_ULONG phObject);
        private delegate uint C_DestroyObject(CK_SESSION_HANDLE hSession, CK_OBJECT_HANDLE hObject);
        private delegate uint C_Finalize(CK_VOID_PTR reserved);
        private delegate uint C_GetSlotList(CK_BBOOL tokenPresent, CK_SLOT_ID[] pSlotList, ref CK_ULONG pulCount);
        private delegate uint C_GetAttributeValue(CK_SESSION_HANDLE hSession, CK_OBJECT_HANDLE hObject, ref CK_ATTRIBUTE pTemplate, CK_ULONG ulCount);
        private delegate uint C_GetMechanismList(CK_SLOT_ID slotID, CK_VOID_PTR[] pMechanismList, ref CK_ULONG pulCount);
        private delegate uint C_GetSlotInfo(CK_SLOT_ID slotID, ref CK_SLOT_INFO pSlotList);
        private delegate uint C_GetTokenInfo(CK_SLOT_ID slotID, ref CK_TOKEN_INFO pInfo);
        private delegate uint C_GetSessionInfo(CK_SESSION_HANDLE hSession, ref CK_SESSION_INFO pInfo);
        private delegate uint C_GetInfo(ref CK_INFO pInfo);
        private delegate uint C_OpenSession(CK_SLOT_ID slotID, CK_FLAGS flags, CK_VOID_PTR pApplication, CK_NOTIFY Notify, out CK_SESSION_HANDLE phSession);
        private delegate uint C_CloseSession(CK_SESSION_HANDLE hSession);
        private delegate uint C_FindObjectsInit(CK_SESSION_HANDLE hSession, CK_ATTRIBUTE[] pTemplate, CK_ULONG ulCount);
        private delegate uint C_FindObjects(CK_SESSION_HANDLE hSession, ref CK_OBJECT_HANDLE phObject, CK_ULONG ulMaxObjectCount, ref CK_ULONG pulObjectCount);
        private delegate uint C_FindObjectsFinal(CK_SESSION_HANDLE hSession);
        private delegate uint C_Sign(CK_SESSION_HANDLE hSession, CK_BYTE[] pData, CK_ULONG ulDataLen, IntPtr pSignature, ref CK_ULONG pulSignatureLen);

        private static C_Initialize initialize;
        private static C_Login login;
        private static C_Logout logout;
        private static C_CreateObject create_object;
        private static C_DestroyObject destroy_object;
        private static C_Finalize finalize;
        private static C_GetSlotList get_slot_list;
        private static C_GetAttributeValue get_attribute_value;
        private static C_GetMechanismList get_mechanism_list;
        private static C_GetSlotInfo get_slot_info;
        private static C_GetTokenInfo get_token_info;
        private static C_GetSessionInfo get_session_info;
        private static C_GetInfo get_info;
        private static C_OpenSession open_session;
        private static C_CloseSession close_session;
        private static C_FindObjectsInit find_objects_init;
        private static C_FindObjects find_objects;
        private static C_FindObjectsFinal find_objects_final;
        private static C_Sign sign;


        static public T CreateDynamicDllInvoke<T>(string functionName, string library)
        {
            // create in-memory assembly, module and type
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("DynamicDllInvoke"),
                AssemblyBuilderAccess.Run);

            ModuleBuilder modBuilder = assemblyBuilder.DefineDynamicModule("DynamicDllModule");

            // note: without TypeBuilder, you can create global functions
            // on the module level, but you cannot create delegates to them
            TypeBuilder typeBuilder = modBuilder.DefineType(
                "DynamicDllInvokeType",
                TypeAttributes.Public | TypeAttributes.UnicodeClass);

            // get params from delegate dynamically (!), trick from Eric Lippert
            MethodInfo delegateMI = typeof(T).GetMethod("Invoke");
            Type[] delegateParams = (from param in delegateMI.GetParameters()
                                     select param.ParameterType).ToArray();

            // automatically create the correct signagure for PInvoke
            MethodBuilder methodBuilder = typeBuilder.DefinePInvokeMethod(
                functionName,
                library,
                MethodAttributes.Public |
                MethodAttributes.Static |
                MethodAttributes.PinvokeImpl,
                CallingConventions.Standard,
                delegateMI.ReturnType,        /* the return type */
                delegateParams,               /* array of parameters from delegate T */
                CallingConvention.Winapi,
                CharSet.Ansi);

            // needed according to MSDN
            methodBuilder.SetImplementationFlags(
                methodBuilder.GetMethodImplementationFlags() |
                MethodImplAttributes.PreserveSig);

            Type dynamicType = typeBuilder.CreateType();

            MethodInfo methodInfo = dynamicType.GetMethod(functionName);

            // create the delegate of type T, double casting is necessary
            return (T)(object)Delegate.CreateDelegate(
                typeof(T),
                methodInfo, true);
        }


        public static void PKCS11_init(string lib)
        {
            initialize = CreateDynamicDllInvoke<C_Initialize>("C_Initialize", lib);
            open_session = CreateDynamicDllInvoke<C_OpenSession>("C_OpenSession", lib);
            close_session = CreateDynamicDllInvoke<C_CloseSession>("C_CloseSession", lib);
            login = CreateDynamicDllInvoke<C_Login>("C_Login", lib);
            logout = CreateDynamicDllInvoke<C_Logout>("C_Logout", lib);
            create_object = CreateDynamicDllInvoke<C_CreateObject>("C_CreateObject", lib);
            get_slot_list = CreateDynamicDllInvoke<C_GetSlotList>("C_GetSlotList", lib);
            get_attribute_value = CreateDynamicDllInvoke<C_GetAttributeValue>("C_GetAttributeValue", lib);
            get_mechanism_list = CreateDynamicDllInvoke<C_GetMechanismList>("C_GetMechanismList", lib);
            get_slot_info = CreateDynamicDllInvoke<C_GetSlotInfo>("C_GetSlotInfo", lib);
            get_token_info = CreateDynamicDllInvoke<C_GetTokenInfo>("C_GetTokenInfo", lib);
            get_session_info = CreateDynamicDllInvoke<C_GetSessionInfo>("C_GetSessionInfo", lib);
            get_info = CreateDynamicDllInvoke<C_GetInfo>("C_GetInfo", lib);
            open_session = CreateDynamicDllInvoke<C_OpenSession>("C_OpenSession", lib);
            close_session = CreateDynamicDllInvoke<C_CloseSession>("C_CloseSession", lib);
            find_objects_init = CreateDynamicDllInvoke<C_FindObjectsInit>("C_FindObjectsInit", lib);
            find_objects = CreateDynamicDllInvoke<C_FindObjects>("C_FindObjects", lib);
            find_objects_final = CreateDynamicDllInvoke<C_FindObjectsFinal>("C_FindObjectsFinal", lib);
            sign = CreateDynamicDllInvoke<C_Sign>("C_Sign", lib);
            destroy_object = CreateDynamicDllInvoke<C_DestroyObject>("C_DestroyObject", lib);
            finalize = CreateDynamicDllInvoke<C_Finalize>("C_Finalize", lib);
        }

        public static void Initialize() {
            if (!is_initalized)
            {
                if ((rv = initialize(IntPtr.Zero)) != (int)ReturnValues.CKR_OK)
                    throw new Exception("Failed to initialize cryptoki library - Error: " + Enum.GetName(typeof(ReturnValues), rv));
                else
                    is_initalized = true;
            }
        }

        public static void OpenSession(CK_SLOT_ID slotID)
        {
            if ((rv = open_session(slotID, CKF_RW_SESSION | CKF_SERIAL_SESSION, IntPtr.Zero, IntPtr.Zero, out session)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to open session - Error: " + Enum.GetName(typeof(ReturnValues), rv));
        }

        public static void CloseSession()
        {
            close_session(session);
        }

        public static void Login(string pPin)
        {
            if ((rv = login(session, pkcs11.CKU_USER, pPin, (uint)pPin.Length)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to login - Error: " + Enum.GetName(typeof(ReturnValues), rv));
        }

        public static void Login()
        {
            if ((rv = login(session, pkcs11.CKU_USER, null, 0)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to login - Error: " + Enum.GetName(typeof(ReturnValues), rv));
        }

        public static void Logout()
        {
            logout(session);
                
        }

        public static void CreateObject(CK_ATTRIBUTE[] pTemplate, ref CK_ULONG phObject)
        {
            if ((rv = create_object(session, pTemplate, (uint)pTemplate.Length, ref phObject)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to create object - Error: " + Enum.GetName(typeof(ReturnValues), rv));
        }

        public static void createKeyfile(string label, byte[] data)
        {
            uint hObject = 0;

            pkcs11.CK_ATTRIBUTE[] template = new pkcs11.CK_ATTRIBUTE[] {
                    pkcs11.createAttribute(pkcs11.CKA_CLASS, BitConverter.GetBytes(0)),
                    pkcs11.createAttribute(pkcs11.CKA_TOKEN, new byte[] { 0x01 }),
                    pkcs11.createAttribute(pkcs11.CKA_PRIVATE, new byte[] { 0x01 }),
                    pkcs11.createAttribute(pkcs11.CKA_LABEL, Encoding.UTF8.GetBytes(label)),
                    pkcs11.createAttribute(pkcs11.CKA_VALUE, data)
            };
            try {
                CreateObject(template, ref hObject);
            } catch (Exception) {
                throw;
            }
        }

        public static CK_TOKEN_INFO GetTokenInfo(CK_SLOT_ID slotId)
        {
            CK_TOKEN_INFO info = new CK_TOKEN_INFO();

            if ((rv = get_token_info(slotId, ref info)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Can't get information of token - Error: " + Enum.GetName(typeof(ReturnValues), rv));

            return info;
        }

        public static CK_SESSION_INFO GetSessionInfo()
        {
            CK_SESSION_INFO info = new CK_SESSION_INFO();

            if ((rv = get_session_info(session, ref info)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Can't get information of session - Error: " + Enum.GetName(typeof(ReturnValues), rv));

            return info;
        }

        public static uint[] GetSlotList()
        {
            uint slotCount = 0;
       
            if ((rv = get_slot_list(true, null, ref slotCount)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to get number of slots - Error: " + Enum.GetName(typeof(ReturnValues), rv));

            uint[] slotList = new uint[slotCount];

            if ((rv = get_slot_list(true, slotList, ref slotCount)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to get a list of slots - Error: " + Enum.GetName(typeof(ReturnValues), rv));

            return slotList;
        }

        public static void FindObjectsInit()
        {
            CK_ATTRIBUTE[] template = new CK_ATTRIBUTE[1];
            template[0] = createAttribute(CKA_CLASS, BitConverter.GetBytes(CKO_DATA));

            if ((rv = find_objects_init(session, template, 1)) != (int)ReturnValues.CKR_OK)
                throw new Exception("Failed to init find objects - Error: " + Enum.GetName(typeof(ReturnValues), rv));       
        }

        public static List<CK_OBJECT_HANDLE> FindObjects()
        {
            List<CK_OBJECT_HANDLE> hObjectList = new List<CK_OBJECT_HANDLE>();
            CK_OBJECT_HANDLE hObject = 0;
            CK_ULONG ulObjectCount = 0;

            while (true)
            {
                rv = find_objects(session, ref hObject, 1, ref ulObjectCount);

                if (rv != CKR_OK)
                    throw new Exception("Failed to find objects - Error: " + Enum.GetName(typeof(ReturnValues), rv));

                if (ulObjectCount != 1)
                    break;

                hObjectList.Add(hObject);
            }

            return hObjectList;
        }

        public static byte[] GetAttributeValue(CK_OBJECT_HANDLE hObject, CK_ATTRIBUTE_TYPE type)
        {
            CK_ATTRIBUTE template = pkcs11.createAttribute(type, new byte[0]);
            get_attribute_value(session, hObject, ref template, 1);
            template.pValue = Marshal.AllocHGlobal((int)template.ulValueLen);
            get_attribute_value(session, hObject, ref template, 1);

            byte[] value_array = new byte[template.ulValueLen];
            Marshal.Copy(template.pValue, value_array, 0, (int)template.ulValueLen);

            return value_array;
        }


        public static List<keyfile> GetKeyfiles(List<CK_OBJECT_HANDLE> objectHandles, uint slotid, string token_name)
        {
            List<keyfile> list = new List<keyfile>();

            foreach (CK_OBJECT_HANDLE handle in objectHandles)
            {
                string label = Encoding.UTF8.GetString(GetAttributeValue(handle, CKA_LABEL));

                list.Add(new keyfile{handle = handle, label = label, slotid = slotid, token_name = token_name});
            }

            return list;
        }


        public static bool loginRequired(CK_SLOT_ID slotId)
        {
            try
            {
                CK_TOKEN_INFO token_info = GetTokenInfo(slotId);

                if ((token_info.flags & CKF_LOGIN_REQUIRED) == CKF_LOGIN_REQUIRED)
                    return true;
                else 
                    return false; 
            }
            catch (Exception)
            {
                throw;
            }   
        }


        public static void LoginIfRequired(CK_SLOT_ID slotId)
        {
            try
            {
                CK_SESSION_INFO session_info = GetSessionInfo();
                CK_TOKEN_INFO token_info = GetTokenInfo(slotId);

                if ( (token_info.flags & CKF_LOGIN_REQUIRED) == CKF_LOGIN_REQUIRED)
                {
                    if ((token_info.flags & CKF_PROTECTED_AUTHENTICATION_PATH) == CKF_PROTECTED_AUTHENTICATION_PATH)
                    {
                        Login();
                    }
                    else
                    {
                        CkpPromtForm dialog = new CkpPromtForm();
                        if (UIUtil.ShowDialogAndDestroy(dialog) != DialogResult.OK)
                            return;

                        Login(dialog.pin);

                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }        
        }

        public static CK_ATTRIBUTE createAttribute(uint type, byte[] val)
        {
            CK_ATTRIBUTE attr = new CK_ATTRIBUTE();
            attr.type = type;
            if (val != null && val.Length > 0)
            {
                attr.ulValueLen = (uint)val.Length;
                attr.pValue = Marshal.AllocHGlobal(val.Length);
                Marshal.Copy(val, 0, attr.pValue, val.Length);
            }
            else
            {
                attr.ulValueLen = (uint)val.Length;
                attr.pValue = IntPtr.Zero;
            }

            return attr;
        }

        public static List<keyfile> read_allkeyfiles(string path) {

            List<keyfile> _keyfiles = new List<keyfile>();

            try
            {
                
                PKCS11_init(path);
                Initialize();
                slotList = GetSlotList();

                if(slotList.Length == 0)
                    throw new Exception("No token available, please insert token.");

                for (uint i = 0; i < slotList.Length; i++)
                {
                    OpenSession(slotList[i]);

                    CK_TOKEN_INFO token_info = GetTokenInfo(slotList[i]);

                    if ((token_info.flags & CKF_LOGIN_REQUIRED) == CKF_LOGIN_REQUIRED)
                    {
                        if ((token_info.flags & CKF_PROTECTED_AUTHENTICATION_PATH) == CKF_PROTECTED_AUTHENTICATION_PATH)
                        {
                            Login();
                        }
                        else
                        {
                            CkpPromtForm dialog = new CkpPromtForm();
                            if (UIUtil.ShowDialogAndDestroy(dialog) != DialogResult.OK)
                                return null;

                            Login(dialog.pin);
                        }
                    }

                    CK_ATTRIBUTE findtemplate = createAttribute(pkcs11.CKA_CLASS, BitConverter.GetBytes(CKO_DATA));

                    FindObjectsInit();
                    List<uint> objects = FindObjects();
                    _keyfiles.AddRange(GetKeyfiles(objects, slotList[i], token_info.label) );
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _keyfiles;
        }

        public static byte[] pkcs11_read_key(string path, string label)
        {


            try
            {

                PKCS11_init(path);
                Initialize();
                slotList = GetSlotList();

                if(slotList.Length == 0)
                    throw new Exception("No token available, please insert token.");

                keyfiles = read_allkeyfiles(path);

                foreach (keyfile keyfile in keyfiles) {
                    if (keyfile.label == label)
                    {
                        byte[] key = GetAttributeValue(keyfile.handle, CKA_VALUE);
                        Logout();
                        CloseSession();
                        return key;
                    }
                }
            }
            catch (Exception)
            {
                Logout();
                CloseSession();
                throw;
            }

            Logout();
            CloseSession();
            return null;
        }
    }
}
