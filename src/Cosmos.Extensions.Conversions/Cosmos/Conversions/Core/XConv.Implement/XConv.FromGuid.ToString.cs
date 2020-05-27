using System;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace Cosmos.Conversions.Core
{
    internal static partial class XConv
    {
        private static bool FromGuidToString(Guid guid, CastingContext context, string defaultStr, out object result)
        {
            result = StringConv.GuidToString(guid, context, defaultStr);
            return true;
        }

        private static bool FromNullableGuidToString(Guid? guid, CastingContext context, out object result)
        {
            result = StringConv.GuidToString(guid, context);
            return true;
        }
    }
}