#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuYuShengOlEntries
{
    internal bool AdvanceLuYuShengSubtree() => _resolve().AdvanceLuYuShengSubtree();
    internal global::CardGame.Core.PromptChoice AiLuYuSheng(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiLuYuSheng(p, f);
    internal void AnswerLuYuSheng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerLuYuSheng(choice);
    internal void ApplyLuYuShengReduction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyLuYuShengReduction(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLuYuSheng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLuYuSheng(supplied, op);
    internal bool CanOfferLuYuSheng(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLuYuSheng(c, t, context);
    internal void ContinueLuYuSheng(long id) => _resolve().ContinueLuYuSheng(id);
    internal bool ContinuePaidLuYuSheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidLuYuSheng(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> CurrentLuYuShengRevealedCards() => _resolve().CurrentLuYuShengRevealedCards();
    internal bool DrainLuYuSheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLuYuSheng(f);
    internal void FinishLuYuSheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLuYuSheng(f);
    internal bool IsLuYuShengIssuedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsLuYuShengIssuedMovement(f, m);
    internal bool IsLuYuShengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLuYuShengRecoveryParent(f, recovery);
    internal void IssueLuYuShengDraw(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().IssueLuYuShengDraw(f, count);
    internal void IssueLuYuShengRunwei(global::CardGame.Core.ProgramSkillFrame f, int delta) => _resolve().IssueLuYuShengRunwei(f, delta);
    internal int LuYuShengChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().LuYuShengChainCursor(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LuYuShengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LuYuShengChoices(f);
    internal int LuYuShengChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LuYuShengChooser(f);
    internal global::CardGame.Core.LuYuShengDamageReceipt? LuYuShengDamageState(long id) => _resolve().LuYuShengDamageState(id);
    internal bool LuYuShengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LuYuShengFirstChild(f, child);
    internal bool LuYuShengFirstDamage(int seat) => _resolve().LuYuShengFirstDamage(seat);
    internal global::CardGame.Core.ProgramSkillFrame? LuYuShengObserverRoot() => _resolve().LuYuShengObserverRoot();
    internal bool LuYuShengParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().LuYuShengParent(candidate, c);
    internal int LuYuShengRunweiHandLimitBonus(int actorSeat) => _resolve().LuYuShengRunweiHandLimitBonus(actorSeat);
    internal bool LuYuShengSilverLion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LuYuShengSilverLion(f);
    internal global::CardGame.Core.CardConversionSource LuYuShengSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LuYuShengSource(f);
    internal void ObserveLuYuShengDamageTransfer(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLuYuShengDamageTransfer(payload);
    internal void PrepareLuYuShengChainAdvance(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().PrepareLuYuShengChainAdvance(attack);
    internal void PublishLuYuSheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLuYuSheng(f);
    internal void RecordLuYuShengMovement(long id) => _resolve().RecordLuYuShengMovement(id);
    internal bool ResumeLuYuSheng(long id) => _resolve().ResumeLuYuSheng(id);
    internal bool ReturnLuYuSheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLuYuSheng(f);
    internal void SetLuYuShengDamageAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().SetLuYuShengDamageAmount(attack, amount);
    internal void SetLuYuShengDamageState(long id, global::CardGame.Core.LuYuShengDamageReceipt? r) => _resolve().SetLuYuShengDamageState(id, r);
}
