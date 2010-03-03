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

using FluentWebUITesting.Extensions;

using NUnit.Framework;

using WatiN.Core;

namespace gar3t.decuit
{
	public class TextBoxSetter : IInputSetter
	{
		public bool IsMatch(Browser browser, string id)
		{
			return browser.TextBoxWithId(id).Exists().IsTrue;
		}

		public void SetText(Browser browser, string id, string textToSet)
		{
			browser.TextBoxWithId(id).Text().SetValueTo(textToSet);
		}
	}

	public interface IInputSetter
	{
		bool IsMatch(Browser browser, string id);
		void SetText(Browser browser, string id, string textToSet);
	}

	public class DropDownListSetter : IInputSetter
	{
		public bool IsMatch(Browser browser, string id)
		{
			return browser.DropDownListWithId(id).Exists().IsTrue;
		}

		public void SetText(Browser browser, string id, string textToSet)
		{
			var dropDown = browser.DropDownListWithId(id);
			var option = dropDown.OptionWithText(textToSet);
			if (option.Exists().IsTrue)
			{
				option.Select();
				return;
			}
			option = dropDown.OptionWithValue(textToSet);
			if (option.Exists().IsTrue)
			{
				option.Select();
				return;
			}
			Assert.Fail(String.Format("The drop down with id '{0}' does not have option '{1}'", id, textToSet));
		}
	}
}