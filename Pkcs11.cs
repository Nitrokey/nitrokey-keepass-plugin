using System;
using System.Collections.Generic;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.Common;
using CryptokiKeyProvider.Forms;
using System.Windows.Forms;
using KeePass.UI;
namespace CryptokiKeyProvider
{
	public class Pkcs11
	{
		private static Net.Pkcs11Interop.HighLevelAPI.Pkcs11 pk;
		public struct keyfile
        {
            public ObjectHandle handle;
            public System.UInt32 slotid;
            public string label;
            public string token_name;
        }
		private static List<keyfile> keyfiles;
		public static Session session;
		public static void init (String lib)
		{
			pk = new Net.Pkcs11Interop.HighLevelAPI.Pkcs11(lib,false);
			//Console.WriteLine("OK");
		}

		public static Slot slots(){
			if (pk.GetSlotList(true).Count == 1)
				return pk.GetSlotList(true)[0];
			else return pk.GetSlotList(true)[1];
		}
		public static void Login(String password){
			Slot slot  = slots();
			try{
				//try logout any session open
				Logout();}catch(Exception ex){

			}
			session = slot.OpenSession(false);
			//Login as normal user
			session.Login(Net.Pkcs11Interop.Common.CKU.CKU_USER, password);
		}
		public static bool loginRequired(System.UInt32 slotId)
        {
            try
            {
				TokenInfo token_info = slots().GetTokenInfo();
                if ((token_info.TokenFlags.Flags & CKF.CKF_LOGIN_REQUIRED) == CKF.CKF_LOGIN_REQUIRED)
                    return true;
                else 
                    return false; 
            }
            catch (Exception)
            {
                throw;
            }   
        }


        public static void LoginIfRequired(UInt32 slotId)
        {
            try
            {
                //SessionInfo session_info = session.GetSessionInfo();
				TokenInfo token_info = slots().GetTokenInfo();

                if ( (token_info.TokenFlags.Flags & CKF.CKF_LOGIN_REQUIRED) == CKF.CKF_LOGIN_REQUIRED)
                {
                    if ((token_info.TokenFlags.Flags & CKF.CKF_PROTECTED_AUTHENTICATION_PATH) == CKF.CKF_PROTECTED_AUTHENTICATION_PATH)
                    {
                        Login("");
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
		public static void createKeyfile(string label, byte[] data)
        {                
                //Slot slot = slots();		
	            TokenInfo token_info = slots().GetTokenInfo();	
				if ( (token_info.TokenFlags.Flags & CKF.CKF_LOGIN_REQUIRED) == CKF.CKF_LOGIN_REQUIRED)
                {
                    if ((token_info.TokenFlags.Flags & CKF.CKF_PROTECTED_AUTHENTICATION_PATH) == CKF.CKF_PROTECTED_AUTHENTICATION_PATH)
                    {
                        Login("");
                    }
                    else
                    {
                        CkpPromtForm dialog = new CkpPromtForm();
                        if (UIUtil.ShowDialogAndDestroy(dialog) != DialogResult.OK)
						return;
                        Login(dialog.pin);

                    }
                }
                    List<ObjectAttribute> objectAttributes = new List<ObjectAttribute>();
                    objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_CLASS, (uint)Net.Pkcs11Interop.Common.CKO.CKO_DATA));
                    objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_TOKEN, new byte[] { 0x01 }));
                    objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_APPLICATION, "CryptokiKeyProvider"));
                    objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_LABEL, "CryptokiKeyProvider"));
                    objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_VALUE, data));
                    // Create object
                    session.CreateObject(objectAttributes);
                    //session.Logout();                
        }
		public static List<keyfile> read_allkeyfiles(String path) {
			init(path);
            List<keyfile> _keyfiles = new List<keyfile>();
			Slot slot = slots();
			//Console.WriteLine("Get Slots");
			//SessionInfo session_info = session.GetSessionInfo();
            TokenInfo token_info = slots().GetTokenInfo();

                if ( (token_info.TokenFlags.Flags & CKF.CKF_LOGIN_REQUIRED) == CKF.CKF_LOGIN_REQUIRED)
                {
                    if ((token_info.TokenFlags.Flags & CKF.CKF_PROTECTED_AUTHENTICATION_PATH) == CKF.CKF_PROTECTED_AUTHENTICATION_PATH)
                    {
                        Login("");
                    }
                    else
                    {
                        CkpPromtForm dialog = new CkpPromtForm();
                        if (UIUtil.ShowDialogAndDestroy(dialog) != DialogResult.OK)
                            return null;
                        Login(dialog.pin);

                    }
                }
			//Console.WriteLine("Login");
			//using (Session session = slot.OpenSession(false)){
			//	session.Login(Net.Pkcs11Interop.Common.CKU.CKU_USER,password);
				List<ObjectAttribute> objectAttributes = new List<ObjectAttribute>();
				objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_CLASS, (uint)Net.Pkcs11Interop.Common.CKO.CKO_DATA));
				objectAttributes.Add(new ObjectAttribute(Net.Pkcs11Interop.Common.CKA.CKA_TOKEN, true));
				List<ObjectHandle> found = session.FindAllObjects(objectAttributes);
				foreach(ObjectHandle hanlder in found){
					List<Net.Pkcs11Interop.Common.CKA> attr = new List<Net.Pkcs11Interop.Common.CKA>();
					attr.Add(Net.Pkcs11Interop.Common.CKA.CKA_LABEL);
					string label = session.GetAttributeValue(hanlder, attr)[0].GetValueAsString();
					_keyfiles.Add(new keyfile{
						handle = hanlder,
						label = label,
						slotid = slot.GetTokenInfo().SlotId,
						token_name = slot.GetTokenInfo().Label,
					});
				}
			//}
			return _keyfiles;            
        }
		public static byte[] GetAttributeValue(ObjectHandle hObject, Net.Pkcs11Interop.Common.CKA type)
        {            
            //byte[] value_array = new byte[template.ulValueLen]; 
			List<Net.Pkcs11Interop.Common.CKA> attr = new List<Net.Pkcs11Interop.Common.CKA>();
			attr.Add(type);
			return session.GetAttributeValue(hObject,attr)[0].GetValueAsByteArray();
            //return null;
        }
		public static void Logout(){
			session.Logout();
			session.CloseSession();
		}
		public static void deleteDataObject(keyfile key){
			try{
				//LoginIfRequired(key.slotid);
				session.DestroyObject(key.handle);
				Logout();
			}catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}
		public static byte[] pkcs11_read_key(string path, string label)
        {
            try
            {

                keyfiles = read_allkeyfiles(path);

                foreach (keyfile keyfile in keyfiles) {
                    if (keyfile.label == label)
                    {
                        byte[] key = GetAttributeValue(keyfile.handle, CKA.CKA_VALUE);
                        Logout();
                        //CloseSession();
                        return key;
                    }
                }
            }
            catch (Exception)
            {
                Logout();
                //CloseSession();
                throw;
            }

            Logout();
            //CloseSession();
            return null;
        }    
		/*public static byte[] pkcs11_read_key(string password)
        {
            try
            {   
                keyfiles keyfile = read_allkeyfiles(password);
				Slot slot = slots();                
				using(Session session = slot.OpenSession(false)){
                	foreach (keyfiles keyfile in keyfiles) {                    	                    
                        	byte[] key = GetAttributeValue(keyfile.handle, CKA_VALUE);
                        	return key;                    	
                	}
				}
            }
            catch (Exception)
            {                
            }            
            return null;
        }*/
        
	}
}

