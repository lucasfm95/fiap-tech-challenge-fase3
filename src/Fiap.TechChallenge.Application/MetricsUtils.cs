using System.Diagnostics.CodeAnalysis;
using Prometheus;

namespace Fiap.TechChallenge.Application;

[ExcludeFromCodeCoverage]
public static class MetricsUtils
{
    public static readonly Counter InsertCounter = Metrics.CreateCounter("contact_insert_request_count",
        "Number of request to insert contact.", new CounterConfiguration
        {
            LabelNames = new[] { "Insert" }
        });

    public static readonly Counter UpdateCounter = Metrics.CreateCounter("contact_update_request_count",
        "Number of request to update contact.", new CounterConfiguration
        {
            LabelNames = new[] { "Update" }
        });

    public static readonly Counter DeleteCounter = Metrics.CreateCounter("contact_delete_request_count",
        "Number of request to delete contact.", new CounterConfiguration
        {
            LabelNames = new[] { "Delete" }
        });
}