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
using FluentAssert.Exceptions;
using FluentWebUITesting;
using FluentWebUITesting.Extensions;

using gar3t.decuit;

using NUnit.Framework;

namespace decuit.Tests.Integration
{
	public partial class SetTests
	{
		[TestFixture]
		public class When_asked_to_Set_the_value_of_a_CheckBox
		{
			private const string PageName = "SetCheckBox.html";

			[Test]
			public void Should_fail_if_the_label_text_does_not_exist()
			{
				const string textOfBadLabel = "Chinese";
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set(textOfBadLabel).To(CheckedState.Checked)
					);

				var exception = Assert.Throws<ShouldBeTrueAssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	PageName));
				exception.Message.Contains(textOfBadLabel).ShouldBeTrue();
			}

			[Test]
			public void Should_fail_if_the_matching_label_does_not_have_a_For_attribute()
			{
				const string textOfBadLabel = "Label without for";
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set(textOfBadLabel).To(CheckedState.Checked)
					);

				var exception = Assert.Throws<ShouldBeTrueAssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	PageName));
				exception.Message.Contains(textOfBadLabel).ShouldBeTrue();
			}

			[Test]
			public void Should_fail_if_the_matching_labels_For_attribute_refers_to_a_missing_control()
			{
				const string textOfBadLabel = "Label with incorrect for";
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set(textOfBadLabel).To(CheckedState.Checked)
					);

				var exception = Assert.Throws<ShouldBeTrueAssertionException>(() => new IntegrationTestRunner().Run(
				                                                        	browserActions,
				                                                        	SimpleWebServer.InitializeServerResponsesContainer(),
				                                                        	PageName));
				exception.Message.Contains(textOfBadLabel).ShouldBeTrue();
			}

			[Test]
			public void Should_succeed_if_the_label_text_and_for_attribute_match_and_want_it_checked()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("English").To(CheckedState.Checked),
					b => b.CheckBoxWithId("cbEnglish").CheckedState().ShouldBeTrue()
					);

				new IntegrationTestRunner().Run(
					browserActions,
					SimpleWebServer.InitializeServerResponsesContainer(),
					PageName);
			}

			[Test]
			public void Should_succeed_if_the_label_text_and_for_attribute_match_and_want_it_unchecked()
			{
				var browserActions = UITestRunner.InitializeWorkFlowContainer(
					b => b.Set("Spanish").To(CheckedState.Unchecked),
					b => b.CheckBoxWithId("cbSpanish").CheckedState().ShouldBeFalse()
					);

				new IntegrationTestRunner().Run(
					browserActions,
					SimpleWebServer.InitializeServerResponsesContainer(),
					PageName);
			}
		}
	}
}