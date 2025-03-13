global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using System.Collections.Specialized;
global using System.Security.Claims;
global using System.Net;

global using Pharmacy.Application.Features.User.Services;
global using Pharmacy.Application.Features.UserLogs.Services;
global using Pharmacy.Application.Common.Enums;
global using Pharmacy.Application.Contracts.Infrastructure;
global using Pharmacy.Application.Contracts.Persistence;
global using Pharmacy.Application.Exceptions;
global using Pharmacy.Application.Models;
global using Pharmacy.Application.Models.Email;
global using Pharmacy.Application.Models.SMS;
global using Pharmacy.Application.Wrapper;
global using Pharmacy.Domain;

global using IdentityModel;
global using IdentityModel.Client;
global using IdentityServer4.Extensions;
global using IdentityServer4.Models;
global using IdentityServer4.Services;
global using IdentityServer4.Validation;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
