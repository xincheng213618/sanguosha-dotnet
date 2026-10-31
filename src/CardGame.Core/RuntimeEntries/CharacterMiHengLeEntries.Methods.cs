#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMiHengLeEntries
{
    internal bool AdvanceJiguChildren() => _resolve().AdvanceJiguChildren();
    internal void AssertSiruiUses() => _resolve().AssertSiruiUses();
    internal SkillProgramStepOutcome BeginJiguDraw(ProgramSkillFrame supplied) => _resolve().BeginJiguDraw(supplied);
    internal void BindSiruiResponseAction(ResolutionFrame parent, CardActionContext action) => _resolve().BindSiruiResponseAction(parent, action);
    internal bool CanOfferJigu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferJigu(candidate, trigger, context);
    internal bool CanOfferJiguGate(ProgramTriggerLegalityContext gate) => _resolve().CanOfferJiguGate(gate);
    internal bool CanUseSiruiConversion(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().CanUseSiruiConversion(owner, card, instance, rule, forResponse, dyingUse, requestedUseTarget);
    internal bool CanUseSiruiPhase(CharacterState owner, CardConversionSource source) => _resolve().CanUseSiruiPhase(owner, source);
    internal ProgramLifecycleTriggerWindowFrame CaptureJiguInitialEntities(ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureJiguInitialEntities(window);
    internal void CaptureSiruiMaterialPayment(IGameEvent payload) => _resolve().CaptureSiruiMaterialPayment(payload);
    internal void ContinueJigu(long id) => _resolve().ContinueJigu(id);
    internal bool DistanceBorrowedPaidWeaponSlash20SiruiPhysicalSlashVariants(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash20SiruiPhysicalSlashVariants(q);
    internal bool DistanceBorrowedPhysicalSlash30IsSiruiPhysicalSlashKind(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash30IsSiruiPhysicalSlashKind(q);
    internal bool DistanceBorrowedSlash40HasSiruiSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash40HasSiruiSlashDistance(q);
    internal bool DistancePhysicalSlash10HasIssuedSiruiUse(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash10HasIssuedSiruiUse(q);
    internal bool DrainJigu(ProgramSkillFrame f) => _resolve().DrainJigu(f);
    internal bool HasIssuedSiruiUse(long useId, int actorSeat) => _resolve().HasIssuedSiruiUse(useId, actorSeat);
    internal bool HasJiguEntityMark(int cardId) => _resolve().HasJiguEntityMark(cardId);
    internal bool HasJiguEntityMark2(int ownerSeat, int cardId) => _resolve().HasJiguEntityMark(ownerSeat, cardId);
    internal bool HasSiruiForcedSlash(CharacterState actor, CharacterState target, NativeOwners.QinglongCrescentBladeHandle pending) => _resolve().HasSiruiForcedSlash(actor, target, pending);
    internal bool HasSiruiSlashDistance(CharacterState actor, CardKind kind, IReadOnlyList<int>? materialIds = default, CardConversionSource? conversionSource = default, long? existingUseFrameId = default) => _resolve().HasSiruiSlashDistance(actor, kind, materialIds, conversionSource, existingUseFrameId);
    internal SkillProgramStepOutcome InitializeJigu(ProgramSkillFrame supplied) => _resolve().InitializeJigu(supplied);
    internal bool IsJiguHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsJiguHandLimitExempt(owner, cardId);
    internal bool IsLeMiHengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLeMiHengRecoveryParent(f, recovery);
    internal bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => _resolve().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
    internal bool IsSiruiOutsideResponseMaterial(long parentId, int cardId) => _resolve().IsSiruiOutsideResponseMaterial(parentId, cardId);
    internal bool IsSiruiPhysicalSlashKind(CharacterState actor, Card card, CardKind kind) => _resolve().IsSiruiPhysicalSlashKind(actor, card, kind);
    internal bool IsSiruiSource(CardConversionSource? source) => _resolve().IsSiruiSource(source);
    internal bool IsSiruiTrueResponseUse(CharacterState owner, CardKind kind, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().IsSiruiTrueResponseUse(owner, kind, dyingUse, requestedUseTarget);
    internal void IssueSiruiReceiptFact(SiruiUseReceipt r) => _resolve().IssueSiruiReceiptFact(r);
    internal void IssueSiruiUse(CardUseFrame use) => _resolve().IssueSiruiUse(use);
    internal bool JiguDrawMovement(ProgramSkillFrame f, CardMovementRecord movement) => _resolve().JiguDrawMovement(f, movement);
    internal int JiguEmptySlotCount(CharacterState owner) => _resolve().JiguEmptySlotCount(owner);
    internal bool JiguFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().JiguFirstChild(f, child);
    internal int JiguMarkedHandCount(int owner) => _resolve().JiguMarkedHandCount(owner);
    internal ProgramSkillFrame? JiguObserverRoot() => _resolve().JiguObserverRoot();
    internal bool JiguObserverSuffix(int index) => _resolve().JiguObserverSuffix(index);
    internal bool JiguRoundDisabled(int owner, int round) => _resolve().JiguRoundDisabled(owner, round);
    internal int JiguUseCount(int owner, int round) => _resolve().JiguUseCount(owner, round);
    internal bool PermitPaidJigu(ProgramSkillFrame f) => _resolve().PermitPaidJigu(f);
    internal void PrepareSiruiResponsePayment(CharacterState provider, Card card, CardKind kind, CardConversionSource? source) => _resolve().PrepareSiruiResponsePayment(provider, card, kind, source);
    internal void ProjectJiguCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectJiguCardSkillMark(cardId, ref projection);
    internal bool ResumeJigu(long id) => _resolve().ResumeJigu(id);
    internal void ReturnJiguDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnJiguDraw(f, outcome);
    internal bool ReturnJiguMovement(ProgramSkillFrame f) => _resolve().ReturnJiguMovement(f);
    internal bool SiruiAllowsUnspecifiedForcedSlash(CharacterState actor, Card card, CardKind kind) => _resolve().SiruiAllowsUnspecifiedForcedSlash(actor, card, kind);
    internal bool SiruiBorrowedChoiceAllowed(CharacterState actor, CharacterState target, Card card, PromptChoice choice) => _resolve().SiruiBorrowedChoiceAllowed(actor, target, card, choice);
    internal bool SiruiConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().SiruiConversionUnavailable(query);
    internal ActualDiscardRecoveryPhaseKey? SiruiCurrentPhase(int actor) => _resolve().SiruiCurrentPhase(actor);
    internal bool SiruiExplicitForcedSlashFrontier(CharacterState actor) => _resolve().SiruiExplicitForcedSlashFrontier(actor);
    internal bool SiruiIgnoresAlcoholLimit(CharacterState actor, CardConversionSource? source) => _resolve().SiruiIgnoresAlcoholLimit(actor, source);
    internal bool SiruiIgnoresSlashDistance(CharacterState actor, CardConversionSource? source) => _resolve().SiruiIgnoresSlashDistance(actor, source);
    internal bool SiruiIgnoresSlashDistanceQuery(ProgramSlashConversionDistanceQuery q) => _resolve().SiruiIgnoresSlashDistanceQuery(q);
    internal bool SiruiIgnoresSlashLimit(CharacterState actor, CardConversionSource? source) => _resolve().SiruiIgnoresSlashLimit(actor, source);
    internal bool SiruiIgnoresTrickDistance(CharacterState actor, CardConversionSource? source, IReadOnlyList<int>? materialIds = default) => _resolve().SiruiIgnoresTrickDistance(actor, source, materialIds);
    internal bool SiruiNativeConversionMatchesOutput(CharacterState actor, CardConversionSource source, CardKind kind) => _resolve().SiruiNativeConversionMatchesOutput(actor, source, kind);
    internal SiruiUseReceipt SiruiNewReceipt(long owner, CardConversionSource source, CardKind output, Card card, CardLocation from, SiruiNativeUseKind kind) => _resolve().SiruiNewReceipt(owner, source, output, card, from, kind);
    internal IReadOnlyList<SlashUseVariant> SiruiPhysicalSlashVariants(CharacterState actor, Card card) => _resolve().SiruiPhysicalSlashVariants(actor, card);
    internal bool SiruiPureProviderFrontier(int actor) => _resolve().SiruiPureProviderFrontier(actor);
    internal int? SiruiResponseRank(CardConversionSource? source, IReadOnlyList<CardActionCost> costs) => _resolve().SiruiResponseRank(source, costs);
    internal CardKind SiruiSelectedSlashKind(CharacterState actor, Card card, CardKind fallback, CardConversionSource? explicitSource = default) => _resolve().SiruiSelectedSlashKind(actor, card, fallback, explicitSource);
    internal bool SiruiSlashAliasAllowed(CharacterState actor, bool forResponse) => _resolve().SiruiSlashAliasAllowed(actor, forResponse);
    internal bool TryGetSiruiDestroyedResponseCost(CardActionContext action, int cardId, scoped out CardMovementRecord movement) => _resolve().TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
    internal bool ValidSiruiInvoice(ResolutionFrame frame, SiruiUseReceipt r, bool requirePayment) => _resolve().ValidSiruiInvoice(frame, r, requirePayment);
    internal bool HasLeMiHengJiguCapability { get => _resolve().HasLeMiHengJiguCapability; }
    internal bool HasSiruiCapability { get => _resolve().HasSiruiCapability; }
    internal int JiguStartedSeatCount { get => _resolve().JiguStartedSeatCount; }
}
