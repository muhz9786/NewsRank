// ================================
// Name: Spider
// Description: 爬虫的抽象类；
// Author: Muhz
// Create Date: 2019-07-10
// ================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NewsRank.Controllers
{
    abstract public class Spider
    {
        public IConfiguration configuration;    // Configuration

        /// <summary>
        /// Initializes a new Spider with configuration.
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Spider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Sipder running.
        /// </summary>
        abstract public void Run();
    }
}
