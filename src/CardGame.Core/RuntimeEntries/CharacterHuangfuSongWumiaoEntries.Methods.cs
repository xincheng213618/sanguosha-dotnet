#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangfuSongWumiaoEntries
{
    internal bool AdvanceWuHfChildren() => _resolve().AdvanceWuHfChildren();
    internal global::CardGame.Core.PromptChoice AiWuHf(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiWuHf(p, f);
    internal void AnswerWuHf(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerWuHf(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWuHuangFuSong(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWuHuangFuSong(supplied, op);
    internal bool CanOfferWuHuangFuSong(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferWuHuangFuSong(candidate, t, c);
    internal global::CardGame.Core.CardActionContext CaptureWuHfUseAction(global::CardGame.Core.CardActionContext a) => _resolve().CaptureWuHfUseAction(a);
    internal bool ContinuePaidWuHf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWuHf(f);
    internal void ContinueWuHf(long id) => _resolve().ContinueWuHf(id);
    internal bool DistanceBorrowedPhysicalSlash20HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash20HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DistanceCardUseExemption10HasWuHfOrDixianIssuedUse(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption10HasWuHfOrDixianIssuedUse(q);
    internal bool DistancePhysicalSlash100HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash100HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DistanceVirtualSlash10HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash10HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DrainWuHf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainWuHf(f);
    internal void FinishWuHf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWuHf(f);
    internal bool HasIssuedWuHfUse(long id, int actor) => _resolve().HasIssuedWuHfUse(id, actor);
    internal bool HasWuHfMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int>? ids) => _resolve().HasWuHfMaterialPrivilege(actor, ids);
    internal bool HasWuHfOrDixianIssuedUse(long id, int actor) => _resolve().HasWuHfOrDixianIssuedUse(id, actor);
    internal bool HasWuHfOrDixianMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.CardConversionSource? source = default) => _resolve().HasWuHfOrDixianMaterialPrivilege(actor, kind, ids, source);
    internal bool HasWuHfOrDixianPotential(int actor) => _resolve().HasWuHfOrDixianPotential(actor);
    internal bool HasWuHfOrWuyouMaterialPrivilege(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? ids, long? useId = default) => _resolve().HasWuHfOrWuyouMaterialPrivilege(actor, kind, ids, useId);
    internal bool HasWuHfPotential(int owner) => _resolve().HasWuHfPotential(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeWuHfEndingItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeWuHfEndingItems(parent, ordinary);
    internal bool IsIssuedWuHfEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedWuHfEnding(c, context);
    internal bool IsWuHfRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame r) => _resolve().IsWuHfRecoveryParent(f, r);
    internal void ObserveWuHfUse(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveWuHfUse(fact);
    internal void ProjectWuHuangFuSongCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectWuHuangFuSongCardSkillMark(cardId, ref projection);
    internal void PublishWuHf(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishWuHf(f, choices);
    internal bool ResumeWuHf(long id) => _resolve().ResumeWuHf(id);
    internal void ReturnWuHfDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnWuHfDraw(f, o);
    internal bool ReturnWuHfMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWuHfMovement(f);
    internal void SealWuHf(long id, long before) => _resolve().SealWuHf(id, before);
    internal bool ValidWuHf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWuHf(f);
    internal bool WuHfChaozhenAvailable(int owner) => _resolve().WuHfChaozhenAvailable(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WuHfChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WuHfChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> WuHfDamageBonuses(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WuHfDamageBonuses(attack);
    internal string? WuHfEntityMark(int cardId) => _resolve().WuHfEntityMark(cardId);
    internal bool WuHfFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WuHfFirstChild(f, child);
    internal global::CardGame.Core.CardUseFrame? WuHfLianjieUse(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().WuHfLianjieUse(owner, c);
    internal bool WuHfMarkedFor(int owner, int id, global::CardGame.Core.WuHuangFuSongTurn turn) => _resolve().WuHfMarkedFor(owner, id, turn);
    internal bool WuHfMaterialSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().WuHfMaterialSlashQuotaAfterLimit(query);
    internal (global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)[] WuHfMinimumPool() => _resolve().WuHfMinimumPool();
    internal global::CardGame.Core.ProgramSkillFrame? WuHfObserverRoot() => _resolve().WuHfObserverRoot();
    internal bool WuHfObserverSuffix(int i) => _resolve().WuHfObserverSuffix(i);
    internal global::CardGame.Core.WuHuangFuSongJiangxianArmedEvent? WuHfOutstandingArm(int owner) => _resolve().WuHfOutstandingArm(owner);
    internal int WuHfPriorDamageCount(int owner) => _resolve().WuHfPriorDamageCount(owner);
    internal global::CardGame.Core.WuHuangFuSongTurn WuHfTurn() => _resolve().WuHfTurn();
    internal bool WuHfTurnOpen(global::CardGame.Core.WuHuangFuSongTurn t) => _resolve().WuHfTurnOpen(t);
    internal bool WuHfVictimAvailable(int owner, int target) => _resolve().WuHfVictimAvailable(owner, target);
    internal bool WuHfWuyouSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().WuHfWuyouSlashQuotaBeforeLimit(query);
    internal bool HasWuHuangFuSongCapability { get => _resolve().HasWuHuangFuSongCapability; }
}
