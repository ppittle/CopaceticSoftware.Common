//----------------------------------------------------------------------- 
// <copyright file="TimeLimitCommandThrottlerTest.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, June 12, 2013 3:49:52 PM</date> 
// Licensed under the Apache License, Version 2.0,
// you may not use this file except in compliance with one of the Licenses.
//  
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an 'AS IS' BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright> 
//-----------------------------------------------------------------------

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CopaceticSoftware.Common.Infrastructure;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace CopaceticSoftware.Common.Tests.Infrastructure
{
    [TestFixture]
    public class TimeLimitCommandThrottlerTest : TestBase
    {
        [Test]
        public void CanThrottleWithDropBehavior()
        {
            const int executionLimit = 100;

            int executionCounter = 0;

            var throttler = new TimeLimitCommandThrottler(executionLimit);

            Parallel.For(0, executionLimit*10, i => throttler.Execute(() => Interlocked.Increment(ref executionCounter)));

            executionCounter.ShouldEqual(executionLimit);
        }

        [Test]
        public void CanThrottleWithBlockBehavior()
        {
            const int executionLimit = 10;

            int executionCounter = 0;

            var throttler = new TimeLimitCommandThrottler(executionLimit, 50, 
                TimeLimitCommandThrottler.CommandThrottlerLimitingBehavior.Block);

            var sw = Stopwatch.StartNew();

            Parallel.For(0, executionLimit * 2, i => throttler.Execute(() => Interlocked.Increment(ref executionCounter)));

            sw.ElapsedMilliseconds.ShouldBeGreaterThan(50);

            executionCounter.ShouldEqual(2* executionLimit);
        }
    }
}
