using System;

using System.ComponentModel;

namespace hello.restaurant.api.Models
{
    // Add the attribute Flags or FlagsAttribute.
    [Flags]
    public enum PlaceType
    {

        [Description("restaurant")]
        RESTAURANT,


        [Description("food")]
        FOOD,

    }
}