#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessBasicEntries
{
    internal bool CanUseRoundOnceCardlessBasic(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse) => _resolve().CanUseRoundOnceCardlessBasic(actor, instance, rule, forResponse);
    internal RoundOnceCardlessBasicPaymentReceipt ConsumeRoundOnceCardlessBasic(CardConversionSource source, bool forResponse) => _resolve().ConsumeRoundOnceCardlessBasic(source, forResponse);
    internal bool IsIssuedRoundOnceCardlessBasicUse(RoundOnceCardlessBasicUseReceipt? receipt, ResolutionFrame owner, CardActionContext action) => _resolve().IsIssuedRoundOnceCardlessBasicUse(receipt, owner, action);
    internal RoundOnceCardlessBasicUseReceipt IssueRoundOnceCardlessBasicUse(ResolutionFrame owner, CardActionContext action, CardConversionSource source) => _resolve().IssueRoundOnceCardlessBasicUse(owner, action, source);
    internal bool RoundOnceCardlessBasicUsed(int ownerSeat, string skillId, string group) => _resolve().RoundOnceCardlessBasicUsed(ownerSeat, skillId, group);
}
