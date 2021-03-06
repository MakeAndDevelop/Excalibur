﻿using Excalibur.Base.Configuration;
using Excalibur.Base.State;
using Excalibur.Tests.Cross.Core.Configuration;

namespace Excalibur.Tests.Cross.Core.State
{
    public class ApplicationState : BaseState<Config>, IApplicationState
    {
        public ApplicationState(IConfigurationManager configurationManager) : base(configurationManager)
        {
        }

        public string Email
        {
            get { return Config.Email; }
            set { Config.Email = value; }
        }
    }
}
