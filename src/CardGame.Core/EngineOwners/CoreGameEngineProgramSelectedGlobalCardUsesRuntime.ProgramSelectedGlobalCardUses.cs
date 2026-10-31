#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectedGlobalCardUsesRuntime
{
    internal SkillProgramStepOutcome UseProgramSelectedCardsAsGlobal(ProgramSkillFrame frame, ProgramMultiCardViewAsSelection selection)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (active.TriggerId is not null || active.SelectedTargetSeats.Count != 0 && !_coreNextActualUseTargetAdjustmentsOperations.HasExactNextActualUseProgramSelection(active, ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!)) || selection.OutputKind != CardKind.ArrowBarrage || selection.Cards.Count != 2 || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)is { SameSuit: true } rule && selection.Cards.Select(card => rule.UseEffectiveInputSuit == true ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit).Distinct().Count() != 1 || !_coreDirectedCardPolicyQueriesOperations.CanUseGlobalCard(owner, CardKind.ArrowBarrage) || !selection.Cards.All(card => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(held => held.Id == card.Id)))
            throw new InvalidOperationException("A global selected-card use requires two legal same-suit owner hand cards.");
        var targets = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(owner.Seat, offset)]).Where(player => player.IsAlive && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.ArrowBarrage, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, player.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards))).Select(player => player.Seat).ToArray();
        var adjusted = _coreNextActualUseTargetAdjustmentsOperations.ApplyNextActualUseGlobalRemoval(active, targets, out var changedTargets);
        targets = changedTargets.ToArray();
        var representative = selection.Cards[0];
        var physicalIds = selection.Cards.Select(card => card.Id).ToArray();
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(representative, owner.Seat, targets, CardKind.ArrowBarrage, physicalCardIds: physicalIds, conversionSource: selection.Source);
        if (adjusted)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(resolutionId, use => use with { TargetsAdjusted = true });
        _corePhysicalMovementFacadesOperations.MoveCards(selection.Cards, CardLocation.Hand(owner.Seat), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(resolutionId, frame.SkillId, selection.Source.BindingId, owner.Seat, Array.AsReadOnly(physicalIds), CardKind.ArrowBarrage, IsUse: true, targets));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, representative, owner.Seat, targets, LegalActionKind.ArrowBarrage, requiredCardKind: CardKind.Dodge, playedCardKind: CardKind.ArrowBarrage);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
