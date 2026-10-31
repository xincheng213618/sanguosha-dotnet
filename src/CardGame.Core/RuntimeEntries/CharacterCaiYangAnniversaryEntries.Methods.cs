#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiYangAnniversaryEntries
{
    internal bool AdmitCaiYangXunji(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().AdmitCaiYangXunji(c, w);
    internal bool AdvanceCaiYangObserver() => _resolve().AdvanceCaiYangObserver();
    internal void AppendCaiYangEnding(List<TurnEndingBoundaryItem> items) => _resolve().AppendCaiYangEnding(items);
    internal SkillProgramStepOutcome BeginCaiYang(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginCaiYang(supplied, operation);
    internal int CaiYangChainCursor(GameEngine.IDamageAttempt a) => _resolve().CaiYangChainCursor(a);
    internal CaiYangXunjiOrigin? CaiYangDuelDamage(int owner, ProgramSkillWindowContext w) => _resolve().CaiYangDuelDamage(owner, w);
    internal CaiYangXunjiGrant? CaiYangEndingWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool begun = false) => _resolve().CaiYangEndingWitness(c, w, begun);
    internal bool CaiYangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaiYangFirstChild(f, child);
    internal bool CaiYangFirstDamage(int owner) => _resolve().CaiYangFirstDamage(owner);
    internal CaiYangJiaofengOrigin? CaiYangFirstWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().CaiYangFirstWitness(c, w);
    internal int CaiYangJiaofengDamageBonus(GameEngine.IDamageAttempt a) => _resolve().CaiYangJiaofengDamageBonus(a);
    internal ProgramSkillFrame? CaiYangObserverRoot() => _resolve().CaiYangObserverRoot();
    internal bool CanOfferCaiYang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferCaiYang(c, t, w);
    internal void ContinueCaiYang(long id) => _resolve().ContinueCaiYang(id);
    internal bool DrainCaiYang(long id) => _resolve().DrainCaiYang(id);
    internal void ExpireCaiYangXunji(TurnEndedEvent t) => _resolve().ExpireCaiYangXunji(t);
    internal void FinishCaiYang(ProgramSkillFrame f) => _resolve().FinishCaiYang(f);
    internal bool HasCaiYangObserverDying() => _resolve().HasCaiYangObserverDying();
    internal bool KeepCaiYangIssued(ProgramSkillFrame f) => _resolve().KeepCaiYangIssued(f);
    internal bool ResumeAnniversaryCaiYangJiaofengSourceWindow(BeforeDamageProgramWindowFrame p, GameEngine.IDamageAttempt a) => _resolve().ResumeAnniversaryCaiYangJiaofengSourceWindow(p, a);
    internal bool ResumeCaiYang(long id) => _resolve().ResumeCaiYang(id);
    internal void ReturnCaiYangDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnCaiYangDraw(supplied, draw);
    internal bool ReturnCaiYangDuel(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnCaiYangDuel(completion);
    internal bool ReturnCaiYangMovement(ProgramSkillFrame f) => _resolve().ReturnCaiYangMovement(f);
    internal void StartCaiYangNextTurn(TurnStartedEvent t) => _resolve().StartCaiYangNextTurn(t);
    internal void TrackCaiYangBlackUse(CardUseAppearanceCapturedEvent e) => _resolve().TrackCaiYangBlackUse(e);
    internal bool TryBeginAnniversaryCaiYangJiaofengSourceWindow(GameEngine.IDamageAttempt a) => _resolve().TryBeginAnniversaryCaiYangJiaofengSourceWindow(a);
    internal bool ValidCaiYangDuelUse(CardUseFrame use, ProgramSelectedActorDuelOrigin native) => _resolve().ValidCaiYangDuelUse(use, native);
    internal bool ValidCaiYangFirstOrigin(CaiYangJiaofengOrigin r) => _resolve().ValidCaiYangFirstOrigin(r);
    internal bool ValidCaiYangGrant(CaiYangXunjiGrant g) => _resolve().ValidCaiYangGrant(g);
    internal bool HasAnniversaryCaiYang { get => _resolve().HasAnniversaryCaiYang; }
}
