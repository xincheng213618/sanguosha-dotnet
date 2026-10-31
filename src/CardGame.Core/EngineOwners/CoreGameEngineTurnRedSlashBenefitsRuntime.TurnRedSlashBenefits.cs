#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsRuntime
{
    internal bool HasTurnRedSlashCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantTurnRedSlashBenefits) == true;
    internal bool CapturesActionColor => _contentRegistryOperations._contentRegistry.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeActionAppearance) || _characterWenChouMouOlOperations.HasMouWenChouOl || _characterLuZhiMouMobileOperations.Value.HasMouLuZhiCapability || _characterHanDangMouMobileOperations.HasMouHanDangCapability || _characterLuKangWumiaoOperations.HasWuLuKangDixianCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuaXiongYaowu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGongsunZanJiaodi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesOwnPublicPileConversions || _characterLiuYanOlOperations.HasLiuYanCapability || _characterQuHuangOlOperations.Value.HasQuHuangCapability || _characterRuanYuOlOperations.HasRuanYuCapability || _characterLiuPanOlOperations.HasLiuPanCapability || _characterWuJingOlOperations.HasWuJingLiubing || _characterWuJingOlOperations.HasWuJingHeji || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveKenshangTargetRewrite) || _characterMaMiDiOlOperations.HasMaMiDiCapability || _characterLiuZanMobileOperations.TracksAlternateActualUseColor || _coreDamageTargetBudgetOperations.TracksDamageTargetBudgetTargets || _coreAdjacentRedTargetsOperations.HasAdjacentRedTargetCapability || _coreSoleTargetProtectionOperations.Value.HasSoleTargetProtectionCapability || _coreTargetedOrdinaryTrickOperations.HasTargetedOrdinaryTrickAdjustment || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordOpeningTurnUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewriteWeilinTargetHand) || _coreSourceCursesOperations.HasSourceCurseActionColors || _coreActionDiscardColorsOperations.TracksActionDiscardColor || HasTurnRedSlashCapability || _coreDyingWindowSkillPoliciesOperations.HasBlackTrickTargetPolicy || _coreComparedBlackSlashPoliciesOperations.Value.HasComparedBlackSlashPolicies || _coreDirectedCardPolicyQueriesOperations.HasYuanchouCardPolicy || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundDistinctBasicUse || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferPairedColorCardDisposition);

    internal void GrantTurnRedSlashBenefits(ProgramSkillFrame frame)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var change = frame.WindowContext?.HpChange;
        if (change?.Kind != HpChangeKind.Loss || change.TargetSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Red Slash benefits require the owner's actual Loss.");
        if (change.LossOccurrence is not { ActualTurnNumber: > 0, Phase: TurnPhase.Play } occurrence || occurrence.TurnOwnerSeat != frame.OwnerSeat)
            return;
        if (occurrence.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || occurrence.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("A Loss benefit crossed its actual turn.");
        var source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
        var modifier = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, source, SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(modifier));
        var policy = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRedSlashPolicy(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, source);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRedSlashPolicyGrantedEvent(policy));
    }

    internal bool HasTurnRedSlashPolicy(int actor, CardKind kind, Suit? suit) => HasTurnRedSlashPolicyForColor(actor, kind, CoreGameEnginePublicPileColorsRuntime.SuitColor(suit));
    internal bool HasTurnRedSlashPolicyForColor(int actor, CardKind kind, bool? color) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && color == true && _turnCardUseEffectsOperations._turnCardUseEffects.HasRedSlashPolicy(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, actor);
    internal bool? PhysicalGroupColor(CharacterState owner, IReadOnlyList<Card> cards)
    {
        var colors = cards.Select(c => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, c))).Distinct().ToArray();
        return colors.Length == 1 ? colors[0] : null;
    }

    internal bool IsRedSlashProviderPaymentLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false)
    {
        if (!_characterGuanYuWumiaoOperations.IsWuyouFactionPaymentLegal(pending, kind, cards, isTrueZhangba))
            return false;
        if (CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) && _coreFinalTargetSlashOperations.HasRankSlashRange(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat], kind))
            return _coreFinalTargetSlashOperations.IsSpecificRankProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
        if (!CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) || pending.IsAssistedProgramUse || pending.TargetSeat is not { } target || (!_turnCardUseEffectsOperations._turnCardUseEffects.HasRedSlashPolicy(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, pending.OwnerSeat) && !_coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat], kind, Suit.Diamond)))
            return true;
        var actor = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var color = PhysicalGroupColor(actor, cards);
        var suit = _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(actor, kind, Suit.Diamond) ? _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards) : null;
        if (pending.IsBorrowedSwordUse)
            return _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, physicalSuit: suit, allowAnyPhysicalSuit: false, effectiveColor: color);
        if (pending.IsQinglongCrescentBladeUse)
            return _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, color, allowAnyColor: false, physicalSuit: suit);
        return _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], kind, physicalSuit: suit, allowAnyPhysicalSuit: false, effectiveColor: color);
    }

    void ITurnRedSlashProgramHost.GrantTurnRedSlashBenefits(ProgramSkillFrame frame) => GrantTurnRedSlashBenefits(frame);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 180)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 70)]
    internal bool DistancePhysicalSlash180HasTurnRedSlashPolicy(ProgramSlashDistanceQuery q) => HasTurnRedSlashPolicy(q.Actor.Seat, q.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(q.Actor, q.PhysicalCard!));
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 80)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 60)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 120)]
    internal bool DistanceVirtualSlash80HasTurnRedSlashPolicyForColor(ProgramSlashDistanceQuery q) => HasTurnRedSlashPolicyForColor(q.Actor.Seat, q.Kind, q.EffectiveColor ?? CoreGameEnginePublicPileColorsRuntime.SuitColor(q.PhysicalSuit));
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 130)]
    internal bool DistanceBorrowedSlash130HasTurnRedSlashPolicy(ProgramSlashDistanceQuery q) => (q.AllowAnyPhysicalSuit && _coreActionQueriesOperations.GetSlashUseCards(q.Actor).Any(c => HasTurnRedSlashPolicy(q.Actor.Seat, q.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(q.Actor, c))));
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 70)]
    internal bool DistanceProvidedSlash70HasRedSlashPolicy(ProgramSlashDistanceQuery q) => q.AllowAnyPhysicalSuit && _turnCardUseEffectsOperations._turnCardUseEffects.HasRedSlashPolicy(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, q.Actor.Seat);
}
