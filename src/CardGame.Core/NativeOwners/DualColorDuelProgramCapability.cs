#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DualColorDuelProgramCapability(Func<RuntimeEntries.CoreGameEngineDualColorDuelProgramsEntries> getCoreGameEngineDualColorDuelProgramsRuntime) : IDualColorDuelProgramHost
{
    public SkillProgramStepOutcome ChoosePrivateColorsDiscardAndDuel(ProgramSkillFrame frame) => getCoreGameEngineDualColorDuelProgramsRuntime().BeginDualColorDuel(frame);
}
