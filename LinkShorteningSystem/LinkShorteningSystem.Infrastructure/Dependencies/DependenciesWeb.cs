﻿using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure
{
    public static class DependenciesWeb
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(context => context.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
        }
    }
}