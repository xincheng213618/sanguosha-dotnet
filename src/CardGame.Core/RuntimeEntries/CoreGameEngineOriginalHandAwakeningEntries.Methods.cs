#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandAwakeningEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOriginalHandAwakening(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginOriginalHandAwakening(supplied, effect);
    internal bool OriginalHandAwakeningFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OriginalHandAwakeningFirstChild(f, child);
    internal bool ResumeOriginalHandAwakening(long id) => _resolve().ResumeOriginalHandAwakening(id);
    internal bool ValidOriginalHandAwakeningReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOriginalHandAwakeningReceipt(f);
}
