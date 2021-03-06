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
using System.Threading;

using FluentWebUITesting;

using gar3t.Common.Web;

using WatiN.Core;

namespace decuit.Tests.Integration
{
	public class IntegrationTestRunner
	{
		public void Run(IEnumerable<Action<Browser>> browserActions, IEnumerable<Action<HttpMessage, ISocketProxy>> webServerResponses, string initialPage)
		{
			UITestRunner.InitializeBrowsers(x =>
				{
					x.CloseBrowserAfterEachTest = false;
					x.UseInternetExplorer = false;
					x.UseFireFox = true;
				});

			var webServer = new SimpleWebServer(webServerResponses);
			try
			{
				foreach (var step in webServerResponses)
				{
					webServer.Steps.Add(step);
				}
				var thread = new Thread(webServer.Run);
				thread.Start();

				UITestRunner.RunTest("http://localhost:" + SimpleWebServer.Port + "/" + initialPage,
				                     "",
				                     browserActions);
			}
			finally
			{
				webServer.Stop();
				try
				{
					UITestRunner.CloseBrowsers();
				}
				catch
				{
				}
			}
		}
	}
}