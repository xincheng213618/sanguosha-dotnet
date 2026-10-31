#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DualColorDuelProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime>> getCoreGameEngineDualColorDuelProgramsRuntime) : IDualColorDuelProgramHost
{
    public SkillProgramStepOutcome ChoosePrivateColorsDiscardAndDuel(ProgramSkillFrame frame) => getCoreGameEngineDualColorDuelProgramsRuntime().Value.BeginDualColorDuel(frame);
}
