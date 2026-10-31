#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoXiaoQiaoOlEntries
{
    internal bool AdvanceDaQiaoXiaoQiaoChildren() => _resolve().AdvanceDaQiaoXiaoQiaoChildren();
    internal void AnswerDaQiaoXiaoQiao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerDaQiaoXiaoQiao(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame input, bool maintain) => _resolve().BeginDaQiaoXiaoQiao(input, maintain);
    internal long CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(global::CardGame.Core.CharacterState owner, long existingStamp) => _resolve().CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(owner, existingStamp);
    internal global::CardGame.Core.PromptChoice ChooseDaQiaoXiaoQiaoAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseDaQiaoXiaoQiaoAi(p, f);
    internal void ContinueDaQiaoXiaoQiao(long id) => _resolve().ContinueDaQiaoXiaoQiao(id);
    internal bool ContinuePaidDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidDaQiaoXiaoQiao(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DaQiaoXiaoQiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoChoices(f);
    internal bool DaQiaoXiaoQiaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().DaQiaoXiaoQiaoFirstChild(f, child);
    internal bool DaQiaoXiaoQiaoHasPile(int seat) => _resolve().DaQiaoXiaoQiaoHasPile(seat);
    internal bool DaQiaoXiaoQiaoLive(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoLive(f);
    internal bool DaQiaoXiaoQiaoLuoyanSourceLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant source) => _resolve().DaQiaoXiaoQiaoLuoyanSourceLive(owner, source);
    internal bool DaQiaoXiaoQiaoMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().DaQiaoXiaoQiaoMovementMatches(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? DaQiaoXiaoQiaoObserverRoot() => _resolve().DaQiaoXiaoQiaoObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoOwned(global::CardGame.Core.ProgramSkillFrame f, bool handOnly, bool discard) => _resolve().DaQiaoXiaoQiaoOwned(f, handOnly, discard);
    internal bool DaQiaoXiaoQiaoParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoParent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoPileCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoPileCards(f);
    internal bool DrainDaQiaoXiaoQiao(long id) => _resolve().DrainDaQiaoXiaoQiao(id);
    internal void FinishDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishDaQiaoXiaoQiao(f);
    internal bool HasDaQiaoXiaoQiaoBeforeDamageObserver(long id) => _resolve().HasDaQiaoXiaoQiaoBeforeDamageObserver(id);
    internal bool HasDaQiaoXiaoQiaoDamageObserver(long id) => _resolve().HasDaQiaoXiaoQiaoDamageObserver(id);
    internal bool IsDaQiaoXiaoQiaoLuoyanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsDaQiaoXiaoQiaoLuoyanGrantQualified(owner, grant);
    internal bool IsDaQiaoXiaoQiaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDaQiaoXiaoQiaoRecoveryParent(f, recovery);
    internal void MoveDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DaQiaoXiaoQiaoStage stage, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DaQiaoXiaoQiaoEntity> cards, global::CardGame.Core.CardLocation to, string reason, bool discard) => _resolve().MoveDaQiaoXiaoQiao(f, stage, cards, to, reason, discard);
    internal void PublishDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDaQiaoXiaoQiao(f);
    internal bool ResumeDaQiaoXiaoQiao(long id) => _resolve().ResumeDaQiaoXiaoQiao(id);
    internal void SynchronizeDaQiaoXiaoQiaoLuoyan() => _resolve().SynchronizeDaQiaoXiaoQiaoLuoyan();
    internal bool ValidDaQiaoXiaoQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDaQiaoXiaoQiao(f);
    internal bool ValidDaQiaoXiaoQiaoPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DaQiaoXiaoQiaoReceipt r) => _resolve().ValidDaQiaoXiaoQiaoPayment(f, r);
    internal bool HasDaQiaoXiaoQiaoCapability { get => _resolve().HasDaQiaoXiaoQiaoCapability; }
}
