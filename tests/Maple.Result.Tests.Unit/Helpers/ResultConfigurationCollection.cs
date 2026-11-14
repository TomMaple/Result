// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Result.Tests.Unit.Helpers;

/// <summary>
///     The collection of the tests that change the global <see cref="ResultConfiguration" />. They run alone, after
///     the other tests, so a changed setting cannot affect a test that runs at the same time.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ResultConfigurationCollection
{
    public const string Name = "ResultConfiguration";
}
