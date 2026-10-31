#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunOlGod2016Entries
{
    internal CardActionContext CaptureHpScaledCardUseAction(CardActionContext action) => _resolve().CaptureHpScaledCardUseAction(action);
    internal bool HasPaidHpScaledViewAsAction(CardActionContext action) => _resolve().HasPaidHpScaledViewAsAction(action);
    internal bool HpScaledViewAsCountForbidden(ProgramViewAsLegalityQuery q) => _resolve().HpScaledViewAsCountForbidden(q);
    internal void IssueHpScaledProgramSelection(ProgramSkillFrame f, SkillProgramActivation a) => _resolve().IssueHpScaledProgramSelection(f, a);
}
