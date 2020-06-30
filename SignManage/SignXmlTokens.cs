﻿using System;
using System.Runtime.InteropServices;

namespace Camellia_Management_System.SignManage
{
    /// @author Yevgeniy Cherdantsev
    /// @date 18.02.2020 10:35:27
    /// <summary>
    /// Class that signs xml tokens
    /// </summary>
    public static class SignXmlTokens
    {
        /// @author Yevgeniy Cherdantsev
        /// @date 18.02.2020 10:35:42
        /// <summary>
        /// Signing of the xml token
        /// </summary>
        /// <param name="inData">XML text</param>
        /// <param name="sign">Sign</param>
        /// <returns>string - signed token</returns>
        public static string SignToken(string inData, Sign sign)
        {
            /*
             * Error status for electronic digital signature
             * 0 - ok
             * any numers greater then 0 is error
             */

            // Here calling COM and initialazing it
            var kalkanCom = new KalkanCryptCOMLib.KalkanCryptCOM();
            kalkanCom.Init();

            // Loading hey with handling exception
            kalkanCom.LoadKeyStore((int) KalkanCryptCOMLib.KALKANCRYPTCOM_STORETYPE.KCST_PKCS12, sign.password,
                sign.filePath,
                "");
            var edsError = kalkanCom.GetLastError();

            if (edsError > 0)
                throw new KalkanCryptException($"Some error occured while loading the key storage '{sign.filePath}'");

            var Alias = "";
            var SignNodeID = "";
            var ParentSignNode = "";
            var ParentNameSpace = "";

            // Signing XML
            kalkanCom.SignXML(Alias, 0, SignNodeID, ParentSignNode, ParentNameSpace,
                inData,
                out var outSign);
            kalkanCom.GetLastErrorString(out var error, out edsError);

            if (edsError > 0)
                throw new KalkanCryptException(
                    $"Some error occured while signing the token:${error}\n\nPossible reasons:\n1.Shortage of bin length (Should be 12);\n");

            var result = outSign.Replace("\n", "\r\n");
            return result;
        }

        /// <summary>
        /// Custom KalkanCryptCOMLib exception
        /// </summary>
        [Serializable]
        public class KalkanCryptException : Exception
        {
            /// <inheritdoc />
            public KalkanCryptException()
            {
            }

            /// <inheritdoc />
            public KalkanCryptException(string message)
                : base(message)
            {
            }

            /// <inheritdoc />
            public KalkanCryptException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }
    }
}