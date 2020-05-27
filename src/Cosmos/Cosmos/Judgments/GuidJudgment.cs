﻿using System;
using System.Text.RegularExpressions;

namespace Cosmos.Judgments
{
    /// <summary>
    /// Guid Judgment Utilities
    /// </summary>
    public static class GuidJudgment
    {
        /// <summary>
        /// To judge whether the <see cref="Guid"/> is null or empty.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(Guid guid) => guid == Guid.Empty;

        /// <summary>
        /// To judge whether the <see cref="Guid"/> is null or empty.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(Guid? guid) => guid is null || IsNullOrEmpty(guid.Value);

        private static readonly Regex GuidSchema
            = new Regex("^[A-Fa-f0-9]{32}$|" +
                        "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                        "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2},{0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

        /// <summary>
        /// To judge whether the string is <see cref="Guid"/> or not.
        /// </summary>
        /// <param name="guidStr"></param>
        /// <returns></returns>
        public static bool IsValid(string guidStr) =>
            !string.IsNullOrWhiteSpace(guidStr) && GuidSchema.Match(guidStr).Success;
    }
}