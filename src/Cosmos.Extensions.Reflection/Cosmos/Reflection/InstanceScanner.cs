using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmos.Reflection
{
    /// <summary>
    /// Instance Scanner
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public abstract class InstanceScanner<TClass> : TypeScanner
    {
        /// <summary>
        /// Create a new <see cref="InstanceScanner{TClass}"/> instance.
        /// </summary>
        protected InstanceScanner() { }

        /// <summary>
        /// Create a new <see cref="InstanceScanner{TClass}"/> instance.
        /// </summary>
        /// <param name="scannerName"></param>
        protected InstanceScanner(string scannerName) : base(scannerName) { }

        /// <summary>
        /// Create a new <see cref="InstanceScanner{TClass}"/> instance.
        /// </summary>
        /// <param name="baseType"></param>
        protected InstanceScanner(Type baseType) : base(baseType) { }

        /// <summary>
        /// Create a new <see cref="InstanceScanner{TClass}"/> instance.
        /// </summary>
        /// <param name="scannerName"></param>
        /// <param name="baseType"></param>
        protected InstanceScanner(string scannerName, Type baseType) : base(scannerName, baseType) { }

        /// <summary>
        /// Scan, and return instances.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TClass> ScanAndReturnInstances() =>
            Scan().Select(type => type.CreateInstance<TClass>());
    }
}