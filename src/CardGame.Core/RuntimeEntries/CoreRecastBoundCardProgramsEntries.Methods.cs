#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreRecastBoundCardProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramBoundCardRecast(long frameId, string sourceBind) => _resolve().BeginProgramBoundCardRecast(frameId, sourceBind);
    internal bool ResumeProgramBoundCardRecast(long frameId) => _resolve().ResumeProgramBoundCardRecast(frameId);
}
