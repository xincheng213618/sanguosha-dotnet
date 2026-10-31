#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFenAnniversaryEntries
{
    internal void AnswerZhangFen(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerZhangFen(selected);
    internal void AppendDagongCheIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => _resolve().AppendDagongCheIssuedDamageCandidates(attack, window, result);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangFen(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginZhangFen(supplied, op);
    internal bool CanOfferZhangFen(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferZhangFen(c, t, w);
    internal bool CompleteDagongCheAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteDagongCheAttack(attack, out paused);
    internal bool ContinueZhangFen(long id) => _resolve().ContinueZhangFen(id);
    internal string? DagongCheUpgradeLabel(int cardId) => _resolve().DagongCheUpgradeLabel(cardId);
    internal int[] DagongLegalTargets(int owner, bool ignoreDistance) => _resolve().DagongLegalTargets(owner, ignoreDistance);
    internal global::CardGame.Core.DagongCheUpgradedEvent[] DagongUpgrades(int id) => _resolve().DagongUpgrades(id);
    internal bool DrainZhangFen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainZhangFen(f);
    internal bool ExactDagongCheUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactDagongCheUse(use, f);
    internal void FinishZhangFen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhangFen(f);
    internal bool HasBoundDagongCheDiscardOrigin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasBoundDagongCheDiscardOrigin(f);
    internal global::CardGame.Core.Card? HeldDagongChe(int owner) => _resolve().HeldDagongChe(owner);
    internal bool IsDagongCheDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => _resolve().IsDagongCheDestroyedMaterial(m);
    internal bool IsDagongCheDestroyedMaterial2(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().IsDagongCheDestroyedMaterial(cardId, from, to);
    internal bool IsDagongCheDestroyedMaterial3(long useId, int cardId) => _resolve().IsDagongCheDestroyedMaterial(useId, cardId);
    internal bool IsIssuedDagongCheDamageCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedDagongCheDamageCandidate(c, w);
    internal bool IsRealDagongChe(int id) => _resolve().IsRealDagongChe(id);
    internal bool IsUnbegunIssuedDagongCheDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsUnbegunIssuedDagongCheDamage(f);
    internal bool IsUnupgradedDagongCheDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => _resolve().IsUnupgradedDagongCheDiscardForbidden(actor, card, from, intent);
    internal void IssueDagongCheSlash(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IssueDagongCheSlash(f, targets);
    internal bool KeepPaidZhangFen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidZhangFen(f);
    internal global::CardGame.Core.CardLocation NormalizeDagongCheDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().NormalizeDagongCheDestination(card, from, to);
    internal void ObserveDagongCheMovement(global::CardGame.Core.CardMovedEvent moved) => _resolve().ObserveDagongCheMovement(moved);
    internal void PublishZhangFen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangFen(f);
    internal bool ReturnDagongCheAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnDagongCheAttack(completion);
    internal void ReturnZhangFenDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnZhangFenDraw(supplied, draw);
    internal bool ReturnZhangFenMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangFenMovement(f);
    internal bool ValidZhangFen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangFen(f);
    internal void ValidateDagongCheUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateDagongCheUseReturn(use);
    internal global::CardGame.Core.PromptChoice ZhangFenAi(global::CardGame.Core.PendingDecision p) => _resolve().ZhangFenAi(p);
    internal global::CardGame.Core.PromptChoice ZhangFenChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, global::System.Collections.Generic.IReadOnlyDictionary<string, string>? extra = default) => _resolve().ZhangFenChoice(f, option, label, cards, targets, extra);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangFenChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangFenChoices(f);
    internal int ZhangFenDestroyedCount(int owner, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ZhangFenDestroyedCount(owner, w);
    internal bool ZhangFenMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ZhangFenMovementProof(f, effect, p);
    internal bool ZhangFenSlashDamage(int owner, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ZhangFenSlashDamage(owner, w);
    internal bool HasDagongCheCapability { get => _resolve().HasDagongCheCapability; }
}
