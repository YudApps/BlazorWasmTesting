using BlazorWasmTesting.Client.Pages;
using Bunit;
using FluentAssertions;
using Xunit;

namespace BlazorWasmTesting.Test.Client
{
    public class ClientSideOnly_CounterTest
    {
        [Fact]
        public void UiOnly_CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Counter>();

            // Act: find and click the <button> element to increment
            // the counter in the <p> element
            cut.Find("button").Click();
            cut.Find("button").Click();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").MarkupMatches("<p>Current count: 2</p>");
        }

        [Fact]
        public void UiToModel_CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Counter>();

            // Act: find and click the <button> element to increment
            // the counter in the <p> element
            cut.Find("button").Click();
            cut.Find("button").Click();
            
            // Assert
            cut.Instance.CurrentCount.Should().Be(2);
        }

        [Fact]
        public void ModelToUi_CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Counter>();

            // Act: use the model's IncrementCount
            cut.Instance.IncrementCount();
            cut.Instance.IncrementCount();
            cut.Render();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").MarkupMatches("<p>Current count: 2</p>");
        }

        [Fact]
        public void ModelOnly_CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Counter>();

            // Act: use the model's IncrementCount
            cut.Instance.IncrementCount();
            cut.Instance.IncrementCount();

            // Assert
            cut.Instance.CurrentCount.Should().Be(2);
        }
    }
}
