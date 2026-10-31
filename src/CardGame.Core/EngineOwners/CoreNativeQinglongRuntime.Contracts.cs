#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeQinglongCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    ProgramCardLegality CardLegality { get; }
}
