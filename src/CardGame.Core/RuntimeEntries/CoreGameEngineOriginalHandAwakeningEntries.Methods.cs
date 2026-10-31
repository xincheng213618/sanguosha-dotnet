#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandAwakeningEntries
{
    internal SkillProgramStepOutcome BeginOriginalHandAwakening(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginOriginalHandAwakening(supplied, effect);
    internal bool OriginalHandAwakeningFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OriginalHandAwakeningFirstChild(f, child);
    internal bool ResumeOriginalHandAwakening(long id) => _resolve().ResumeOriginalHandAwakening(id);
    internal bool ValidOriginalHandAwakeningReceipt(ProgramSkillFrame f) => _resolve().ValidOriginalHandAwakeningReceipt(f);
}
