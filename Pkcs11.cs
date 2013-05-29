using System;
using Net.Pkcs11Interop.HighLevelAPI;

namespace CryptokiKeyProvider
{
	public class pkcs11
	{
		public struct keyfile
        {
            public CK_OBJECT_HANDLE handle;
            public CK_SLOT_ID slotid;
            public string label;
            public string token_name;
        }

		public pkcs11 ()
		{
		}
	}
}

