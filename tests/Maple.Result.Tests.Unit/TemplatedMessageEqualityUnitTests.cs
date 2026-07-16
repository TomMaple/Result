// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using Sut = Maple.Result.TemplatedMessage;

namespace Maple.Result.Tests.Unit;

public class TemplatedMessageEqualityUnitTests
{
    #region value equality

    [Fact]
    public void Equals_TwoMessagesWithoutParams_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id");
        var second = new Sut("template.id");

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoMessagesWithEqualParams_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });
        var second = new Sut("template.id", new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_MessagesWithParamsInDifferentOrder_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });
        var second = new Sut("template.id", new Dictionary<string, object>
        {
            ["bonusPoints"] = 100,
            ["userName"] = "johnny"
        });

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamValues_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object> { ["userName"] = "jane" });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamKeys_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object> { ["userId"] = "johnny" });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamCount_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentTemplateId_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.first");
        var second = new Sut("template.second");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessageWithNullParamsVsEmptyParams_AreNotEqual()
    {
        // Arrange
        var withNullParams = new Sut("template.id");
        var withEmptyParams = new Sut("template.id", new Dictionary<string, object>());

        // Act & Assert
        withNullParams.Equals(withEmptyParams).ShouldBeFalse();
        (withNullParams == withEmptyParams).ShouldBeFalse();
    }

    #endregion

    #region read-only collection

    [Fact]
    public void Params_ExposedAsReadOnly()
    {
        // Arrange
        var sut = new Sut("template.id", new Dictionary<string, object> { ["userName"] = "johnny" });

        // Act & Assert
        sut.Params.ShouldBeAssignableTo<IReadOnlyDictionary<string, object>>();
    }

    #endregion
}
