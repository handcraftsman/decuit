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

using FluentAssert;

using FluentWebUITesting.Extensions;

using WatiN.Core;

namespace gar3t.decuit
{
	public class SetExpression
	{
		private static readonly List<IInputSetter> _setters = new List<IInputSetter>
		{
			new TextBoxSetter(),
			new DropDownListSetter()
		};

		private readonly Browser _browser;

		public SetExpression(Browser browser, string labelText)
		{
			_browser = browser;
			LabelText = labelText;
		}

		public string LabelText { get; private set; }

		private string GetItsLinkedControlId()
		{
			var label = _browser.Label(Find.ByText(LabelText));
			label.Exists.ShouldBeTrue(String.Format("Could not find Label with text '{0}'", LabelText));

			string itsLinkedControlId = label.For;
			itsLinkedControlId.ShouldNotBeNullOrEmpty(String.Format("Label with text '{0}' does not have a For attribute", LabelText));
			return itsLinkedControlId;
		}

		public void To(string text)
		{
			string itsLinkedControlId = GetItsLinkedControlId();

			var control = _browser.Element(Find.ById(itsLinkedControlId));
			control.Exists.ShouldBeTrue(String.Format("Could not find a control with id '{0}' as referenced in For attribute of Label with text {1}", itsLinkedControlId, LabelText));
			control.Enabled.ShouldBeTrue(String.Format("Cannot set the value of control with id '{0}' because it is disabled.", itsLinkedControlId));

			var setter = _setters.FirstOrDefault(x => x.IsMatch(_browser, itsLinkedControlId));
			if (setter == null)
			{
				throw new ArgumentOutOfRangeException("text", String.Format("There is no configured InputSetter for control type with label '{0}'", LabelText));
			}
			setter.SetText(_browser, itsLinkedControlId, text);
		}

		public void To(CheckedState checkedState)
		{
			string itsLinkedControlId = GetItsLinkedControlId();
			var checkBox = _browser.CheckBoxWithId(itsLinkedControlId);
			checkBox.Exists().ShouldBeTrue(String.Format("Could not find a checkbox with id '{0}' as referenced in For attribute of Label with text {1}", itsLinkedControlId, LabelText));
			checkBox.Enabled().ShouldBeTrue(String.Format("Cannot set the value of checkbox with id '{0}' because it is disabled.", itsLinkedControlId));

			checkBox.CheckedState().SetValue(checkedState.Value);
		}
	}
}