#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal bool HasMouLuXun => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordMouLuXunQianxun);

    internal static CardConversionSource MouLuXunSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal CardKind[] MouLuXunRecordedNames(int owner)
    {
        var result = new HashSet<CardKind>();
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (fact is MouLuXunNameRecordedEvent recorded && recorded.OwnerSeat == owner)
                result.Add(recorded.Kind);
            if (fact is MouLuXunNameRemovedEvent removed && removed.OwnerSeat == owner)
                result.Remove(removed.Kind);
        }

        return result.Order().ToArray();
    }

    internal int MouLuXunLostCount(int owner, int turn) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.TurnNumber == turn && m.From.OwnerSeat == owner && m.From != m.To && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32800, SkillProgramEffectOp.RecordMouLuXunQianxun, SkillProgramEffectOp.UseMouLuXunRecordedTrick, SkillProgramEffectOp.DistributeMouLuXunLianying)]
    internal bool CanOfferMouLuXun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return false;
        if (trigger.Id == "record-and-hold")
            return context.Window == SkillProgramTriggerWindow.CardEffectBeforeApply && context.CardUse is { } card && CardCatalog.Get(card.EffectiveKind).CategoryName == "锦囊牌" && _resolutionStackOperations._resolutionStack.OfType<CardEffectBeforeApplyFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && !MouLuXunRecordedNames(candidate.OwnerSeat).Contains(card.EffectiveKind) && MouLuXunEffectUser(window) != candidate.OwnerSeat;
        if (trigger.Id == "use-recorded-trick")
            return context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && context.SourceSeat == candidate.OwnerSeat && MouLuXunRecordedNames(candidate.OwnerSeat).Length > 0;
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.SourceSeat != candidate.OwnerSeat && MouLuXunLostCount(candidate.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) > 0;
    }

    internal int? MouLuXunEffectUser(CardEffectBeforeApplyFrame window)
    {
        if (window.DelayedReturn is not { } delayed)
            return window.Action.ActorSeat;
        var declared = _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().LastOrDefault(e => e.CardId == delayed.CardId);
        if (declared is null)
            return null; // Native placement without a Use has no invented actor.
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().LastOrDefault(e => e.CardUseFrameId == declared.ResolutionId)?.ActorSeat ?? declared.SourceSeat;
    }

    internal bool MouLuXunParentMatches(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 1 || f.WindowContext is not { } context)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        if (parent.Id != context.ParentFrameId)
            return false;
        if (parent is CardEffectBeforeApplyFrame effect)
            return effect.CandidateIndex < effect.Candidates.Count && effect.Candidates[effect.CandidateIndex] is { } c && c.FrozenContext == context && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(c));
        if (parent is PlayPhaseStartingBoundaryFrame starting)
            return starting.OwnerSeat == f.OwnerSeat && starting.ItemIndex < starting.Items.Count && starting.Items[starting.ItemIndex].Candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
        return parent is DeferredTurnEndFrame ending && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(ending, f, context);
    }

    internal bool ValidMouLuXun(ProgramSkillFrame f) => f.MouLuXun is { } r && !string.IsNullOrWhiteSpace(r.GrantSourceId) && r.Source == MouLuXunSource(f) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && f.WindowContext?.ParentFrameId == r.WindowId && r.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && MouLuXunParentMatches(f) && f.TriggerId == f.ActivationId && f.InstructionIndex == 1 && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuXunStartedEvent>().Count(e => e == new MouLuXunStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.WindowId, r.ActualTurn, r.TurnOwner, r.Limit, r.GrantSourceId)) == 1;
    internal SkillProgramStepOutcome BeginMouLuXun(ProgramSkillFrame input, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.MouLuXun is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || !CanOfferMouLuXun(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Mobile Lu Xun requires its actual unpaid effect, Play-start or other-turn ending candidate.");
        var hold = operation == SkillProgramEffectOp.RecordMouLuXunQianxun;
        var distribute = operation == SkillProgramEffectOp.DistributeMouLuXunLianying;
        var limit = hold ? Math.Min(5, MouLuXunRecordedNames(f.OwnerSeat).Length + 1) : distribute ? Math.Min(5, MouLuXunLostCount(f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)) : 0;
        var r = new MouLuXunReceipt(MouLuXunSource(f), f.GameplayHash, operation, c.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, hold ? MouLuXunStage.HoldChoice : distribute ? MouLuXunStage.ViewChildren : MouLuXunStage.NameChoice, limit)
        {
            GrantSourceId = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Single(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId).SourceId
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunStartedEvent(f.Id, r.Source, r.GameplayHash, operation, r.WindowId, r.ActualTurn, r.TurnOwner, limit, r.GrantSourceId));
        if (hold)
        {
            var kind = c.CardUse!.EffectiveKind;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunNameRecordedEvent(f.Id, f.OwnerSeat, kind, c.ParentFrameId, c.CardUse.CardActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, MouLuXunRecordedNames(f.OwnerSeat).Length + 1));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Paid = true, Kind = kind } });
        }

        if (distribute)
        {
            BeginMouLuXunView(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMouLuXun(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouLuXunChoices(ProgramSkillFrame f)
    {
        var r = f.MouLuXun!;
        var choices = new List<PromptChoice>();
        void Add(string verb, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string? value = null) => choices.Add(new(new($"mou-lu-xun.{(f.Id)}.{(r.Stage)}.{(r.Cursor)}.{(verb)}.{(value)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "mou-lu-xun", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["verb"] = verb, ["value"] = value ?? "" }));
        if (r.Stage == MouLuXunStage.HoldChoice)
        {
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])))
                if (r.Selected.Contains(card.Id) || r.Selected.Count < r.Limit)
                    Add("toggle", (r.Selected.Contains(card.Id) ? "取消" : "暂存") + card.DisplayName, [card.Id]);
            Add("confirm", r.Selected.Count == 0 ? "不暂存牌" : "确认暂存");
        }
        else if (r.Stage == MouLuXunStage.NameChoice)
            foreach (var kind in MouLuXunRecordedNames(f.OwnerSeat))
                Add("name", "移去记录【" + CardCatalog.Get(kind).DisplayName + "】", value: kind.ToString());
        else if (r.Stage == MouLuXunStage.UseChoice)
        {
            var options = MouLuXunUseOptions(f);
            for (var i = 0; i < options.Count; i++)
                Add("use", options[i].Description, targets: options[i].TargetSeats, value: i.ToString(CultureInfo.InvariantCulture));
            Add("decline", "不使用此牌");
        }
        else if (r.Stage == MouLuXunStage.Distribution && r.Cursor < r.Viewed.Count)
        {
            var cardId = r.Viewed[r.Cursor];
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                Add("give", "将此牌交给" + player.Name, [cardId], [player.Seat]);
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouLuXun(ProgramSkillFrame f)
    {
        var choices = MouLuXunChoices(f);
        if (choices.Count == 0)
        {
            FinishMouLuXun(f);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, "请选择【" + _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name + "】的合法选项。", f.OwnerSeat);
    }

    [ProgramAnswer("mou-lu-xun")]
    internal void AnswerMouLuXun(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuXun: { } r } f || !ValidMouLuXun(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouLuXunChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lu Xun accepts only its exact current physical entities, record and ordered native target plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var verb = choice.Parameters["verb"];
        if (r.Stage == MouLuXunStage.HoldChoice)
        {
            if (verb == "toggle")
            {
                var id = choice.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray() } });
                PublishMouLuXun(f);
                return;
            }

            if (r.Selected.Count == 0)
            {
                FinishMouLuXun(f);
                return;
            }

            PayMouLuXunHold(f);
            return;
        }

        if (r.Stage == MouLuXunStage.NameChoice)
        {
            var kind = Enum.Parse<CardKind>(choice.Parameters["value"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunNameRemovedEvent(f.Id, f.OwnerSeat, kind, r.ActualTurn));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Kind = kind, Paid = true, Stage = MouLuXunStage.UseChoice } });
            if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) || MouLuXunUseOptions(f).Count == 0)
            {
                FinishMouLuXun(f);
                return;
            }

            PublishMouLuXun(f);
            return;
        }

        if (r.Stage == MouLuXunStage.UseChoice)
        {
            if (verb == "decline")
            {
                FinishMouLuXun(f);
                return;
            }

            IssueMouLuXunUse(f, int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture));
            return;
        }

        if (r.Stage == MouLuXunStage.Distribution)
        {
            PayMouLuXunGift(f, choice.Targets.Single());
            return;
        }

        throw new InvalidOperationException("Lu Xun's native child cannot accept another selection.");
    }

    internal bool DrainMouLuXun(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishMouLuXun(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouLuXunHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginMouLuXun(f, operation);
}
