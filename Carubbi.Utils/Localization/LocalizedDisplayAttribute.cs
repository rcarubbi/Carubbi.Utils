using System;
using System.ComponentModel;
using System.Resources;

namespace Carubbi.Utils.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LocalizedDisplayAttribute : DisplayNameAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="resourceType"></param>
        public LocalizedDisplayAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }


        /// <summary>
        /// 
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }



    }
}
