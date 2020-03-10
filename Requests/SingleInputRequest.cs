﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Camellia_Management_System.JsonObjects;
using Camellia_Management_System.JsonObjects.RequestObjects;
using Camellia_Management_System.SignManage;

namespace Camellia_Management_System.Requests
{
    /// @author Yevgeniy Cherdantsev
    /// @date 07.03.2020 16:35:25
    /// @version 1.0
    /// <summary>
    /// INPUT
    /// </summary>
    /// <code>
    /// 
    /// </code>
    public abstract class SingleInputRequest : CamelliaRequest
    {
        protected SingleInputRequest(CamelliaClient camelliaClient) : base(camelliaClient)
        {
        }


        public IEnumerable<ResultForDownload> GetReference(string input, int delay = 1000)
        {
            var token = GetToken(input);
            token = JsonSerializer.Deserialize<TokenResponse>(token).xml;
            var signedToken = SignXmlTokens.SignToken(token, CamelliaClient.FullSign.RsaSign);
            var requestNumber = SendPdfRequest(signedToken);
            var readinessStatus = WaitResult(requestNumber, delay);
            if (readinessStatus.status.Equals("APPROVED"))
                return readinessStatus.resultsForDownload;

            throw new Exception($"Readiness status equals {readinessStatus.status}");
        }
    }
}