#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoXiaoQiaoOlEntries
{
    internal bool AdvanceDaQiaoXiaoQiaoChildren() => _resolve().AdvanceDaQiaoXiaoQiaoChildren();
    internal void AnswerDaQiaoXiaoQiao(PromptChoice choice) => _resolve().AnswerDaQiaoXiaoQiao(choice);
    internal SkillProgramStepOutcome BeginDaQiaoXiaoQiao(ProgramSkillFrame input, bool maintain) => _resolve().BeginDaQiaoXiaoQiao(input, maintain);
    internal long CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(CharacterState owner, long existingStamp) => _resolve().CaptureDaQiaoXiaoQiaoLuoyanQualificationStamp(owner, existingStamp);
    internal PromptChoice ChooseDaQiaoXiaoQiaoAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseDaQiaoXiaoQiaoAi(p, f);
    internal void ContinueDaQiaoXiaoQiao(long id) => _resolve().ContinueDaQiaoXiaoQiao(id);
    internal bool ContinuePaidDaQiaoXiaoQiao(ProgramSkillFrame f) => _resolve().ContinuePaidDaQiaoXiaoQiao(f);
    internal IReadOnlyList<PromptChoice> DaQiaoXiaoQiaoChoices(ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoChoices(f);
    internal bool DaQiaoXiaoQiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().DaQiaoXiaoQiaoFirstChild(f, child);
    internal bool DaQiaoXiaoQiaoHasPile(int seat) => _resolve().DaQiaoXiaoQiaoHasPile(seat);
    internal bool DaQiaoXiaoQiaoLive(ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoLive(f);
    internal bool DaQiaoXiaoQiaoLuoyanSourceLive(CharacterState owner, SkillGrant source) => _resolve().DaQiaoXiaoQiaoLuoyanSourceLive(owner, source);
    internal bool DaQiaoXiaoQiaoMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().DaQiaoXiaoQiaoMovementMatches(f, m);
    internal ProgramSkillFrame? DaQiaoXiaoQiaoObserverRoot() => _resolve().DaQiaoXiaoQiaoObserverRoot();
    internal IReadOnlyList<DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoOwned(ProgramSkillFrame f, bool handOnly, bool discard) => _resolve().DaQiaoXiaoQiaoOwned(f, handOnly, discard);
    internal bool DaQiaoXiaoQiaoParent(ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoParent(f);
    internal IReadOnlyList<DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoPileCards(ProgramSkillFrame f) => _resolve().DaQiaoXiaoQiaoPileCards(f);
    internal bool DrainDaQiaoXiaoQiao(long id) => _resolve().DrainDaQiaoXiaoQiao(id);
    internal void FinishDaQiaoXiaoQiao(ProgramSkillFrame f) => _resolve().FinishDaQiaoXiaoQiao(f);
    internal bool IsDaQiaoXiaoQiaoLuoyanGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsDaQiaoXiaoQiaoLuoyanGrantQualified(owner, grant);
    internal bool IsDaQiaoXiaoQiaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsDaQiaoXiaoQiaoRecoveryParent(f, recovery);
    internal void MoveDaQiaoXiaoQiao(ProgramSkillFrame f, DaQiaoXiaoQiaoStage stage, IReadOnlyList<DaQiaoXiaoQiaoEntity> cards, CardLocation to, string reason, bool discard) => _resolve().MoveDaQiaoXiaoQiao(f, stage, cards, to, reason, discard);
    internal void PublishDaQiaoXiaoQiao(ProgramSkillFrame f) => _resolve().PublishDaQiaoXiaoQiao(f);
    internal bool ResumeDaQiaoXiaoQiao(long id) => _resolve().ResumeDaQiaoXiaoQiao(id);
    internal void SynchronizeDaQiaoXiaoQiaoLuoyan() => _resolve().SynchronizeDaQiaoXiaoQiaoLuoyan();
    internal bool ValidDaQiaoXiaoQiao(ProgramSkillFrame f) => _resolve().ValidDaQiaoXiaoQiao(f);
    internal bool ValidDaQiaoXiaoQiaoPayment(ProgramSkillFrame f, DaQiaoXiaoQiaoReceipt r) => _resolve().ValidDaQiaoXiaoQiaoPayment(f, r);
    internal bool HasDaQiaoXiaoQiaoCapability { get => _resolve().HasDaQiaoXiaoQiaoCapability; }
}
