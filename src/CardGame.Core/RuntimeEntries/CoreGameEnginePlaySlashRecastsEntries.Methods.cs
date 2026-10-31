#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlaySlashRecastsEntries
{
    internal SkillProgramStepOutcome BeginActualPlaySlashRecast(ProgramSkillFrame supplied) => _resolve().BeginActualPlaySlashRecast(supplied);
    internal bool ResumeEndingPairOrSlashRecast(long id) => _resolve().ResumeEndingPairOrSlashRecast(id);
    internal bool ValidPlaySlashRecast(ProgramSkillFrame f) => _resolve().ValidPlaySlashRecast(f);
}
