#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFenAnniversaryEntries
{
    internal void AnswerZhangFen(PromptChoice selected) => _resolve().AnswerZhangFen(selected);
    internal void AppendDagongCheIssuedDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> result) => _resolve().AppendDagongCheIssuedDamageCandidates(attack, window, result);
    internal SkillProgramStepOutcome BeginZhangFen(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginZhangFen(supplied, op);
    internal bool CanOfferZhangFen(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferZhangFen(c, t, w);
    internal bool CompleteDagongCheAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteDagongCheAttack(attack, out paused);
    internal bool ContinueZhangFen(long id) => _resolve().ContinueZhangFen(id);
    internal string? DagongCheUpgradeLabel(int cardId) => _resolve().DagongCheUpgradeLabel(cardId);
    internal int[] DagongLegalTargets(int owner, bool ignoreDistance) => _resolve().DagongLegalTargets(owner, ignoreDistance);
    internal DagongCheUpgradedEvent[] DagongUpgrades(int id) => _resolve().DagongUpgrades(id);
    internal bool DrainZhangFen(ProgramSkillFrame f) => _resolve().DrainZhangFen(f);
    internal bool ExactDagongCheUse(CardUseFrame use, ProgramSkillFrame f) => _resolve().ExactDagongCheUse(use, f);
    internal void FinishZhangFen(ProgramSkillFrame f) => _resolve().FinishZhangFen(f);
    internal bool HasBoundDagongCheDiscardOrigin(ProgramSkillFrame f) => _resolve().HasBoundDagongCheDiscardOrigin(f);
    internal Card? HeldDagongChe(int owner) => _resolve().HeldDagongChe(owner);
    internal bool IsDagongCheDestroyedMaterial(CardMovementRecord m) => _resolve().IsDagongCheDestroyedMaterial(m);
    internal bool IsDagongCheDestroyedMaterial2(int cardId, CardLocation from, CardLocation to) => _resolve().IsDagongCheDestroyedMaterial(cardId, from, to);
    internal bool IsDagongCheDestroyedMaterial3(long useId, int cardId) => _resolve().IsDagongCheDestroyedMaterial(useId, cardId);
    internal bool IsIssuedDagongCheDamageCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedDagongCheDamageCandidate(c, w);
    internal bool IsRealDagongChe(int id) => _resolve().IsRealDagongChe(id);
    internal bool IsUnbegunIssuedDagongCheDamage(ProgramSkillFrame f) => _resolve().IsUnbegunIssuedDagongCheDamage(f);
    internal bool IsUnupgradedDagongCheDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent) => _resolve().IsUnupgradedDagongCheDiscardForbidden(actor, card, from, intent);
    internal void IssueDagongCheSlash(ProgramSkillFrame f, IReadOnlyList<int> targets) => _resolve().IssueDagongCheSlash(f, targets);
    internal bool KeepPaidZhangFen(ProgramSkillFrame f) => _resolve().KeepPaidZhangFen(f);
    internal CardLocation NormalizeDagongCheDestination(Card card, CardLocation from, CardLocation to) => _resolve().NormalizeDagongCheDestination(card, from, to);
    internal void ObserveDagongCheMovement(CardMovedEvent moved) => _resolve().ObserveDagongCheMovement(moved);
    internal void PublishZhangFen(ProgramSkillFrame f) => _resolve().PublishZhangFen(f);
    internal bool ReturnDagongCheAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnDagongCheAttack(completion);
    internal void ReturnZhangFenDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnZhangFenDraw(supplied, draw);
    internal bool ReturnZhangFenMovement(ProgramSkillFrame f) => _resolve().ReturnZhangFenMovement(f);
    internal bool ValidZhangFen(ProgramSkillFrame f) => _resolve().ValidZhangFen(f);
    internal void ValidateDagongCheUseReturn(CardUseFrame use) => _resolve().ValidateDagongCheUseReturn(use);
    internal PromptChoice ZhangFenAi(PendingDecision p) => _resolve().ZhangFenAi(p);
    internal PromptChoice ZhangFenChoice(ProgramSkillFrame f, string option, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default, IReadOnlyDictionary<string, string>? extra = default) => _resolve().ZhangFenChoice(f, option, label, cards, targets, extra);
    internal IReadOnlyList<PromptChoice> ZhangFenChoices(ProgramSkillFrame f) => _resolve().ZhangFenChoices(f);
    internal int ZhangFenDestroyedCount(int owner, ProgramSkillWindowContext w) => _resolve().ZhangFenDestroyedCount(owner, w);
    internal bool ZhangFenSlashDamage(int owner, ProgramSkillWindowContext w) => _resolve().ZhangFenSlashDamage(owner, w);
    internal bool HasDagongCheCapability { get => _resolve().HasDagongCheCapability; }
}
