#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramOwnedZoneDiscardFacts(Func<RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime) : IProgramOwnedZoneDiscardFacts
{
    public bool HasAtomicDiscardReplacement => getCoreHellMengpoProgramsRuntime().HasHellShiyouCapability;

    public CardLocation? DiscardSource(CardMovementRecord movement) => getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(movement);
}
