//----------------------------------------------------------------------- 
// <copyright file="TestBase.cs" company="Copacetic Software"> 
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

using NBehave.Spec.NUnit;

namespace CopaceticSoftware.Common.Tests
{
    public class TestBase : SpecBase
    {
        public TestBase()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
