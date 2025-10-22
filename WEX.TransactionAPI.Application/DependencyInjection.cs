using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WEX.TransactionAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzkyNjI3MjAwIiwiaWF0IjoiMTc2MTA5MjIyNCIsImFjY291bnRfaWQiOiIwMTlhMDk0NjYzODc3ZDg0OTA5ZDkzNWJiYThhNjEyMiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazg0bWRoajFyZGI5eG5uYW13MzdrNGNzIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.BdYA5aNir4k8vyFN3wBeVueuSS3lN6ZnDzDShIpBzfciSvz91R0W71Rz4MxrZq2vZf24c-XGiqrqmz4x50R1GQTH5_uGB0_GCcrjQb7YjqGnXCZnlGvhUBIzSZha2Zrmlg7qroXO_T_5TFyUxUGXLsgF_wdJ0EfoN3ab149e8fwg7-mRR-yNPcLLIieKfeYIQzJ1aCIZTAOyU2llbl7QPvE9jBsb4CDBQWzot418tU_EHZhXcvUpufwDA-HKp6yJh_bgsGEXoz3qb3FHLbcq5ttZeGtYsjdj0djQaEJKc-Y6I6eeLWeiNmUJZQ2HjvdQl4kECmKMfiRisk-1i_yodA";
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
