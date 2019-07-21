using System;
using ProcessFinder;
using Xunit;

namespace UnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async void InvalidProcessNameSearch()
        {
            string processName = "dummy";
            var testMethod = new Analysis();
            var result=await testMethod.SearchProcessNameAsync(processName);
            Assert.False(result);
        }
        [Fact]
        public async void InvalidProcessNameSearchWithNullAsync()
        {
            string processName = "";
            var testMethod = new Analysis();
            await Assert.ThrowsAsync<ArgumentException>(async () => await testMethod.SearchProcessNameAsync(processName));

        }
        [Fact]
        public async void ValidProcessSearchAsync()
        {
            string processName = "chrome";
            var testMethod = new Analysis();
            var result = await testMethod.SearchProcessNameAsync(processName);
            Assert.True(result);
        }
    }
}
