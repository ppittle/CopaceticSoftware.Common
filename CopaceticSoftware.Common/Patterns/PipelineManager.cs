//----------------------------------------------------------------------- 
// <copyright file="PipelineManager.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, May 29, 2013 6:40:54 PM</date> 
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
using System.Collections.Generic;
using CopaceticSoftware.Common.Infrastructure;
using log4net;

namespace CopaceticSoftware.Common.Patterns
{
    public interface IPipelineStep<in TPipelineStateManager>
    {
        /// <summary>
        /// Performs a task in a Pipeline
        /// </summary>
        /// <param name="manager">The State Manager</param>
        /// <returns>
        /// <c>True</c> if the step succeded, <c>false</c> otherwise.
        /// </returns>
        bool PerformTask(TPipelineStateManager manager);
    }

    public static class PipelineStepExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger("CopaceticSoftware.Common.Patterns.PipelineExectuor");

        public static bool RunPipeline<TPipelineStateManager>(
            this IEnumerable<IPipelineStep<TPipelineStateManager>> steps,
            TPipelineStateManager stateManager,
            Func<IPipelineStep<TPipelineStateManager>, bool> haltOnStepFailing = null,
            Func<IPipelineStep<TPipelineStateManager>, Exception, bool> throwException = null)
        {
            Ensure.ArgumentNotNull(steps, "steps");
            Ensure.ArgumentNotNull(stateManager, "stateManager");

            bool pipelineSucceded = true;

            foreach (var step in steps)
            {
                Log.InfoFormat("Starting Step [{0}]", step.GetType().Name);

                try
                {
                    if (!step.PerformTask(stateManager))
                    {
                        pipelineSucceded = false;

                        var halt = (null == haltOnStepFailing || haltOnStepFailing(step));

                        var logMessage = halt
                                             ? "Step [{0}] failed.  Pipeline will not continue."
                                             : "Step [{0}] failed.  Pipeline will continue.";

                        Log.WarnFormat(logMessage, step.GetType().Name);

                        if (halt)
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(
                        string.Format(
                            "Unhandled exception executing Step [{0}]: {1}",
                            step.GetType().FullName, e.Message), e);

                    if (null == throwException || throwException(step, e))
                        throw;

                    return false;
                }

                Log.InfoFormat("Finishing Step [{0}]", step.GetType().Name);
            }

            return pipelineSucceded;
        }
    }
}
