#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlaySlashRecastsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginActualPlaySlashRecast(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginActualPlaySlashRecast(supplied);
    internal bool ResumeEndingPairOrSlashRecast(long id) => _resolve().ResumeEndingPairOrSlashRecast(id);
    internal bool ValidPlaySlashRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPlaySlashRecast(f);
}
