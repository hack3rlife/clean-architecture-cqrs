using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="https://josef.codes/you-are-probably-still-using-httpclient-wrong-and-it-is-destabilizing-your-software/"/>
    public class EndToEndClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EndToEndClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public EndToEndClient Create()
        {
            return _serviceProvider.GetRequiredService<EndToEndClient>();
        }

    }
}
