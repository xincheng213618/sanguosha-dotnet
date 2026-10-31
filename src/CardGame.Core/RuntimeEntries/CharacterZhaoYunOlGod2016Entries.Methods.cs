#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunOlGod2016Entries
{
    internal global::CardGame.Core.CardActionContext CaptureHpScaledCardUseAction(global::CardGame.Core.CardActionContext action) => _resolve().CaptureHpScaledCardUseAction(action);
    internal bool HasPaidHpScaledViewAsAction(global::CardGame.Core.CardActionContext action) => _resolve().HasPaidHpScaledViewAsAction(action);
    internal bool HpScaledProgramSelectionIsExact(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HpScaledProgramSelectionIsExact(f);
    internal bool HpScaledViewAsCountForbidden(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().HpScaledViewAsCountForbidden(q);
    internal void IssueHpScaledProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramActivation a) => _resolve().IssueHpScaledProgramSelection(f, a);
}
