#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SelectedForeignCardSlashCapability(Func<RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries> getCoreGameEngineForeignSelectedCardSlashRuntime) : ISelectedForeignCardSlashHost
{
    public SkillProgramStepOutcome UseSelectedForeignCardAsSlash(ProgramSkillFrame frame, int actorSeat) => getCoreGameEngineForeignSelectedCardSlashRuntime().BeginSelectedForeignCardSlash(frame, actorSeat);
}
