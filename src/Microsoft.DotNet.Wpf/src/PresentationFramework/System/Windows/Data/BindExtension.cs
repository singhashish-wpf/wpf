// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Runtime.CompilerServices;
using System.Windows.Markup;
namespace System.Windows.Data
{
    /// <summary>
    /// Class for Xaml markup extension for Bind.
    /// </summary>
    [MarkupExtensionReturnType(typeof(object))]
    public class Bind : MarkupExtension
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Bind()
        {
        }

        public Bind(string path)
        {
        }
        /// <summary>
        /// Return an object that should be set on the targetObject's targetProperty
        /// for this markup extension. In this case it is simply null.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object to set on this property.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => null;
    }
}
