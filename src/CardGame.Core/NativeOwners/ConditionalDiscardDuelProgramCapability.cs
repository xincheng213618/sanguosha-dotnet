#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ConditionalDiscardDuelProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime>> getCoreGameEngineConditionalDiscardDuelsRuntime) : IConditionalDiscardDuelProgramHost
{
    public SkillProgramStepOutcome DiscardSlashThenOtherCardAndUseDuel(ProgramSkillFrame frame) => getCoreGameEngineConditionalDiscardDuelsRuntime().Value.BeginConditionalDiscardDuel(frame);
}
