#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramOwnedZoneDiscardFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime) : IProgramOwnedZoneDiscardFacts
{
    public bool HasAtomicDiscardReplacement => getCoreHellMengpoProgramsRuntime().HasHellShiyouCapability;

    public CardLocation? DiscardSource(CardMovementRecord movement) => getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(movement);
}
