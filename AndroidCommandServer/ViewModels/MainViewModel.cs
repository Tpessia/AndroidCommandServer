using AndroidCommandServer.Common;
using AndroidCommandServer.Services;
using System;
using System.Net;

namespace AndroidCommandServer.ViewModels
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        // Services

        private readonly IHttpServer _httpServer;
        private readonly ICommandHandler _commandHandler;

        // Observables

        private string _endpoint;
        public string Endpoint
        {
            get => _endpoint;
            set
            {
                if (_endpoint != value)
                {
                    _endpoint = value;
                    OnPropertyChanged();
                }
            }
        }

        // Constructors

        public MainViewModel(IHttpServer httpServer, ICommandHandler commandHandler)
        {
            _httpServer = httpServer;
            _commandHandler = commandHandler;

            _endpoint = new IPEndPoint(IPAddress.Any, 8000).ToString();
        }

        // Actions

        public IPEndPoint GetEndpoint()
        {
            IPEndPoint endpoint;

            try
            {
                var (address, port, _) = _endpoint.Split(":");
                endpoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid endpoint: {_endpoint}", ex);
            }

            return endpoint;
        }

        public void Connect(IPEndPoint endpoint)
        {
            if (_httpServer.IsRunning)
            {
                ShowMessage("Server is already running");
                return;
            }

            _httpServer.Error += (sender, e) => ShowError(e.Exception);

            _httpServer.Start(endpoint, async req =>
            {
                try
                {
                    var result = await _commandHandler.HandleCommand(req.Body);
                    return new HttpResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return new HttpResponse(HttpStatusCode.InternalServerError, ex.ToErrorString());
                }
            });

            ShowMessage($"Starting: {endpoint}");
        }

        public void Disconnect()
        {
            if (!_httpServer.IsRunning)
            {
                ShowMessage("Server is already stopped");
                return;
            }

            _httpServer.Stop();

            ShowMessage("Stoping");
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
