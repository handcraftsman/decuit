//    Copyright 2010 Clinton Sheppard <sheppard@cs.unm.edu>
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using gar3t.Common;
using gar3t.Common.Extensions;
using gar3t.Common.Web;
using gar3t.Common.Web.Extensions;

namespace decuit.Tests.Integration
{
	public class SimpleWebServer
	{
		public static int Port = 8955;
		private int _stepIndex;
		private Tcp _tcp;

		public SimpleWebServer(IEnumerable<Action<HttpMessage, ISocketProxy>> serverResponses)
		{
			Steps = serverResponses.ToList();
		}

		public IList<Action<HttpMessage, ISocketProxy>> Steps { get; private set; }

		public static IEnumerable<Action<HttpMessage, ISocketProxy>> InitializeServerResponsesContainer(params Action<HttpMessage, ISocketProxy>[] serverResponses)
		{
			return serverResponses;
		}

		private void ProcessClientRequest(object state)
		{
			var client = (Socket)state;
			var message = new AsynchReceiver().Receive(client, false, "");
			Action<HttpMessage, ISocketProxy> action = ReturnNamedResourceAsWebPage;
			if (Steps.Count == 0 || Steps.Count < _stepIndex)
			{
				Console.WriteLine("no defined action for step " + (_stepIndex++) + " - trying ReturnNamedResourceAsWebPage");
			}
			else
			{
				action = Steps[_stepIndex++];
			}
			var proxy = new SocketProxy(client);
			action(message, proxy);
			proxy.Close();
		}

		public static void ReturnNamedResourceAsWebPage(HttpMessage httpMessage, ISocketProxy socketProxy)
		{
			string destinationUrl = httpMessage.Headers.DestinationUrl().Substring(1);
			var resourceStream = new ResourceReader().GetResource(destinationUrl);
			var bytes = resourceStream.ReadFully();
			socketProxy.SendWebPage(Encoding.ASCII.GetString(bytes));
		}

		public void Run()
		{
			_stepIndex = 0;
			_tcp = new Tcp();
			_tcp.Listen(Port, ProcessClientRequest);
		}

		public void Stop()
		{
			if (_tcp != null)
			{
				_tcp.Stop();
			}
		}
	}
}