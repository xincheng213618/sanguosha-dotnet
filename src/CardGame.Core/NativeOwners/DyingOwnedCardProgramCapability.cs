#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DyingOwnedCardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnedCardSelectionsEntries> getCoreGameEngineDyingOwnedCardSelectionsRuntime) : IDyingOwnedCardProgramHost
{
    public SkillProgramStepOutcome SelectDyingOwnedCard(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineDyingOwnedCardSelectionsRuntime().SelectDyingOwnedCard(f, e);
}
