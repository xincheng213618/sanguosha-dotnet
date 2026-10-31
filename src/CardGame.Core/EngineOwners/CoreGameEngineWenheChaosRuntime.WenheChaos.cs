#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenheChaosRuntime
{
    internal void BeginWenheCardChaos(CardUseFrame use) => BeginWenheChaos(null, use);
    internal void BeginWenheChaos(ProgramSkillFrame? roundParent, CardUseFrame? cardParent)
    {
        if ((roundParent is null) == (cardParent is null) || cardParent is { WenheCardEffect.Stage: not WenheCardEffectStage.AwaitingChaos } || roundParent is { WenheRuleFlow.Stage: not 3 })
            throw new InvalidOperationException("Chaos requires one already issued native parent.");
        var parentId = roundParent?.Id ?? cardParent!.Id;
        var ownerSeat = roundParent?.OwnerSeat ?? cardParent!.SourceSeat;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parentId)
            throw new InvalidOperationException("Chaos cannot change its live parent.");
        var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
        var start = cardParent is null && living.Length > 0 ? living[_randomOperations._random.Next(living.Length)] : _coreTableOrderOperations.TableStepSeat(ownerSeat, 1);
        var actors = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(start, offset)).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && (cardParent is null || seat != ownerSeat)).ToArray();
        const string skillId = "wenhe:chaos-resolution";
        var program = _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program ?? throw new InvalidOperationException("Chaos program is missing.");
        var skillInstance = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
        var child = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, ownerSeat, skillId, "card-effect", program.GameplayHash, 0, [], [])
        {
            SkillInstanceId = skillInstance,
            WenheChaos = new(parentId, cardParent?.Action?.ActionId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberAccess._turnNumber)
            {
                ActorSeats = actors
            }
        };
        _coreRuntimeOperations.PushRuntimeFrame(child);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheChaosIssuedEvent(child.Id, parentId, child.WenheChaos.CardActionId, ownerSeat, skillInstance, program.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberAccess._turnNumber, actors.Length));
        for (var i = 0; i < actors.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheChaosActorEvent(child.Id, i, actors[i]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(child.Id, ownerSeat, skillId, "card-effect"));
        _coreRuntimeOperations.AdvanceRuntimeProgram(child.Id);
    }

    internal bool IsExactWenheChaos(ProgramSkillFrame frame)
    {
        if (frame.WenheChaos is not { } r || frame.SkillId != "wenhe:chaos-resolution" || frame.TriggerId is not null || frame.ActivationId != "card-effect" || frame.SelectedCardIds.Count != 0 || r.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.ActorSeats is not System.Collections.IList { IsReadOnly: true } || r.ActorSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || r.CardActionId is not null && seat == frame.OwnerSeat) || r.ActorSeats.Distinct().Count() != r.ActorSeats.Count || frame.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<WenheChaosIssuedEvent>().Count(e => e.ProgramFrameId == frame.Id && e.ParentFrameId == r.ParentFrameId && e.CardActionId == r.CardActionId && e.OwnerSeat == frame.OwnerSeat && e.SkillInstanceId == frame.SkillInstanceId && e.GameplayHash == frame.GameplayHash && e.RoundNumber == r.RoundNumber && e.TurnNumber == r.TurnNumber && e.ActorCount == r.ActorSeats.Count) != 1 || r.ActorSeats.Where((seat, index) => _coreProgramEventHistoryOperations.ProgramEventHistory<WenheChaosActorEvent>().Count(e => e.ProgramFrameId == frame.Id && e.Index == index && e.Seat == seat) != 1).Any())
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.ParentFrameId)
            return false;
        return r.CardActionId is { } actionId ? _resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame { CardKind: CardKind.WenheChaos, WenheCardEffect.Stage: WenheCardEffectStage.AwaitingChaos, Action: { } action } use && use.SourceSeat == frame.OwnerSeat && action.ActionId == actionId : _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { WenheRuleFlow: { Operation: SkillProgramEffectOp.ResolveWenheRoundStart, Stage: 3 } } parent && parent.OwnerSeat == frame.OwnerSeat;
    }

    internal bool TryCompleteWenheChaosProgram(ProgramSkillFrame frame, bool completed)
    {
        if (frame.WenheChaos is not { } r)
            return false;
        if (!IsExactWenheChaos(frame))
            throw new InvalidOperationException("Chaos lost its exact native return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, completed));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        if (r.CardActionId is not null)
            _coreWenheCardsOperations.CompleteWenheCardChaos(r.ParentFrameId);
        else
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(r.ParentFrameId);
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WenheRuleFlow = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        }

        return true;
    }

    internal bool WenheChaosFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => child is ProgramSkillFrame { WenheChaos: { } r } frame && r.ParentFrameId == parent.Id && IsExactWenheChaos(frame);
}
