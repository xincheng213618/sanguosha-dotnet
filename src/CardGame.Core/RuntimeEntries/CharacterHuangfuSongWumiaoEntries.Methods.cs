#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangfuSongWumiaoEntries
{
    internal bool AdvanceWuHfChildren() => _resolve().AdvanceWuHfChildren();
    internal PromptChoice AiWuHf(PendingDecision p, ProgramSkillFrame f) => _resolve().AiWuHf(p, f);
    internal void AnswerWuHf(PromptChoice selected) => _resolve().AnswerWuHf(selected);
    internal SkillProgramStepOutcome BeginWuHuangFuSong(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginWuHuangFuSong(supplied, op);
    internal bool CanOfferWuHuangFuSong(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferWuHuangFuSong(candidate, t, c);
    internal CardActionContext CaptureWuHfUseAction(CardActionContext a) => _resolve().CaptureWuHfUseAction(a);
    internal bool ContinuePaidWuHf(ProgramSkillFrame f) => _resolve().ContinuePaidWuHf(f);
    internal void ContinueWuHf(long id) => _resolve().ContinueWuHf(id);
    internal bool DistanceBorrowedPhysicalSlash20HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash20HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DistanceCardUseExemption10HasWuHfOrDixianIssuedUse(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption10HasWuHfOrDixianIssuedUse(q);
    internal bool DistancePhysicalSlash100HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash100HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DistanceVirtualSlash10HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash10HasWuHfOrWuyouMaterialPrivilege(q);
    internal bool DrainWuHf(ProgramSkillFrame f) => _resolve().DrainWuHf(f);
    internal void FinishWuHf(ProgramSkillFrame f) => _resolve().FinishWuHf(f);
    internal bool HasIssuedWuHfUse(long id, int actor) => _resolve().HasIssuedWuHfUse(id, actor);
    internal bool HasWuHfMaterialPrivilege(CharacterState actor, IReadOnlyList<int>? ids) => _resolve().HasWuHfMaterialPrivilege(actor, ids);
    internal bool HasWuHfOrDixianIssuedUse(long id, int actor) => _resolve().HasWuHfOrDixianIssuedUse(id, actor);
    internal bool HasWuHfOrDixianMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, CardConversionSource? source = default) => _resolve().HasWuHfOrDixianMaterialPrivilege(actor, kind, ids, source);
    internal bool HasWuHfOrDixianPotential(int actor) => _resolve().HasWuHfOrDixianPotential(actor);
    internal bool HasWuHfOrWuyouMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, long? useId = default) => _resolve().HasWuHfOrWuyouMaterialPrivilege(actor, kind, ids, useId);
    internal bool HasWuHfPotential(int owner) => _resolve().HasWuHfPotential(owner);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeWuHfEndingItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeWuHfEndingItems(parent, ordinary);
    internal bool IsIssuedWuHfEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedWuHfEnding(c, context);
    internal bool IsWuHfRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame r) => _resolve().IsWuHfRecoveryParent(f, r);
    internal void ObserveWuHfUse(IGameEvent fact) => _resolve().ObserveWuHfUse(fact);
    internal void ProjectWuHuangFuSongCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectWuHuangFuSongCardSkillMark(cardId, ref projection);
    internal void PublishWuHf(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => _resolve().PublishWuHf(f, choices);
    internal bool ResumeWuHf(long id) => _resolve().ResumeWuHf(id);
    internal void ReturnWuHfDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnWuHfDraw(f, o);
    internal bool ReturnWuHfMovement(ProgramSkillFrame f) => _resolve().ReturnWuHfMovement(f);
    internal void SealWuHf(long id, long before) => _resolve().SealWuHf(id, before);
    internal bool ValidWuHf(ProgramSkillFrame f) => _resolve().ValidWuHf(f);
    internal bool WuHfChaozhenAvailable(int owner) => _resolve().WuHfChaozhenAvailable(owner);
    internal IReadOnlyList<PromptChoice> WuHfChoices(ProgramSkillFrame f) => _resolve().WuHfChoices(f);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> WuHfDamageBonuses(GameEngine.IDamageAttempt attack) => _resolve().WuHfDamageBonuses(attack);
    internal string? WuHfEntityMark(int cardId) => _resolve().WuHfEntityMark(cardId);
    internal bool WuHfFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WuHfFirstChild(f, child);
    internal CardUseFrame? WuHfLianjieUse(int owner, ProgramSkillWindowContext c) => _resolve().WuHfLianjieUse(owner, c);
    internal bool WuHfMarkedFor(int owner, int id, WuHuangFuSongTurn turn) => _resolve().WuHfMarkedFor(owner, id, turn);
    internal bool WuHfMaterialSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().WuHfMaterialSlashQuotaAfterLimit(query);
    internal (Card Card, CardLocation From)[] WuHfMinimumPool() => _resolve().WuHfMinimumPool();
    internal ProgramSkillFrame? WuHfObserverRoot() => _resolve().WuHfObserverRoot();
    internal bool WuHfObserverSuffix(int i) => _resolve().WuHfObserverSuffix(i);
    internal WuHuangFuSongJiangxianArmedEvent? WuHfOutstandingArm(int owner) => _resolve().WuHfOutstandingArm(owner);
    internal int WuHfPriorDamageCount(int owner) => _resolve().WuHfPriorDamageCount(owner);
    internal WuHuangFuSongTurn WuHfTurn() => _resolve().WuHfTurn();
    internal bool WuHfTurnOpen(WuHuangFuSongTurn t) => _resolve().WuHfTurnOpen(t);
    internal bool WuHfVictimAvailable(int owner, int target) => _resolve().WuHfVictimAvailable(owner, target);
    internal bool WuHfWuyouSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().WuHfWuyouSlashQuotaBeforeLimit(query);
    internal bool HasWuHuangFuSongCapability { get => _resolve().HasWuHuangFuSongCapability; }
}
