//----------------------------------------------------------------------- 
// <copyright file="TimeLimitCommandThrottler.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, June 12, 2013 3:30:30 PM</date> 
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

using System;
using System.ComponentModel;
using System.Threading;
using CopaceticSoftware.Common.Exceptions;

namespace CopaceticSoftware.Common.Infrastructure
{
    /// <summary>
    /// Restricts the number of times the <see cref="Execute"/> method
    /// can be called in a given <see cref="TimeSpan"/>.  
    /// </summary>
    /// <remarks>
    /// Algorithm based on http://en.wikipedia.org/wiki/Token_bucket
    /// as recommended in http://stackoverflow.com/questions/1450217/what-is-the-best-way-to-implement-a-rate-limiting-algorithm-for-web-requests?rq=1
    /// </remarks>
    public class TimeLimitCommandThrottler : ICommandThrottler
    {
        #region enum CommandThrottlerLimitingBehavior

        /// <summary>
        /// Indicates how the <see cref="TimeLimitCommandThrottler"/>
        /// should behave once the number of calls to 
        /// <see cref="TimeLimitCommandThrottler.Execute"/> has exceeded
        /// the <see cref="TimeLimitCommandThrottler.ExecutionLimit"/>
        /// for the given 
        /// </summary>
        public enum CommandThrottlerLimitingBehavior
        {
            /// <summary>
            /// Extra calls to <see cref="TimeLimitCommandThrottler.Execute"/>
            /// will be dropped.
            /// </summary>
            Drop,

            /// <summary>
            /// The current Thread will block until 
            /// <see cref="TimeLimitCommandThrottler.Execute"/>
            /// can be called.
            /// </summary>
            Block
        }

        #endregion

        #region Properties

        public int ExecutionLimit { get; set; }
        public CommandThrottlerLimitingBehavior LimitingBehavior { get; set; }
        public TimeSpan ThrottlingInterval { get; set; }

        #endregion

        #region Data Members

        private object _tokenLock = new object();

        private int _remainingTokens;
        private DateTime _nextTokenRefreshInterval;

        #endregion

        #region Constructor

        public TimeLimitCommandThrottler(int executionLimit,
                                         int throttlingIntervalInMilliseconds = 60000,
                                         CommandThrottlerLimitingBehavior limitingBehavior =
                                             CommandThrottlerLimitingBehavior.Drop)
            : this(executionLimit, new TimeSpan(0, 0, 0, 0, throttlingIntervalInMilliseconds), limitingBehavior)
        {
        }

        public TimeLimitCommandThrottler(int executionLimit,
                                         TimeSpan throttlingInterval,
                                         CommandThrottlerLimitingBehavior limitingBehavior =
                                             CommandThrottlerLimitingBehavior.Drop)
        {
            Ensure.ArgumentIsGreaterThanZero(executionLimit, "executionLimit");

            ExecutionLimit = executionLimit;
            ThrottlingInterval = throttlingInterval;
            LimitingBehavior = limitingBehavior;

            RefilTokenBucket();
        }

        #endregion
        
        private void RefilTokenBucket()
        {
            _remainingTokens = ExecutionLimit;
            _nextTokenRefreshInterval = DateTime.Now.Add(ThrottlingInterval);
        }

        private bool CanHaveAToken()
        {
            lock (_tokenLock)
            {
                //There is an available token
                if (_remainingTokens > 0)
                {
                    _remainingTokens--;
                    return true;
                }
                
                //We can refill the token bucket
                if (_nextTokenRefreshInterval < DateTime.Now)
                {
                    RefilTokenBucket();
                    return CanHaveAToken();
                }

                //Execute Command Throttler Behavior
                switch (LimitingBehavior)
                {
                    case CommandThrottlerLimitingBehavior.Drop:
                        return false;
                    case CommandThrottlerLimitingBehavior.Block:
                        //Sleep until we can refill the token bucket
                        Thread.Sleep(_nextTokenRefreshInterval.Subtract(DateTime.Now));
                        return CanHaveAToken();
                    default:
                        throw new UnknownEnumValueException<CommandThrottlerLimitingBehavior>(LimitingBehavior);
                        #region Throw Exception
                        throw new InvalidEnumArgumentException(
                            string.Format(
                                Strings.UnknownEnumValueExceptionMessage,
                                typeof (TimeLimitCommandThrottler).Name,
                                typeof (CommandThrottlerLimitingBehavior).Name,
                                Enum.GetName(typeof (CommandThrottlerLimitingBehavior), LimitingBehavior)));
                        #endregion
                }
            }
        }

        public void Execute(Action action)
        {
            if (CanHaveAToken())
                action();
        }
    }
}
