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

using FluentWebUITesting.Extensions;

using JetBrains.Annotations;

using NUnit.Framework;

using WatiN.Core;

namespace gar3t.decuit
{
	public class InputSetter
	{
		private static readonly List<InputSetter> _setters = new List<InputSetter>();

		static InputSetter()
		{
			new InputSetter(
				(b, id) => b.TextBoxWithId(id).Exists().Passed,
				(b, id, textToSet) => b.TextBoxWithId(id).Text().SetValueTo(textToSet));
			new InputSetter(
				(b, id) => b.DropDownListWithId(id).Exists().Passed,
				(b, id, textToSet) =>
					{
						var dropDown = b.DropDownListWithId(id);
						var option = dropDown.OptionWithText(textToSet);
						if (option.Exists().Passed)
						{
							option.Select();
							return;
						}
						option = dropDown.OptionWithValue(textToSet);
						if (option.Exists().Passed)
						{
							option.Select();
							return;
						}
						Assert.Fail(String.Format("The drop down with id '{0}' does not have option '{1}'", id, textToSet));
					});
		}

		private InputSetter(Func<Browser, string, bool> isMatch, Action<Browser, string, string> setText)
		{
			IsMatch = isMatch;
			SetText = setText;
			_setters.Add(this);
		}

		private Func<Browser, string, bool> IsMatch { get; set; }
		public Action<Browser, string, string> SetText { get; private set; }

		[NotNull]
		public static InputSetter GetFor([NotNull] Browser browser, [NotNull] string id)
		{
			var setter = _setters.FirstOrDefault(x => x.IsMatch(browser, id));
			if (setter == null)
			{
				throw new ArgumentOutOfRangeException("id",
				                                      String.Format("There is no configured InputSetter for type of control with id '{0}'", id));
			}
			return setter;
		}
	}
}