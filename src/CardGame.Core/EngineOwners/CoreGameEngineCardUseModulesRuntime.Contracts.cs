#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardUseModulesCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId);
    ProgramCardLegality CardLegality { get; }
}
