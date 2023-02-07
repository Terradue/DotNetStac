using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Disaster
{
    /// <summary>
    /// The disaster:types is the commonly used category name to classify the type of disaster.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisastersType
    {
        /// <summary>
        /// Earthquakes occur following the release of energy when tectonic plates move apart. 
        /// These plates move in currents in the Earth's lithosphere and the edges, 
        /// which have been mapped to fault lines, sometimes collide.
        /// </summary>
        [JsonProperty("earthquake")]
        Earthquake,

        /// <summary>
        /// Wildfires occur when vegetated areas are set alight and are particularly common during hot and dry periods. 
        /// They can occur in forests, grasslands, brush and deserts, and with sufficient wind can rapidly spread.
        /// </summary>
        [JsonProperty("fire")]
        Fire,

        /// <summary>
        /// Large Flooding occurs when bodies of water flow onto land that is normally dry over a period of days on a large area.
        /// </summary>
        [JsonProperty("flood_large")]
        FloodLarge,

        /// <summary>
        /// Flash Floods occurs when storms bring large quantities of precipitation in a matter of minutes.
        /// </summary>
        [JsonProperty("flood_flash")]
        FloodFlash,

        /// <summary>
        // Ice on the surface of water or in compacted snow makes for treacherous conditions and can result in injuries if people slip and fall.
        /// Water sources may freeze, cutting off access for residents to clean water or heat.
        /// </summary>
        [JsonProperty("ice")]
        Ice,

        /// <summary>
        /// Landslides occur when ground on slopes becomes unstable. The unstable ground collapses and flows down the side of a hill or mountain,
        /// and can consist of earth, rocks, mud and any debris which may be caught in its wake.
        /// </summary>
        [JsonProperty("landslide")]
        Landslide,

        /// <summary>
        /// Tropical cyclones are weather phenomena which form over the Atlantic and northeast Pacific Oceans through the release of energy generated
        /// by evaporation and saturation of water on the ocean's surface. This category affecting urban or rural area.
        /// </summary>
        [JsonProperty("storm_hurricane_rural")]
        StormHurricaneRural,

        /// <summary>
        /// Tropical cyclones are weather phenomena which form over the Atlantic and northeast Pacific Oceans through the release of energy generated
        /// by evaporation and saturation of water on the ocean's surface. They are categorized affecting urban or rural area.
        /// </summary>
        [JsonProperty("storm_hurricane_urban")]
        StormHurricaneUrban,

        /// <summary>
        /// sunamis are seismic sea waves and typically occur as a result of underwater earthquakes or volcanic eruptions.
        /// </summary>
        [JsonProperty("tsunami")]
        Tsunami,

        /// <summary>
        /// Oil spills occur when petroleum oil is released into the ocean following accidents,
        /// such as vessels crashing or damage and problems with oil platforms and drilling.
        /// </summary>
        [JsonProperty("oil_spill")]
        OilSpill,

        /// <summary>
        /// Volcanoes are points in the Earth's crust which have ruptured, allowing lava, ash, rocks and gas
        /// to erupt during periods of seismic activity.
        /// </summary>
        [JsonProperty("storm_hurricane_rural")]
        Volcano,

        /// <summary>
        /// Snow Hazard occurs when temperatures drop below the freezing point, and there is sufficient water in clouds.
        /// Snow storms can quickly cause disruption to inhabited areas if the ground temperature is cold enough for the snow to settle.
        /// </summary>
        [JsonProperty("snow_hazard")]
        SnowHazard,

        /// <summary>
        /// In addition to the most common forms of natural disasters, there are other types of disasters which may benefit from satellite observations.
        /// </summary>
        [JsonProperty("other")]
        Other,
    }
}
