﻿using System;
using System.Collections.Generic;
using Exceptionless.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Exceptionless.Core.Configuration {
    public class StorageOptions {
        public string ConnectionString { get; internal set; }
        public string Provider { get; internal set; }
        public Foundatio.Utility.DataDictionary Data { get; internal set; }

        public string Scope { get; internal set; }
        public string ScopePrefix { get; internal set; }
    }

    public class ConfigureStorageOptions : IConfigureOptions<StorageOptions> {
        private readonly IConfiguration _configuration;

        public ConfigureStorageOptions(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void Configure(StorageOptions options) {
            options.Scope = _configuration.GetValue<string>(nameof(options.Scope), String.Empty);
            options.ScopePrefix = !String.IsNullOrEmpty(options.Scope) ? options.Scope + "-" : String.Empty;

            string cs = _configuration.GetConnectionString("storage");
            options.Data = cs.ParseConnectionString();
            options.Provider = options.Data.GetString(nameof(options.Provider).ToLowerInvariant());
            options.ConnectionString = options.Data.BuildConnectionString(new HashSet<string> { nameof(options.Provider).ToLowerInvariant() });
        }
        
    }
}