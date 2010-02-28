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

using FluentAssert;

using gar3t.decuit;

using NUnit.Framework;

namespace decuit.Tests
{
	public class BrowserExtensionsTests
	{
		[TestFixture]
		public class When_asked_to_Set
		{
			[Test]
			public void Should_reference_the_given_label_text()
			{
				const string labelText = "First Name";
				var result = BrowserExtensions.Set(null, labelText);
				result.LabelText.ShouldBeEqualTo(labelText);
			}
		}
	}
}