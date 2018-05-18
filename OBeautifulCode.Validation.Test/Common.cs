﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Validation.Recipes.Test
{
    public static class Common
    {
        public static Parameter Clone(
            this Parameter parameter)
        {
            var result = new Parameter
            {
                Value = parameter.Value,
                Name = parameter.Name,
                HasBeenNamed = parameter.HasBeenNamed,
                HasBeenMusted = parameter.HasBeenMusted,
                HasBeenEached = parameter.HasBeenEached,
                HasBeenValidated = parameter.HasBeenValidated,
            };

            return result;
        }
    }
}
