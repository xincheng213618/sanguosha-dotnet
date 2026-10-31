#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaMiDiOlRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 10)]
    internal void CollectIssuedBingjieCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || !(CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind)) || _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards) != true || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use)
            return;
        if (use.MaMiDiBingjie is null)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            var declared = Array.FindIndex(history, e => e is CardUseDeclaredEvent d && d.ResolutionId == use.Id);
            if (declared < 0)
                return;
            var actor = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;
            var grants = history.Take(declared).OfType<BingjieGrantedEvent>().Where(g => g.Source.OwnerSeat == actor && g.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat).OrderBy(g => g.FrameId).ToArray();
            if (grants.Length == 0)
                return;
            var r = new MaMiDiBingjieUseReceipt(use.Id, action.ActionId, actor, action.EffectiveKind, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            {
                Grants = grants
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MaMiDiBingjie = r });
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }

        var issued = use.MaMiDiBingjie!;
        if (!_coreMatchParticipantsPlayersAccess._players[issued.ActorSeat].IsAlive)
            return;
        foreach (var grant in issued.Grants.DistinctBy(g => (g.Source.SkillId, g.Source.SkillInstanceId)))
        {
            if (_contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program is not { } p || p.GameplayHash != grant.GameplayHash)
                continue;
            var trigger = p.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards }]);
            if (result.Any(c => c.OwnerSeat == issued.ActorSeat && c.SkillId == grant.Source.SkillId && c.SkillInstanceId == grant.Source.SkillInstanceId && c.TriggerId == trigger.Id))
                continue;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, issued.ActorSeat, issued.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[issued.ActorSeat], action));
            result.Add(new(issued.ActorSeat, issued.ActorSeat, grant.Source.SkillId, trigger.Id, grant.GameplayHash, grant.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 120)]
    internal bool IsIssuedBingjieCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId != "ol:bingjie" || candidate.BindingId != "issued-turn-target-discards" || c is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } card } || c.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { MaMiDiBingjie: { } r, Action: { } action } use || r.UseId != use.Id || r.ActionId != action.ActionId || r.ActorSeat != candidate.OwnerSeat || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || r.Kind != action.EffectiveKind || action.ActorSeat != candidate.OwnerSeat || window.Action.ActorSeat != candidate.OwnerSeat || !(CoreNativeCardClassificationsRuntime.IsSlashCard(r.Kind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(r.Kind)) || card.CardActionId != r.ActionId || card.ParentCardUseFrameId != use.Id || window.Action.ActionId != r.ActionId || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        if (!MaMiDiCandidateMatches(candidate, CoreGameEngineCardActionsRuntime.ToSharedCandidate(current)) || _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) != c || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } p || p.GameplayHash != candidate.GameplayHash || p.Triggers.Single(t => t.Id == candidate.BindingId).Effects is not [{ Op: SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards }])
            return false;
        return r.Grants.Any(g => g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId && g.Source.OwnerSeat == candidate.OwnerSeat && g.GameplayHash == candidate.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BingjieGrantedEvent>().Contains(g) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BingjieUseBoundEvent>().Any(e => e.UseId == use.Id && e.GrantFrameId == g.FrameId));
    }

    internal ProgramSkillWindowContext CreateMaMiDiIssuedContext(ProgramCardTriggerWindowFrame w, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId != "ol:bingjie" || candidate.TriggerId != "issued-turn-target-discards" || CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)?.MaMiDiBingjie is not { } r || r.ActorSeat != candidate.OwnerSeat || r.ActionId != w.Action.ActionId || w.Action.ActorSeat != r.ActorSeat)
            return c;
        var current = _coreCardActionsOperations.CreateCardActionProgramContext(w.Action, SkillProgramTriggerWindow.CardUseTargetsFinalized, w.Id, candidate.OwnerSeat, candidate.OpponentSeat, c.Facts ?? _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], w.Action));
        return current with
        {
            CardUse = current.CardUse!with
            {
                ParentCardUseFrameId = w.ParentFrameId
            }
        };
    }

    internal static bool? MaMiDiActionColor(CardActionContext a) => RuntimeSkills.RuntimeRules_ma_mi_di_ol_MaMiDiBridges.MaMiDiActionColor(a);
    internal MaMiDiResponseLinkedEvent? BuildMaMiDiResponseLink(ProgramCardTriggerWindowFrame w)
    {
        if (w.ResponseCompletion is not { } completion || !_coreResponseCompletionOperations.ValidResponseCompletionParent(w) || w.Action is not { Type: CardActionType.Response } a)
            return null;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId);
        var rootId = parent is NullificationWindowFrame nullification ? nullification.ParentFrameId : w.ParentFrameId;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(rootId)is not { Action: { Type: CardActionType.Use } incoming } use || a.ParentActionId != incoming.ActionId || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, incoming))
            return null;
        var incomingActor = incoming.ActorSeat;
        var incomingId = incoming.ActionId;
        var kind = incoming.EffectiveKind;
        var color = MaMiDiActionColor(incoming);
        if (completion.OriginalContinuation == ProgramCardContinuation.NullificationResponse)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            var at = Array.FindIndex(history, e => e is CardActionAcceptedEvent accepted && accepted.Action.ActionId == a.ActionId);
            if (at < 0)
                return null;
            var previous = history.Take(at).OfType<CardResponseCompletedEvent>().LastOrDefault(e => e.ParentFrameId == w.ParentFrameId && e.OriginalContinuation == ProgramCardContinuation.NullificationResponse);
            if (previous is not null)
            {
                var prior = history.Take(at).OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == previous.ActionId).ToArray();
                if (prior is not [var accepted] || accepted.Action.ParentActionId != incoming.ActionId || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(accepted.Action, ProgramCompletedResponseKind.Nullification) || previous.ActorSeat != accepted.Action.ProviderSeat)
                    return null;
                incomingActor = previous.ActorSeat;
                incomingId = previous.ActionId;
                kind = accepted.Action.EffectiveKind;
                color = MaMiDiActionColor(accepted.Action);
            }
        }

        return new(w.Id, a.ActionId, w.ParentFrameId, incomingId, completion.CompletionActorSeat, a.ActorSeat, a.ProviderSeat, incomingActor, a.EffectiveKind, kind, MaMiDiActionColor(a), color, completion.OriginalContinuation, completion.ActualTurnNumber, completion.ActualTurnOwnerSeat);
    }

    internal void CaptureMaMiDiResponseLink(ProgramCardTriggerWindowFrame w)
    {
        if (!HasMaMiDiCapability)
            return;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MaMiDiResponseLinkedEvent>().Any(e => e.ResponseActionId == w.Action.ActionId))
            throw new InvalidOperationException("Zhengding cannot replace a real response's original card/color pair.");
        if (BuildMaMiDiResponseLink(w)is { } link)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(link);
    }

    internal bool TryGetMaMiDiResponse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, out ProgramCardTriggerWindowFrame window, out MaMiDiResponseLinkedEvent link)
    {
        window = null !;
        link = null !;
        if (candidate.SkillId != "ol:zhengding" || c is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } card } || c.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { ResponseCompletion: { CostsDrained: true } } w || !_coreResponseCompletionOperations.ValidResponseCompletionWindow(w) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !MaMiDiCandidateMatches(candidate, CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex])) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MaMiDiResponseLinkedEvent>().Where(e => e.WindowId == w.Id).ToArray()is not [var frozen] || frozen.ResponseActionId != card.CardActionId || frozen.ResponseActor != candidate.OwnerSeat || frozen.Provider != candidate.OwnerSeat || frozen.NativeActor != w.Action.ActorSeat || frozen.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || frozen.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frozen.ResponseActor == frozen.TurnOwner || frozen.ResponseActor == frozen.IncomingActor || frozen.ResponseIsRed is not { } responseColor || frozen.IncomingIsRed is not { } incomingColor || responseColor != incomingColor || frozen.ResponseKind != w.Action.EffectiveKind || card.ParentCardUseFrameId != w.ParentFrameId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(frozen.IncomingActor))
            return false;
        window = w;
        link = frozen;
        return true;
    }

    internal static bool MaMiDiCandidateMatches(ProgramTriggerCandidate a, ProgramTriggerCandidate b) => RuntimeSkills.RuntimeRules_ma_mi_di_ol_MaMiDiBridges.MaMiDiCandidateMatches(a, b);
}
