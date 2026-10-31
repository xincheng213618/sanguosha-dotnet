#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessBasicEntries
{
    internal bool CanUseRoundOnceCardlessBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse) => _resolve().CanUseRoundOnceCardlessBasic(actor, instance, rule, forResponse);
    internal global::CardGame.Core.RoundOnceCardlessBasicPaymentReceipt ConsumeRoundOnceCardlessBasic(global::CardGame.Core.CardConversionSource source, bool forResponse) => _resolve().ConsumeRoundOnceCardlessBasic(source, forResponse);
    internal bool IsIssuedRoundOnceCardlessBasicUse(global::CardGame.Core.RoundOnceCardlessBasicUseReceipt? receipt, global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => _resolve().IsIssuedRoundOnceCardlessBasicUse(receipt, owner, action);
    internal global::CardGame.Core.RoundOnceCardlessBasicUseReceipt IssueRoundOnceCardlessBasicUse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source) => _resolve().IssueRoundOnceCardlessBasicUse(owner, action, source);
    internal bool RoundOnceCardlessBasicUsed(int ownerSeat, string skillId, string group) => _resolve().RoundOnceCardlessBasicUsed(ownerSeat, skillId, group);
}
