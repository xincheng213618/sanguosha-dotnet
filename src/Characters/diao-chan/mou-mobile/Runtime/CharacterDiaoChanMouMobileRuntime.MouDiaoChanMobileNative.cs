#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanMouMobileRuntime
{
    internal void IssueMouDiaoChanDuel(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.MouDiaoChanMobile!;
        while (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && r.Index < r.Targets.Count)
        {
            var actor = r.Targets[r.Index];
            var candidates = r.Targets.Where(s => s != actor && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(s, actor)).ToArray();
            if (_coreMatchParticipantsPlayersAccess._players[actor].IsAlive && candidates is [var target, ..] && _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor))
            {
                var ring = new MouDiaoChanMobileRingOrigin(f.Id, r.Index, actor, target, r.Source, r.GameplayHash);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileDuelPlannedEvent(ring));
                var origin = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, actor, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber)
                {
                    InitialTargetSeat = target,
                    MouDiaoChanRing = ring
                };
                var card = new Card(0, CardKind.Duel, Suit.None, 0);
                var id = _coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: origin);
                var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouDiaoChanMobile = r with { UseId = id } });
                var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileDuelIssuedEvent(ring, id, use.Action!.ActionId));
                _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
                return;
            }

            r = r with
            {
                Index = r.Index + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r });
        }

        FinishMouDiaoChan(f);
    }

    [NativeUseReturn(31700)]
    internal bool ReturnMouDiaoChanRingUse(CardUseFrame use)
    {
        if (use.SelectedActorDuelOrigin?.MouDiaoChanRing is not { } ring || use.SelectedActorDuelOrigin.AttackStarted)
            return false;
        CompleteMouDiaoChanRingDuel(ring.FrameId, ring.Index, use.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 31700)]
    internal bool ReturnMouDiaoChanRingAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDiaoChanMobile: { Stage: MouDiaoChanMobileStage.Duels } r } f || r.UseId != completion.ResolutionId)
            return false;
        CompleteMouDiaoChanRingDuel(f.Id, r.Index, r.UseId);
        return true;
    }

    internal void CompleteMouDiaoChanRingDuel(long frameId, int index, long useId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = f.MouDiaoChanMobile!;
        if (!ValidMouDiaoChan(f) || !r.Paid || r.Stage != MouDiaoChanMobileStage.Duels || r.UseId != useId || r.Index != index || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileDuelIssuedEvent>().Count(e => e.UseId == useId && e.Origin.FrameId == frameId && e.Origin.Index == index && e.Origin.Source == r.Source) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardId == 0 && e.CardKind == CardKind.Duel) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileDuelReturnedEvent>().Any(e => e.FrameId == frameId && e.Index == index))
            throw new InvalidOperationException("Lijian cannot repeat a completed Duel or borrow another use's return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileDuelReturnedEvent(frameId, index, useId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Index = r.Index + 1, UseId = 0 } });
        IssueMouDiaoChanDuel(f);
    }
}
