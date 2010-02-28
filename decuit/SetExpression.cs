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

using FluentAssert;

using FluentWebUITesting.Extensions;

using NUnit.Framework;

using WatiN.Core;

namespace gar3t.decuit
{
	public class SetExpression
	{
		private readonly Browser _browser;

		public SetExpression(Browser browser, string labelText)
		{
			_browser = browser;
			LabelText = labelText;
		}

		public string LabelText { get; private set; }

		public void To(string text)
		{
			var label = _browser.Label(Find.ByText(LabelText));
			label.Exists.ShouldBeTrue(String.Format("Could not find Label with text '{0}'", LabelText));

			string itsLinkedControlId = label.For;
			itsLinkedControlId.ShouldNotBeNullOrEmpty(String.Format("Label with text '{0}' does not have a For attribute", LabelText));

			var control = _browser.Element(Find.ById(itsLinkedControlId));
			control.Exists.ShouldBeTrue(String.Format("Could not find a control with id '{0}' as referenced in For atribute of Label with text {1}", itsLinkedControlId, LabelText));
			control.Enabled.ShouldBeTrue(String.Format("Cannot set the value of control with id '{0}' because it is disabled.", itsLinkedControlId, LabelText));

			var txtBox = _browser.TextBoxWithId(itsLinkedControlId);
			if (txtBox.Exists().Passed)
			{
				txtBox.Text().SetValueTo(text);
				return;
			}
			Assert.Fail(String.Format("Don't know how to set the value of control with id '{0}'", itsLinkedControlId));
//			var dropDown = _browser.DropDownListWithId(itsLinkedControlId);
//			if (dropDown.Exists().Passed)
//			{
//				var option = dropDown.OptionWithText(text);
//				if (option.Exists().Passed)
//				{
//					option.Select();
//					return;
//				}
//				option = dropDown.OptionWithValue(text);
//				if (option.Exists().Passed)
//				{
//					option.Select();
//					return;
//				}
//				Assert.Fail(String.Format("The drop down with label '{0}' does not have option '{1}'", LabelText, text));
//			}
		}
	}
}