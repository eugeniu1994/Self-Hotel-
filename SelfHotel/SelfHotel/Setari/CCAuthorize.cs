using SelfHotel.Nomenclatoare;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SelfHotel.Setari
{
    public class CCAuthorize
    {
        #region Local variables
        private static string TargetURL = "https://secure.euplatesc.ro/tdsprocess/tranzactd.php";
        //private byte[] TestKey = new byte[] { 0x16, 0xB9, 0x16, 0x37, 0xE2, 0x29, 0x57, 0x3F, 0xAE, 0xF2, 0x2F, 0x32, 0x04, 0x83, 0x28, 0x3F, 0x68, 0x95, 0x4B, 0x83 };
        //private static byte[] ProductionKey = new byte[] { 0x16, 0xB9, 0x16, 0x37, 0xE2, 0x29, 0x57, 0x3F, 0xAE, 0xF2, 0x2F, 0x32, 0x04, 0x83, 0x28, 0x3F, 0x68, 0x95, 0x4B, 0x83 };
        //private string TestMerchantID = "44840981086";
        //private static string ProductionMerchantID = "44840981086";
        #endregion

        private static byte[] GetCheie(string cheie)
        {
            byte[] rv = null;

            try
            {
                if (cheie.Length == 0 || cheie.Length % 2 != 0)
                {
                    rv = null;
                }
                else
                {
                    rv = new byte[cheie.Length / 2];
                    for (int i = 0; i < cheie.Length; i += 2)
                    {
                        byte x = 0;
                        if (Byte.TryParse(cheie.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out x))
                        {
                            rv[i / 2] = x;
                        }
                        else
                        {
                            rv[i / 2] = 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                rv = null;
            }

            return rv;
        }

        // StS @ 11.11.2014 - add optional ExtraData string parameter to the authorize form.
        public string StartTransaction(string MerchantID, byte[] Key, double Amount, string Currency, long InvoiceID, string InvoiceDescription, CCPerson PayingPerson, CCPerson TargetPerson, string ExtraData)
        {
            DateTime TimeStamp = DateTime.UtcNow;
            string Nonce = GetRandomHexNumber(32);

            string MerchantMAC = GenerateMerchantMAC(MerchantID, Amount, Currency, InvoiceID, InvoiceDescription, TimeStamp, Nonce);
            string FP_Hash = GenerateHMACMD5(MerchantMAC, Key);

            var TransactionParameters = BuildTransactionParameters(MerchantID, Amount, Currency, InvoiceID, InvoiceDescription, TimeStamp, Nonce, FP_Hash, PayingPerson, TargetPerson);

            // StS @ 11.11.2014 - If ExtraData has some data then add ExtraData to the form 
            if (ExtraData.Trim().Length > 0)
                TransactionParameters.Add("ExtraData", ExtraData.Trim());

            return PreparePOSTForm(TargetURL, TransactionParameters);
        }
        /// <summary>
        /// This method prepares an Html form which holds all data in hidden field in the addition to form submitting script.
        /// </summary>
        /// <param name="url">The destination Url to which the post and redirection will occur, the Url can be in the same App or ouside the App.</param>
        /// <param name="data">A collection of data that will be posted to the destination Url.</param>
        /// <returns>Returns a string representation of the Posting form.</returns>
        /// <Author>Samer Abu Rabie</Author>
        private static string PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";

            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<html><body><form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }
            strForm.Append("</form>");

            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script></body></html>");

            //Return the form and the script concatenated. (The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        #region URL Builder
        private static NameValueCollection BuildTransactionParameters(string MerchantID, double Amount, string Currency, long InvoiceID, string InvoiceDescription, DateTime TimeStamp, string Nonce, string HMACMD5Hash, CCPerson PayingPerson, CCPerson TargetPerson)
        {
            NameValueCollection data = new NameValueCollection();

            data.Add("amount", Amount.ToString(CultureInfo.InvariantCulture));
            data.Add("curr", Currency);
            data.Add("invoice_id", InvoiceID.ToString());
            data.Add("order_desc", InvoiceDescription);
            data.Add("merch_id", MerchantID);
            data.Add("timestamp", GetTimeAsString(TimeStamp));
            data.Add("nonce", Nonce);
            data.Add("fp_hash", HMACMD5Hash);

            data.Add("fname", PayingPerson.FirstName);
            data.Add("lname", PayingPerson.LastName);
            data.Add("country", PayingPerson.Country);
            data.Add("company", PayingPerson.Company);
            data.Add("city", PayingPerson.City);
            data.Add("add", PayingPerson.Address);
            data.Add("email", PayingPerson.Email);
            data.Add("phone", PayingPerson.Phone);
            data.Add("fax", PayingPerson.Fax);

            data.Add("sfname", TargetPerson.FirstName);
            data.Add("slname", TargetPerson.LastName);
            data.Add("scountry", TargetPerson.Country);
            data.Add("scompany", TargetPerson.Company);
            data.Add("scity", TargetPerson.City);
            data.Add("sadd", TargetPerson.Address);
            data.Add("semail", TargetPerson.Email);
            data.Add("sphone", TargetPerson.Phone);
            data.Add("sfax", TargetPerson.Fax);

            return data;
        }
        #endregion

        #region HMAC MD5
        private static string GenerateHMACMD5(string MerchantMAC, byte[] Key)
        {
            HMACMD5 myhmacMD5 = new HMACMD5(Key);
            byte[] hash = myhmacMD5.ComputeHash(Encoding.ASCII.GetBytes(MerchantMAC));
            string result = String.Concat(hash.Select(x => x.ToString("X2")).ToArray());
            return result;
        }
        #endregion

        #region Merchant MAC

        private static string GeneratePaymentGatewayMAC(double Amount, string Currency, long InvoiceID, string GatewayUniqueID,
            string MerchantID, int Action, string Message, string Approval, DateTime TimeStamp, string Nonce)
        {
            // Compute hash
            StringBuilder sb = new StringBuilder();
            sb.Append(AddLengthPrefixTo(Amount.ToString(CultureInfo.InvariantCulture)));   // Append amount
            sb.Append(AddLengthPrefixTo(Currency));                                        // Append currency
            sb.Append(AddLengthPrefixTo(InvoiceID.ToString()));
            sb.Append(AddLengthPrefixTo(GatewayUniqueID));
            sb.Append(AddLengthPrefixTo(MerchantID));
            sb.Append(AddLengthPrefixTo(Action.ToString()));
            sb.Append(AddLengthPrefixTo(Message));
            sb.Append(AddLengthPrefixTo(Approval));
            sb.Append(AddLengthPrefixTo(GetTimeAsString(TimeStamp)));
            sb.Append(AddLengthPrefixTo(Nonce));

            return sb.ToString();
        }

        private static string GenerateMerchantMAC(string MerchantID, double Amount, string Currency, long InvoiceID, string InvoiceDescription, DateTime TimeStamp, string Nonce)
        {
            // Compute hash
            StringBuilder sb = new StringBuilder();
            sb.Append(AddLengthPrefixTo(Amount.ToString(CultureInfo.InvariantCulture)));   // Append amount
            sb.Append(AddLengthPrefixTo(Currency));                                        // Append currency
            sb.Append(AddLengthPrefixTo(InvoiceID.ToString()));
            sb.Append(AddLengthPrefixTo(InvoiceDescription));
            sb.Append(AddLengthPrefixTo(MerchantID));
            sb.Append(AddLengthPrefixTo(GetTimeAsString(TimeStamp)));
            sb.Append(AddLengthPrefixTo(Nonce));

            return sb.ToString();
        }

        private static string GetTimeAsString(DateTime TimeStamp)
        {
            return TimeStamp.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }

        private static string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        private static string AddLengthPrefixTo(string obj)
        {
            if (obj.Length > 0)
            {
                return obj.Length.ToString() + obj;
            }
            else
            {
                return "-";
            }
        }
        #endregion
    }    
}