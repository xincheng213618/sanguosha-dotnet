#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DualColorDuelProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDualColorDuelProgramsEntries> getCoreGameEngineDualColorDuelProgramsRuntime) : IDualColorDuelProgramHost
{
    public SkillProgramStepOutcome ChoosePrivateColorsDiscardAndDuel(ProgramSkillFrame frame) => getCoreGameEngineDualColorDuelProgramsRuntime().BeginDualColorDuel(frame);
}
