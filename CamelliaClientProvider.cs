﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Camellia_Management_System.SignManage;

//TODO(REFACTOR)
namespace Camellia_Management_System
{
    /// @author Yevgeniy Cherdantsev
    /// <summary>
    /// Controller and provider of camellia clients
    /// </summary>
    public class CamelliaClientProvider
    {
        /// <summary>
        /// List of ready clients in the current cycle
        /// </summary>
        private List<CamelliaClient> _camelliaClients = new List<CamelliaClient>();

        /// <summary>
        /// List of used clients in the current cycle
        /// </summary>
        private readonly List<CamelliaClient> _usedClients = new List<CamelliaClient>();

        /// <summary>
        /// Sign provider
        /// </summary>
        private readonly SignProvider _signProvider;

        /// <summary>
        /// List of proxies
        /// </summary>
        private readonly IEnumerator<IWebProxy> _webProxies;

        /// <summary>
        /// Handler timeout of created clients
        /// </summary>
        private readonly int _handlerTimeout;

        /// <summary>
        /// Number of tries while creating clients
        /// </summary>
        private readonly int _numberOfTries;


        /// <summary>
        /// Return the number of left clients before the next shuffle
        /// </summary>
        public int clientsLeft => _camelliaClients.Count;

        /// @author Yevgeniy Cherdantsev
        /// @date 18.02.2020 10:31:53
        /// <summary>
        /// Creates clients from the given signs
        /// </summary>
        /// <param name="signProvider">Sign provider</param>
        /// <param name="webProxies">Proxy if need</param>
        /// <param name="handlerTimeout">Timeout</param>
        /// <param name="numberOfTries">Number Of Tries</param>
        /// <returns>List - Shuffled list</returns>
        public CamelliaClientProvider(SignProvider signProvider, List<IWebProxy> webProxies = null,
            int handlerTimeout = 20000, int numberOfTries = 5)
        {
            _signProvider = signProvider;
            _webProxies = webProxies.GetEnumerator();
            _handlerTimeout = handlerTimeout;
            _numberOfTries = numberOfTries;

            Task.Run(() =>
            {
                // if (_camelliaClients.Count == 0)
                // {
                // throw new InvalidDataException("No clients has been loaded");
                // Console.WriteLine("No clients has been loaded");
                // return;
                // }
            });
        }

        /// @author Yevgeniy Cherdantsev
        /// @date 30.06.2020 11:57:51
        /// <summary>
        /// Loads clients from the given signs
        /// </summary>
        /// <returns>List - Shuffled list</returns>
        public void LoadClients()
        {
            lock (_camelliaClients)
            {
                while (_signProvider.signsLeft > 0)
                {
                    var sign = _signProvider.GetNextSign();

                    for (var i = 0; i < _numberOfTries; i++)
                    {
                        try
                        {
                            if (!_webProxies.MoveNext())
                            {
                                _webProxies.Reset();
                                _webProxies.MoveNext();
                            }

                            Console.WriteLine($"Left to load {_signProvider.signsLeft + 1} clients");
                            var client = new CamelliaClient(sign, _webProxies.Current, _handlerTimeout);
                            client.Login().GetAwaiter().GetResult();
                            _camelliaClients.Add(client);
                            break;
                        }
                        catch (SignXmlTokens.KalkanCryptException)
                        {
                            break;
                        }
                        catch (Exception)
                        {
                            //ignore
                        }
                    }
                }

                _signProvider.LoadSigns();

                if (_camelliaClients.Count == 0)
                    throw new InvalidDataException("No clients has been loaded");
            }
        }

        /// @author Yevgeniy Cherdantsev
        /// <summary>
        /// Get next client from provider
        /// </summary>
        /// <returns>CamelliaClient - returns connected client</returns>
        public CamelliaClient GetNextClient()
        {
            lock (_camelliaClients)
            {
                var tries = _camelliaClients.Count + _usedClients.Count;
                for (var i = 0; i <= tries; i++)
                {
                    if (_camelliaClients.Count == 0)
                    {
                        if (_usedClients.Count == 0)
                            LoadClients();

                        _camelliaClients = _usedClients.OrderBy(x => new Random().NextDouble()).ToList();
                        _usedClients.Clear();
                    }

                    var client = _camelliaClients[0];

                    _usedClients.Add(client);
                    _camelliaClients.Remove(client);
                    if (!client.IsLogged().Result)
                    {
                        try
                        {
                            client.Login().GetAwaiter().GetResult();
                        }
                        catch (Exception)
                        {
                            _usedClients.Remove(client);
                            continue;
                        }
                    }

                    return client;
                }

                throw new Exception("Can't find working client");
            }
        }
    }
}