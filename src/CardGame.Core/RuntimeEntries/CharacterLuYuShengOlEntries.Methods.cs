#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuYuShengOlEntries
{
    internal bool AdvanceLuYuShengSubtree() => _resolve().AdvanceLuYuShengSubtree();
    internal PromptChoice AiLuYuSheng(PendingDecision p, ProgramSkillFrame f) => _resolve().AiLuYuSheng(p, f);
    internal void AnswerLuYuSheng(PromptChoice choice) => _resolve().AnswerLuYuSheng(choice);
    internal void ApplyLuYuShengReduction(ProgramSkillFrame f) => _resolve().ApplyLuYuShengReduction(f);
    internal SkillProgramStepOutcome BeginLuYuSheng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLuYuSheng(supplied, op);
    internal bool CanOfferLuYuSheng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferLuYuSheng(c, t, context);
    internal void ContinueLuYuSheng(long id) => _resolve().ContinueLuYuSheng(id);
    internal bool ContinuePaidLuYuSheng(ProgramSkillFrame f) => _resolve().ContinuePaidLuYuSheng(f);
    internal IReadOnlyList<CardSnapshot> CurrentLuYuShengRevealedCards() => _resolve().CurrentLuYuShengRevealedCards();
    internal bool DrainLuYuSheng(ProgramSkillFrame f) => _resolve().DrainLuYuSheng(f);
    internal void FinishLuYuSheng(ProgramSkillFrame f) => _resolve().FinishLuYuSheng(f);
    internal bool IsLuYuShengIssuedMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsLuYuShengIssuedMovement(f, m);
    internal bool IsLuYuShengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLuYuShengRecoveryParent(f, recovery);
    internal void IssueLuYuShengDraw(ProgramSkillFrame f, int count) => _resolve().IssueLuYuShengDraw(f, count);
    internal void IssueLuYuShengRunwei(ProgramSkillFrame f, int delta) => _resolve().IssueLuYuShengRunwei(f, delta);
    internal int LuYuShengChainCursor(GameEngine.IDamageAttempt attack) => _resolve().LuYuShengChainCursor(attack);
    internal IReadOnlyList<PromptChoice> LuYuShengChoices(ProgramSkillFrame f) => _resolve().LuYuShengChoices(f);
    internal int LuYuShengChooser(ProgramSkillFrame f) => _resolve().LuYuShengChooser(f);
    internal LuYuShengDamageReceipt? LuYuShengDamageState(long id) => _resolve().LuYuShengDamageState(id);
    internal bool LuYuShengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LuYuShengFirstChild(f, child);
    internal bool LuYuShengFirstDamage(int seat) => _resolve().LuYuShengFirstDamage(seat);
    internal ProgramSkillFrame? LuYuShengObserverRoot() => _resolve().LuYuShengObserverRoot();
    internal bool LuYuShengParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().LuYuShengParent(candidate, c);
    internal int LuYuShengRunweiHandLimitBonus(int actorSeat) => _resolve().LuYuShengRunweiHandLimitBonus(actorSeat);
    internal bool LuYuShengSilverLion(ProgramSkillFrame f) => _resolve().LuYuShengSilverLion(f);
    internal CardConversionSource LuYuShengSource(ProgramSkillFrame f) => _resolve().LuYuShengSource(f);
    internal void ObserveLuYuShengDamageTransfer(IGameEvent payload) => _resolve().ObserveLuYuShengDamageTransfer(payload);
    internal void PrepareLuYuShengChainAdvance(GameEngine.IDamageAttempt attack) => _resolve().PrepareLuYuShengChainAdvance(attack);
    internal void PublishLuYuSheng(ProgramSkillFrame f) => _resolve().PublishLuYuSheng(f);
    internal void RecordLuYuShengMovement(long id) => _resolve().RecordLuYuShengMovement(id);
    internal bool ResumeLuYuSheng(long id) => _resolve().ResumeLuYuSheng(id);
    internal bool ReturnLuYuSheng(ProgramSkillFrame f) => _resolve().ReturnLuYuSheng(f);
    internal void SetLuYuShengDamageAmount(GameEngine.IDamageAttempt attack, int amount) => _resolve().SetLuYuShengDamageAmount(attack, amount);
    internal void SetLuYuShengDamageState(long id, LuYuShengDamageReceipt? r) => _resolve().SetLuYuShengDamageState(id, r);
}
