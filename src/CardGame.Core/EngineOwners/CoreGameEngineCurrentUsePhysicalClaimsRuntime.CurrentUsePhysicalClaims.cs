#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsRuntime
{
    internal CardUseFrame CurrentFinalTargetSlashOwner(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context, TargetSeat: { } target } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == context.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } owner || action.ActionId != context.CardActionId || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || frame.OwnerSeat != action.ActorSeat || context.ActorSeat != action.ActorSeat || !action.EffectiveDesignatedTargetSeats.Contains(target) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            throw new InvalidOperationException("A current-target operation lost its exact Slash use and target.");
        return owner;
    }

    internal SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame)
    {
        var owner = CurrentFinalTargetSlashOwner(frame);
        var target = frame.WindowContext!.TargetSeat!.Value;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && !_characterPangDeBoundaryMobileOperations.Value.HasBoundaryPangDePaidDiscard(frame))
            return SkillProgramStepOutcome.Continue;
        var effectIndex = frame.InstructionIndex - 1;
        var claims = owner.CurrentUsePhysicalClaims ?? [];
        if (claims.Any(claim => claim.ProducerFrameId == frame.Id && claim.EffectIndex == effectIndex))
            throw new InvalidOperationException("An already claimed use entity cannot be paid again.");
        var ids = owner.Action!.PhysicalCards.Select(cost => cost.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !IsClaimedUseCardEntity(owner.Id, id)).ToArray();
        if (ids.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var receipt = new ProgramCurrentUsePhysicalClaim(owner.Id, owner.Action.ActionId, frame.Id, effectIndex, frame.OwnerSeat, target, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, Array.AsReadOnly(ids));
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { CurrentUsePhysicalClaims = Array.AsReadOnly(claims.Append(receipt).ToArray()) });
        // Issue the owning receipt before movements: a movement observer can
        // inspect or continue the parent use without treating these entities as leaks.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCurrentUsePhysicalCardsClaimedEvent(receipt));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(target), new("skill-program.current-use-physical-claim"));
        return _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    internal bool IsClaimedUseCardEntity(long frameId, int cardId) => _coreSourceDingxiNativeOperations.IsSourceDingxiRetainedEntity(frameId, cardId) || _characterYanYanBoundaryMobileOperations.IsBoundaryYanYanClaimedEntity(frameId, cardId) || _characterCuiLieAnniversaryOperations.IsAnniversaryCuiLieClaimedUseMaterial(frameId, cardId) || _characterHuCheErAnniversaryOperations.IsAnniversaryHuCheErClaimedUseMaterial(frameId, cardId) || _characterDongChengMouOperations.IsMouDcClaimedEntity(frameId, cardId) || _characterChengJiChengCuiOlOperations.IsXumingClaimedProcessingMaterial(frameId, cardId) || _characterXinPiOlOperations.IsXinPiPhysicalClaim(frameId, cardId) || _characterHuaManOlOperations.IsHuaManClaimedEntity(frameId, cardId) || _characterXuRongOlOperations.IsXuRongCausalEntityClaim(frameId, cardId) || _coreResponseEntityExchangeOperations.IsExchangedUseCardClaim(frameId, cardId) || IsCurrentUsePhysicalCardClaim(frameId, cardId) || _coreExtraEffectMaterialClaimsOperations.HasExtraEffectMaterialClaim(frameId, cardId) || _coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedTrickMaterialClaim(frameId, cardId);
    internal bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => _characterZhouCangBoundaryMobileOperations.Value.IsBoundaryZhouCangPhysicalClaim(frameId, cardId) || _characterXiangChongOlOperations.IsXiangChongClaimedUseMaterial(frameId, cardId) || _coreOverflowTargetCancellationOperations.IsOverflowTargetCancellationPhysicalClaim(frameId, cardId) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == frameId)is { Action: { } action, CurrentUsePhysicalClaims: { } claims } && claims.Any(claim => claim.ActionId == action.ActionId && claim.CardUseFrameId == frameId && claim.CardIds.Contains(cardId));
    internal void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame)
    {
        var owner = CurrentFinalTargetSlashOwner(frame);
        var target = frame.WindowContext!.TargetSeat!.Value;
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return;
        var entries = owner.FinalTargetSlashReceipts ?? [];
        var effectIndex = frame.InstructionIndex - 1;
        if (entries.Any(item => item.ProducerFrameId == frame.Id && item.EffectIndex == effectIndex))
            return;
        var receipt = new ProgramTargetSlashReceipt(owner.Action!.ActionId, frame.OwnerSeat, target, owner.Id, frame.Id, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, effectIndex, true, 0);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { FinalTargetSlashReceipts = Array.AsReadOnly(entries.Append(receipt).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTargetSlashReceiptIssuedEvent(owner.Id, receipt));
    }

    SkillProgramStepOutcome ICurrentUsePhysicalClaimProgramHost.ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame) => ClaimCurrentUsePhysicalCards(frame);
    void ICurrentUsePhysicalClaimProgramHost.PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => PreventCurrentTargetSlashCancellationByRule(frame);
}
