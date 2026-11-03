#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsEntries
{
    internal void RecordCurrentEnhancementSlashRedirect(ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => _resolve().RecordCurrentEnhancementSlashRedirect(redirector, useId, previousSeat, newSeat);
}
