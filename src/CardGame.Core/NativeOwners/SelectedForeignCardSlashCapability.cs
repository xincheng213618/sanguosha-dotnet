#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SelectedForeignCardSlashCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime) : ISelectedForeignCardSlashHost
{
    public SkillProgramStepOutcome UseSelectedForeignCardAsSlash(ProgramSkillFrame frame, int actorSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().BeginSelectedForeignCardSlash(frame, actorSeat);
}
