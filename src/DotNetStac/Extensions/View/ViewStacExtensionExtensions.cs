// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: ViewStacExtensionExtensions.cs

namespace Stac.Extensions.View
{
    public static class ViewStacExtensionExtensions
    {
        public static ViewStacExtension ViewExtension(this StacItem stacItem)
        {
            return new ViewStacExtension(stacItem);
        }
    }
}
