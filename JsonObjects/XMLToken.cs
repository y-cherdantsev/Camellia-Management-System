﻿using System;

namespace Camellia_Management_System.JsonObjects
{
    /// @author Yevgeniy Cherdantsev
    /// @date 18.02.2020 12:19:42
    /// <summary>
    /// XMLToken json object
    /// </summary>
    public class XMLToken : IDisposable
    {
        public string xml { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xml">XML returned by camellia system for authorization</param>
        public XMLToken(string xml)
        {
            this.xml = xml;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            xml = null;
        }
    }
}