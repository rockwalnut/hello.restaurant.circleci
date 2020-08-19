using System;

using System.Collections.Generic;
using System.Linq;

//validator
using FluentValidation;
using FluentValidation.Results;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Validator
{
    public class PlanValidator : AbstractValidator<RestaurantParams>
    {
        public PlanValidator()
        {
            //location
            RuleFor(x => x.Keyword).NotNull().NotEmpty();

        }
    }
}