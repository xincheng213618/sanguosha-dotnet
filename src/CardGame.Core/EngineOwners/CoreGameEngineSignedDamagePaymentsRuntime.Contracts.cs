#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSignedDamagePaymentsCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
