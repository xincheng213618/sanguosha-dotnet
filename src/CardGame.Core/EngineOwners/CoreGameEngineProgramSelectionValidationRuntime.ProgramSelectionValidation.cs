#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionValidationRuntime
{
    internal ProgramSelectionValidationRegistry ProgramSelectionValidators => _programSelectionValidatorsOperations._programSelectionValidators ??= new(CoreGameEngineProgramSelectionValidationDefinitions.ProgramSelectionValidationMetadata.Value, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);

    bool IMatchingRecastSelectionQuery.IsValid(int ownerSeat, IReadOnlyList<int> cards) => _coreMatchingRecastsOperations.ValidMatchingRecastSelection(ownerSeat, cards);
    bool IOwnedRankSumSelectionQuery.CanStart(int ownerSeat, ProgramExecutionPlan plan) => _coreOwnedHandRankSumProgramsOperations.CanStartOwnedHandRankSum(_coreMatchParticipantsPlayersAccess._players[ownerSeat], plan);
    bool IExactOwnedCountSelectionQuery.CanStart(int ownerSeat, ProgramExecutionPlan plan) => _coreOwnedExactCountAndNamedTargetsOperations.CanStartExactOwnedCardCount(_coreMatchParticipantsPlayersAccess._players[ownerSeat], plan);
    bool IEquipmentDonationSelectionQuery.CanPay(int ownerSeat, string skillId, string stateId) => _coreEquipmentDonationProgramsOperations.HasPayableAllEquipmentDonation(ownerSeat, skillId, stateId);
    bool IPairObtainSelectionQuery.CanSelect(int ownerSeat, string skillId, SkillProgramActivation activation, IReadOnlyList<int> targets) => _corePairObtainShownGiftsOperations.CanActivatePairObtain(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId), activation, targets);
    bool IDeferredHandPairSelectionQuery.CanSelect(int ownerSeat, int first, int second) => _coreHalfHandPhaseDebtProgramsOperations.IsPayableDeferredHandPair(ownerSeat, first, second);
    bool IForeignSlashSelectionQuery.CanSelect(int ownerSeat, int targetSeat) => _coreForeignSelectedCardSlashOperations.CanStartSelectedForeignCardSlash(_coreMatchParticipantsPlayersAccess._players[ownerSeat], targetSeat);
    bool ISelectedActorDuelSelectionQuery.CanSelect(int ownerSeat, int targetSeat) => _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(ownerSeat, targetSeat);
    bool IEquipmentPlacementSelectionQuery.CanPlace(int ownerSeat, int targetSeat, int cardId) => _coreEquipmentPlacementAndHpPairOperations.CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    bool ICapturedEquipmentPlacementSelectionQuery.CanPlace(int ownerSeat, int targetSeat, int cardId, string skillId) => _coreCapturedEquipmentDrawsOperations.CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
    bool IBlackGiftSelectionQuery.WasIssued(int ownerSeat, string skillId) => _coreRecipientConsequencesReceiptsOperations.BlackGiftGameIssued(ownerSeat, skillId);
    bool IDynamicDiscardDamageSelectionQuery.CanStart(int ownerSeat, string skillId) => _coreDynamicDiscardDamagePaymentsOperations.CanStartDynamicDiscardDamage(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    bool IDynamicDiscardDamageSelectionQuery.CanTarget(int ownerSeat, string skillId, int targetSeat) => _coreDynamicDiscardDamagePaymentsOperations.CanSelectDynamicDiscardDamageTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId, targetSeat);
    bool IGameHandHpSelectionQuery.CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => _coreGameTargetHandHpChoicesOperations.CanSelectGameHandHpTarget(ownerSeat, skillId, stateId, targetSeat);
    bool ICategoryRefundSelectionQuery.CanTarget(int ownerSeat, string skillId, string stateId, int targetSeat) => _coreDrawDiscardCategoryRefundOperations.DrawDiscardCategoryTargetAvailable(ownerSeat, skillId, stateId, targetSeat);
    bool INextActualUseSelectionQuery.HasHpLossTargets(int ownerSeat) => _coreHpLossSlashMaterialsOperations.HasHpLossSlashTargets(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
    SkillProgramEffect? INextActualUseSelectionQuery.HpLossProducer(ProgramExecutionPlan plan) => CoreGameEngineHpLossSlashMaterialsRuntime.HpLossSlashProgramProducer(plan);
    int INextActualUseSelectionQuery.HpLossMaximum(int ownerSeat, CardKind kind) => _coreHpLossSlashMaterialsOperations.HpLossSlashMaximum(_coreMatchParticipantsPlayersAccess._players[ownerSeat], kind);
    SkillProgramEffect? INextActualUseSelectionQuery.SelectedProducer(ProgramExecutionPlan plan) => CoreGameEngineNextActualUseTargetAdjustmentsRuntime.NextActualUseSelectedProducer(plan);
    string INextActualUseSelectionQuery.SkillInstance(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    ProgramMultiCardViewAsSelection? INextActualUseSelectionQuery.FindSelection(int ownerSeat, IReadOnlyList<int> cards, CardKind kind, CardConversionSource source) => ProgramSelectionProjection.Freeze(_coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[ownerSeat], cards, kind, false, source));
    bool INextActualUseSelectionQuery.HasAdjustment(int ownerSeat) => _coreNextActualUseTargetAdjustmentsOperations.CurrentNextActualUseAdjustment(ownerSeat)is not null;
    bool INextActualUseSelectionQuery.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool INextActualUseSelectionQuery.CanUseSlash(int ownerSeat, int targetSeat, ProgramMultiCardViewAsSelection selection) => _coreNextActualUseTargetAdjustmentsOperations.CanUseNextActualUseSelectedSlash(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], selection);
    IReadOnlyList<int> INextActualUseSelectionQuery.Targets(int ownerSeat, ProgramExecutionPlan plan, ProgramMultiCardViewAsSelection selection) => Array.AsReadOnly(AdjustedUseSelection.SelectTargets(ownerSeat, plan, [], [selection], _coreProgramSelectionModulesOperations.ProgramSelectionQueries.Get<IProgramSelectionSourceQuery>()).ToArray());
    bool IProvenanceSelectionQuery.TracksProvenance => _coreDiscardedProvenanceOperations.TracksDiscardedEntityProvenance;

    bool IProvenanceSelectionQuery.IsFaceDown(int ownerSeat) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsFaceDown;
    bool IProvenanceSelectionQuery.HasClaimedEntityPolicy(int ownerSeat) => _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.ClaimedEntitiesFaceDownUse).Any();
    string IProvenanceSelectionQuery.SkillInstance(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    ProgramMultiCardViewAsSelection? IProvenanceSelectionQuery.FindSelection(int ownerSeat, IReadOnlyList<int> cards, CardKind kind, CardConversionSource source) => ProgramSelectionProjection.Freeze(_coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(_coreMatchParticipantsPlayersAccess._players[ownerSeat], cards, kind, false, source));
    bool IProvenanceSelectionQuery.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProvenanceSelectionQuery.CanUseSlash(int ownerSeat, int targetSeat, ProgramMultiCardViewAsSelection selection) => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], physicalSuit: _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], selection), effectiveColor: _coreGuanHaiBridgesOperations.ProgramMultiCardColor(_coreMatchParticipantsPlayersAccess._players[ownerSeat], selection), physicalCardIds: selection.Cards.Select(card => card.Id).ToArray());
    string IWholeHandSuitSelectionQuery.SkillInstance(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    bool IWholeHandSuitSelectionQuery.IsValid(int ownerSeat, IReadOnlyList<int> cards, CardConversionSource source) => _coreFriendTrioOperations.ValidWholeHandSuitConversion(_coreMatchParticipantsPlayersAccess._players[ownerSeat], cards.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray(), source);
    bool IDistinctFactionSelectionQuery.CanPay(int ownerSeat, string skillId) => _coreDistinctFactionDiscardsOperations.HasPayableDistinctFactionOwnerHe(ownerSeat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId));
}
