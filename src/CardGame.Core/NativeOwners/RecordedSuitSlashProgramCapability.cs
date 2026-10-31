#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RecordedSuitSlashProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecordedSuitSlashEntries> getCoreGameEngineRecordedSuitSlashRuntime) : IRecordedSuitSlashProgramHost
{
    public void RevealRecordedSuitSlash(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineRecordedSuitSlashRuntime().RevealRecordedSuitSlash(frame, effect);
}
