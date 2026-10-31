#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMiHengLeEntries
{
    internal bool AdvanceJiguChildren() => _resolve().AdvanceJiguChildren();
    internal void AssertJigu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertJigu(f);
    internal void AssertSiruiUses() => _resolve().AssertSiruiUses();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginJiguDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginJiguDraw(supplied);
    internal void BindSiruiResponseAction(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action) => _resolve().BindSiruiResponseAction(parent, action);
    internal bool CanOfferJigu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferJigu(candidate, trigger, context);
    internal bool CanOfferJiguGate(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferJiguGate(gate);
    internal bool CanUseSiruiConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().CanUseSiruiConversion(owner, card, instance, rule, forResponse, dyingUse, requestedUseTarget);
    internal bool CanUseSiruiPhase(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source) => _resolve().CanUseSiruiPhase(owner, source);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureJiguInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureJiguInitialEntities(window);
    internal void CaptureSiruiMaterialPayment(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureSiruiMaterialPayment(payload);
    internal void ContinueJigu(long id) => _resolve().ContinueJigu(id);
    internal bool DistanceBorrowedPaidWeaponSlash20SiruiPhysicalSlashVariants(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash20SiruiPhysicalSlashVariants(q);
    internal bool DistanceBorrowedPhysicalSlash30IsSiruiPhysicalSlashKind(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash30IsSiruiPhysicalSlashKind(q);
    internal bool DistanceBorrowedSlash40HasSiruiSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash40HasSiruiSlashDistance(q);
    internal bool DistancePhysicalSlash10HasIssuedSiruiUse(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash10HasIssuedSiruiUse(q);
    internal bool DrainJigu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainJigu(f);
    internal bool HasIssuedSiruiUse(long useId, int actorSeat) => _resolve().HasIssuedSiruiUse(useId, actorSeat);
    internal bool HasJiguEntityMark(int cardId) => _resolve().HasJiguEntityMark(cardId);
    internal bool HasJiguEntityMark2(int ownerSeat, int cardId) => _resolve().HasJiguEntityMark(ownerSeat, cardId);
    internal bool HasLeMiHengBeforeDamageObserver(long id) => _resolve().HasLeMiHengBeforeDamageObserver(id);
    internal bool HasLeMiHengDamageObserver(long id) => _resolve().HasLeMiHengDamageObserver(id);
    internal bool HasSiruiForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => _resolve().HasSiruiForcedSlash(actor, target, pending);
    internal bool HasSiruiSlashDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int>? materialIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? existingUseFrameId = default) => _resolve().HasSiruiSlashDistance(actor, kind, materialIds, conversionSource, existingUseFrameId);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeJigu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeJigu(supplied);
    internal bool IsJiguHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsJiguHandLimitExempt(owner, cardId);
    internal bool IsLeMiHengDying() => _resolve().IsLeMiHengDying();
    internal bool IsLeMiHengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLeMiHengMovement(f, effect, p);
    internal bool IsLeMiHengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLeMiHengRecoveryParent(f, recovery);
    internal bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => _resolve().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
    internal bool IsSiruiOutsideResponseMaterial(long parentId, int cardId) => _resolve().IsSiruiOutsideResponseMaterial(parentId, cardId);
    internal bool IsSiruiPhysicalSlashKind(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().IsSiruiPhysicalSlashKind(actor, card, kind);
    internal bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source) => _resolve().IsSiruiSource(source);
    internal bool IsSiruiTrueResponseUse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().IsSiruiTrueResponseUse(owner, kind, dyingUse, requestedUseTarget);
    internal void IssueSiruiReceiptFact(global::CardGame.Core.SiruiUseReceipt r) => _resolve().IssueSiruiReceiptFact(r);
    internal void IssueSiruiUse(global::CardGame.Core.CardUseFrame use) => _resolve().IssueSiruiUse(use);
    internal bool JiguDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord movement) => _resolve().JiguDrawMovement(f, movement);
    internal int JiguEmptySlotCount(global::CardGame.Core.CharacterState owner) => _resolve().JiguEmptySlotCount(owner);
    internal bool JiguFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().JiguFirstChild(f, child);
    internal int JiguMarkedHandCount(int owner) => _resolve().JiguMarkedHandCount(owner);
    internal global::CardGame.Core.ProgramSkillFrame? JiguObserverRoot() => _resolve().JiguObserverRoot();
    internal bool JiguObserverSuffix(int index) => _resolve().JiguObserverSuffix(index);
    internal bool JiguRootMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JiguRootMatches(f);
    internal bool JiguRoundDisabled(int owner, int round) => _resolve().JiguRoundDisabled(owner, round);
    internal int JiguUseCount(int owner, int round) => _resolve().JiguUseCount(owner, round);
    internal bool PermitPaidJigu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitPaidJigu(f);
    internal void PrepareSiruiResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source) => _resolve().PrepareSiruiResponsePayment(provider, card, kind, source);
    internal void ProjectJiguCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectJiguCardSkillMark(cardId, ref projection);
    internal bool ResumeJigu(long id) => _resolve().ResumeJigu(id);
    internal void ReturnJiguDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnJiguDraw(f, outcome);
    internal bool ReturnJiguMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnJiguMovement(f);
    internal bool SiruiAllowsUnspecifiedForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().SiruiAllowsUnspecifiedForcedSlash(actor, card, kind);
    internal bool SiruiBorrowedChoiceAllowed(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, global::CardGame.Core.PromptChoice choice) => _resolve().SiruiBorrowedChoiceAllowed(actor, target, card, choice);
    internal bool SiruiConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().SiruiConversionUnavailable(query);
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? SiruiCurrentPhase(int actor) => _resolve().SiruiCurrentPhase(actor);
    internal bool SiruiExplicitForcedSlashFrontier(global::CardGame.Core.CharacterState actor) => _resolve().SiruiExplicitForcedSlashFrontier(actor);
    internal bool SiruiIgnoresAlcoholLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source) => _resolve().SiruiIgnoresAlcoholLimit(actor, source);
    internal bool SiruiIgnoresSlashDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source) => _resolve().SiruiIgnoresSlashDistance(actor, source);
    internal bool SiruiIgnoresSlashDistanceQuery(global::CardGame.Core.ProgramSlashConversionDistanceQuery q) => _resolve().SiruiIgnoresSlashDistanceQuery(q);
    internal bool SiruiIgnoresSlashLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source) => _resolve().SiruiIgnoresSlashLimit(actor, source);
    internal bool SiruiIgnoresTrickDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<int>? materialIds = default) => _resolve().SiruiIgnoresTrickDistance(actor, source, materialIds);
    internal bool SiruiNativeConversionMatchesOutput(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().SiruiNativeConversionMatchesOutput(actor, source, kind);
    internal global::CardGame.Core.SiruiUseReceipt SiruiNewReceipt(long owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind output, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.SiruiNativeUseKind kind) => _resolve().SiruiNewReceipt(owner, source, output, card, from, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> SiruiPhysicalSlashVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().SiruiPhysicalSlashVariants(actor, card);
    internal bool SiruiPureProviderFrontier(int actor) => _resolve().SiruiPureProviderFrontier(actor);
    internal int? SiruiResponseRank(global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => _resolve().SiruiResponseRank(source, costs);
    internal global::CardGame.Core.CardKind SiruiSelectedSlashKind(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback, global::CardGame.Core.CardConversionSource? explicitSource = default) => _resolve().SiruiSelectedSlashKind(actor, card, fallback, explicitSource);
    internal bool SiruiSlashAliasAllowed(global::CardGame.Core.CharacterState actor, bool forResponse) => _resolve().SiruiSlashAliasAllowed(actor, forResponse);
    internal bool TryGetSiruiDestroyedResponseCost(global::CardGame.Core.CardActionContext action, int cardId, scoped out global::CardGame.Core.CardMovementRecord movement) => _resolve().TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
    internal bool ValidSiruiInvoice(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.SiruiUseReceipt r, bool requirePayment) => _resolve().ValidSiruiInvoice(frame, r, requirePayment);
    internal bool HasLeMiHengJiguCapability { get => _resolve().HasLeMiHengJiguCapability; }
    internal bool HasSiruiCapability { get => _resolve().HasSiruiCapability; }
    internal int JiguStartedSeatCount { get => _resolve().JiguStartedSeatCount; }
}
