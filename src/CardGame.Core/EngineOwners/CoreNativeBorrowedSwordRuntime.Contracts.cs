#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeBorrowedSwordCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreNativeBorrowedSwordCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICoreNativeBorrowedSwordCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    ProgramCardLegality CardLegality { get; }
}
