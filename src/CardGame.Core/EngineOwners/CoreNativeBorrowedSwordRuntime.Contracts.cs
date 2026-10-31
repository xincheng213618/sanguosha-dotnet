#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeBorrowedSwordCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreNativeBorrowedSwordCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreNativeBorrowedSwordCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}
