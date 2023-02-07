using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stac.Extensions.Disaster
{
    /// <summary>
    /// Class of the object(s) described in the item or collection
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisastersItemClass
    {
        /// <summary>
        /// An Activation represents a Disaster event for which the Charter has been activated.
        /// An identifier is issued or recycled to be associated with a Call identifier.
        /// An Activation can be therefore linked to one or several Call(s).
        /// </summary>
        [JsonProperty("activation")]
        [EnumMember(Value = "activation")]
        Activation,

        /// <summary>
        /// Acquisition represents a satellite resource provided an Agency in the context of the Disaster.
        /// It can be an archived product or a planned acquisition. Each Acquisition records is associated to a Call.
        /// </summary>
        [JsonProperty("acquisition")]
        Acquisition,

        /// <summary>
        /// The Value Added Providers take the data provided by member agencies and interpret this,
        /// assessing what they see from the satellites and compiling it into Value Added Products.
        /// </summary>
        [JsonProperty("value_added_product")]
        ValueAddedProduct,

        /// <summary>
        /// Regions that are affected by the disaster and identified by the parties involved in the Charter process.
        /// </summary>
        [JsonProperty("area")]
        Area
    }
}
