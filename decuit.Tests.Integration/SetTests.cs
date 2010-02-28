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

using FluentWebUITesting;

using gar3t.decuit;

using NUnit.Framework;

namespace decuit.Tests.Integration
{
	public class SetTests
	{
		[TestFixture]
		public class When_asked_to_Set_the_value_of_a_TextBox
		{
			[Test]
			public void Should_fail_if_the_label_text_does_not_exist()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("Last Name").To("James")
					);

				var exception = Assert.Throws<AssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	"SetTextBox.html"));
				exception.Message.Contains("Last Name").ShouldBeTrue();
			}

			[Test]
			public void Should_fail_if_the_matching_label_does_not_have_a_For_attribute()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("Last Name").To("James")
					);

				var exception = Assert.Throws<AssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	"SetTextbox_LabelWithoutFor.html"));
				exception.Message.Contains("Last Name").ShouldBeTrue();
			}

			[Test]
			public void Should_fail_if_the_matching_labels_For_attribute_refers_to_a_missing_control()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("Last Name").To("James")
					);

				var exception = Assert.Throws<AssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	"SetTextbox_LabelWithForPointedToMissingControl.html"));
				exception.Message.Contains("Last Name").ShouldBeTrue();
			}

			[Test]
			public void Should_succeed_if_the_label_text_and_for_attribute_match()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("First Name").To("James")
					);

				new IntegrationTestRunner().Run(
					browserActions,
					SimpleWebServer.InitializeServerResponsesContainer(),
					"SetTextBox.html");
			}
		}
	}
}