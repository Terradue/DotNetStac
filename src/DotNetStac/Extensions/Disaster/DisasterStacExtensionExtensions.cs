// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DisasterStacExtensionExtensions.cs

namespace Stac.Extensions.Disaster
{
    /// <summary>
    /// Extension methods for accessing EO extension
    /// </summary>
    public static class DisasterStacExtensionExtensions
    {
        /// <summary>
        /// Initilize a DisastersCharterStacExtension class from a STAC item
        /// </summary>
        /// <param name="stacObject">The STAC item</param>
        /// <returns>The DisastersCharterStacExtension class</returns>
        public static DisastersCharterStacExtension DisasterExtension(this IStacObject stacObject)
        {
            return new DisastersCharterStacExtension(stacObject);
        }

        /// <summary>
        /// Initilize a DisastersCharterStacExtension
        /// </summary>
        /// <param name="disasterStacExtension">The DisastersCharterStacExtension to initilize</param>
        /// <param name="disastersItemClass">The class of the item</param>
        /// <param name="activationId">The activation id</param>
        /// <param name="callIds">The call ids</param>
        public static void Init(this DisastersCharterStacExtension disasterStacExtension, DisastersItemClass disastersItemClass, int activationId, int[] callIds)
        {
            disasterStacExtension.Class = disastersItemClass;
            disasterStacExtension.ActivationId = activationId;
            disasterStacExtension.CallIds = callIds;
        }
    }
}
