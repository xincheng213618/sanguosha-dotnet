#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiYangAnniversaryEntries
{
    internal bool AdmitCaiYangXunji(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AdmitCaiYangXunji(c, w);
    internal bool AdvanceCaiYangObserver() => _resolve().AdvanceCaiYangObserver();
    internal void AppendCaiYangEnding(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendCaiYangEnding(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaiYang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginCaiYang(supplied, operation);
    internal int CaiYangChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().CaiYangChainCursor(a);
    internal global::CardGame.Core.CaiYangXunjiOrigin? CaiYangDuelDamage(int owner, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CaiYangDuelDamage(owner, w);
    internal global::CardGame.Core.CaiYangXunjiGrant? CaiYangEndingWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w, bool begun = false) => _resolve().CaiYangEndingWitness(c, w, begun);
    internal bool CaiYangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaiYangFirstChild(f, child);
    internal bool CaiYangFirstDamage(int owner) => _resolve().CaiYangFirstDamage(owner);
    internal global::CardGame.Core.CaiYangJiaofengOrigin? CaiYangFirstWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CaiYangFirstWitness(c, w);
    internal int CaiYangJiaofengDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().CaiYangJiaofengDamageBonus(a);
    internal global::CardGame.Core.ProgramSkillFrame? CaiYangObserverRoot() => _resolve().CaiYangObserverRoot();
    internal bool CanOfferCaiYang(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferCaiYang(c, t, w);
    internal void ContinueCaiYang(long id) => _resolve().ContinueCaiYang(id);
    internal bool DrainCaiYang(long id) => _resolve().DrainCaiYang(id);
    internal void ExpireCaiYangXunji(global::CardGame.Core.TurnEndedEvent t) => _resolve().ExpireCaiYangXunji(t);
    internal void FinishCaiYang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaiYang(f);
    internal bool HasCaiYangObserverDying() => _resolve().HasCaiYangObserverDying();
    internal bool KeepCaiYangIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepCaiYangIssued(f);
    internal bool ResumeAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame p, global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().ResumeAnniversaryCaiYangJiaofengSourceWindow(p, a);
    internal bool ResumeCaiYang(long id) => _resolve().ResumeCaiYang(id);
    internal void ReturnCaiYangDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnCaiYangDraw(supplied, draw);
    internal bool ReturnCaiYangDuel(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnCaiYangDuel(completion);
    internal bool ReturnCaiYangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCaiYangMovement(f);
    internal void StartCaiYangNextTurn(global::CardGame.Core.TurnStartedEvent t) => _resolve().StartCaiYangNextTurn(t);
    internal void TrackCaiYangBlackUse(global::CardGame.Core.CardUseAppearanceCapturedEvent e) => _resolve().TrackCaiYangBlackUse(e);
    internal bool TryBeginAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().TryBeginAnniversaryCaiYangJiaofengSourceWindow(a);
    internal bool ValidCaiYangDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin native) => _resolve().ValidCaiYangDuelUse(use, native);
    internal bool ValidCaiYangFirstOrigin(global::CardGame.Core.CaiYangJiaofengOrigin r) => _resolve().ValidCaiYangFirstOrigin(r);
    internal bool ValidCaiYangGrant(global::CardGame.Core.CaiYangXunjiGrant g) => _resolve().ValidCaiYangGrant(g);
    internal bool HasAnniversaryCaiYang { get => _resolve().HasAnniversaryCaiYang; }
}
