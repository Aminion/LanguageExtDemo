using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace LanguageExtDemo
{
    class OptionExample
    {
        Option<uint> LocationId(string locationName) => Some(5u);
        Option<decimal> LocationDiscount(uint locationId) => None;
        decimal WithSeasonDiscountApplied(decimal baseDiscount) => throw new NotImplementedException();

        decimal BaseDiscount () => throw new NotImplementedException();

        decimal Discount(string locationName)
        {
            Option<uint> locationId = LocationId(locationName);
            Option<decimal> baseDiscount = locationId.Bind(LocationDiscount);
            Option<decimal> withSeasonDiscountApplied = baseDiscount.Map(WithSeasonDiscountApplied);

            var result = withSeasonDiscountApplied.IfNone(BaseDiscount);

            return result;
        }
    }
}
