﻿using System;

namespace Camellia_Management_System.JsonObjects.RequestObjects
{
    /// <inheritdoc />
    public class IinDeclarant : Declarant, IDisposable
    {
        public string iin { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iin">IIN of the request target</param>
        /// <param name="declarantUin">BIIN of the sender</param>
        public IinDeclarant(string iin, string declarantUin) : base(declarantUin)
        {
            this.iin = iin;
        }

        /// <inheritdoc />
        public new void Dispose()
        {
            iin = null;
            base.Dispose();
        }
    }
}