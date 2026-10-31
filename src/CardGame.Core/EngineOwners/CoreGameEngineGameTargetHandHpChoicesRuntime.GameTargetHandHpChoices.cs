#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGameTargetHandHpChoicesRuntime
{
    internal static string GameHandHpUsage(string state, int target) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GameTargetHandHpChoices.GameHandHpUsage(state, target);
    internal bool HasIssuedGameHandHpTarget(int owner, string skill, string state, int target) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>().Any(e => e.OwnerSeat == owner && e.SkillId == skill && e.StateId == state && e.TargetSeat == target);
    internal bool CanSelectGameHandHpTarget(int owner, string skill, string state, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count != _coreMatchParticipantsPlayersAccess._players[target].Hp && !HasIssuedGameHandHpTarget(owner, skill, state, target);
    internal static string GameHandHpReason(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GameTargetHandHpChoices.GameHandHpReason(f);
    internal Card[] GameHandHpDiscardCards(ProgramSkillFrame f) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.GameTargetHandHp!.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(f.GameTargetHandHp.TargetSeat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.GameTargetHandHp.TargetSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && (f.GameTargetHandHp.TargetSeat != f.OwnerSeat || !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c))).ToArray();
    internal SkillProgramStepOutcome BeginGameTargetHandHp(ProgramSkillFrame supplied, string state)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.GameTargetHandHp is not null || f.SelectedTargetSeats is not [var target] || f.TriggerId is not null || !CanSelectGameHandHpTarget(f.OwnerSeat, f.SkillId, state, target))
            throw new InvalidOperationException("One unused game target must have unequal current hand/HP.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count;
        var hp = _coreMatchParticipantsPlayersAccess._players[target].Hp;
        var draw = hand < hp;
        var usage = GameHandHpUsage(state, target);
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(f.OwnerSeat, f.SkillId, usage, SkillUsageScope.Game, 1))
            throw new InvalidOperationException("The exact game-target opportunity was already consumed.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(f.OwnerSeat, f.SkillId, usage, SkillUsageScope.Game, 1));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameTargetHandHp = new(f.InstructionIndex, state, target, hand, hp, draw, 0, "choosing", [], []) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameTargetHandHpChoiceIssuedEvent(f.Id, f.OwnerSeat, f.SkillId, state, target, hand, hp, draw));
        if (draw)
        {
            var before = _corePrivateCardOffersOperations.PrivateOfferSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameTargetHandHp = f.GameTargetHandHp!with { Stage = "children", SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(target, 0, null) });
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[target], 2, true, new(GameHandHpReason(f))).Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GameTargetHandHp = f.GameTargetHandHp!with { SequenceAfter = _corePrivateCardOffersOperations.PrivateOfferSequence, ActualCount = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameTargetHandHpMovementIssuedEvent(f.Id, target, true, actual, before, _corePrivateCardOffersOperations.PrivateOfferSequence));
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnGameTargetHandHpMovement(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var count = Math.Min(2, GameHandHpDiscardCards(f).Length);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameTargetHandHp = f.GameTargetHandHp!with { RequiredCount = count } });
        if (count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishGameHandHpDiscard(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> GameHandHpChoices(ProgramSkillFrame f) => Array.AsReadOnly(GameHandHpDiscardCards(f).Where(c => !f.GameTargetHandHp!.CardIds.Contains(c.Id)).Select(c => new PromptChoice(new($"game-hand-hp.{(f.Id)}.{(c.Id)}"), $"弃置【{(c.DisplayName)}】（{(f.GameTargetHandHp!.CardIds.Count + 1)}/{(f.GameTargetHandHp.RequiredCount)}）", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "game-hand-hp-discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishGameHandHpDiscard(ProgramSkillFrame f)
    {
        var choices = GameHandHpChoices(f);
        var target = f.GameTargetHandHp!.TargetSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, target, $"【{(skill.Name)}】请选择真实弃牌。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = target,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[target].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("game-hand-hp-discard")]
    internal void ResolveGameHandHpDiscard(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No owning game-target choice.");
        if (f.GameTargetHandHp is not { Stage: "choosing", Draw: false } r || !ValidGameHandHpReceipt(f) || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], GameHandHpChoices(f).Where(c => c.Id == choice.Id).ToArray()) || choice.Cards is not [var id])
            throw new InvalidOperationException("The mandatory discard changed its exact target or material.");
        var ids = r.CardIds.Append(id).ToArray();
        var locations = r.Locations.Append(_cardZonesOperations._cardZones.GetLocation(id)).ToArray();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (ids.Length < r.RequiredCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameTargetHandHp = r with { CardIds = ids, Locations = locations } });
            PublishGameHandHpDiscard(f);
            return;
        }

        if (ids.Length != r.RequiredCount || ids.Where((card, n) => _cardZonesOperations._cardZones.GetLocation(card) != locations[n] || !GameHandHpDiscardCards(f).Any(c => c.Id == card)).Any())
            throw new InvalidOperationException("The frozen mandatory discard is no longer completely payable.");
        var before = _corePrivateCardOffersOperations.PrivateOfferSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GameTargetHandHp = r with { Stage = "children", CardIds = ids, Locations = locations, ActualCount = ids.Length, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, ids, new(GameHandHpReason(f)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GameTargetHandHp = f.GameTargetHandHp!with { SequenceAfter = _corePrivateCardOffersOperations.PrivateOfferSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameTargetHandHpMovementIssuedEvent(f.Id, r.TargetSeat, false, ids.Length, before, _corePrivateCardOffersOperations.PrivateOfferSequence));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            ReturnGameTargetHandHpMovement(f);
    }

    internal bool ValidGameHandHpReceipt(ProgramSkillFrame f)
    {
        if (f.GameTargetHandHp is not { } r || r.InstructionIndex != f.InstructionIndex || f.SelectedTargetSeats is not [var target] || target != r.TargetSeat || r.HandCount == r.Hp || r.Draw != (r.HandCount < r.Hp) || r.RequiredCount is < 0 or > 2 || r.CardIds.Count != r.Locations.Count || r.CardIds.Distinct().Count() != r.CardIds.Count || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { Op: SkillProgramEffectOp.ResolveGameTargetHandHpChoice } op || op.StateId != r.StateId)
            return false;
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (issued is not [var fact] || fact != new GameTargetHandHpChoiceIssuedEvent(f.Id, f.OwnerSeat, f.SkillId, r.StateId, target, r.HandCount, r.Hp, r.Draw) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.StateId == r.StateId && e.TargetSeat == target) != 1)
            return false;
        if (r.Stage == "choosing")
            return !r.Draw && r.CardIds.Count < r.RequiredCount && r.SequenceBefore == 0 && r.SequenceAfter == 0 && r.CardIds.Select((id, n) => (id, n)).All(x => _cardZonesOperations._cardZones.GetLocation(x.id) == r.Locations[x.n] && GameHandHpDiscardCards(f).Any(c => c.Id == x.id));
        if (r.Stage != "children")
            return false;
        var movements = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpMovementIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (movements is not [var movement] || movement != new GameTargetHandHpMovementIssuedEvent(f.Id, target, r.Draw, r.ActualCount, r.SequenceBefore, r.SequenceAfter))
            return false;
        var paid = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.Reason.Value == GameHandHpReason(f)).ToArray();
        return r.Draw ? r.CardIds.Count == 0 && r.ActualCount is >= 0 and <= 2 && paid.Length == r.ActualCount && paid.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(target)) : r.CardIds.Count == r.RequiredCount && r.ActualCount == r.RequiredCount && paid.Length == r.RequiredCount && r.CardIds.Select((id, n) => (id, from: r.Locations[n])).All(p => paid.Count(m => m.CardId == p.id && m.From == p.from && p.from.OwnerSeat == target && p.from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(p.id).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)))) == 1);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 290)]
    internal bool IsGameHandHpMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.GameTargetHandHp is { Stage: "children" } r && effect?.Op == SkillProgramEffectOp.ResolveGameTargetHandHpChoice && ValidGameHandHpReceipt(f) && pending.SubjectSeat == r.TargetSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 330)]
    internal bool ReturnGameTargetHandHpMovement(ProgramSkillFrame f)
    {
        if (f.GameTargetHandHp is null)
            return false;
        if (f.PendingMovementContinuation is not { } pending || !IsGameHandHpMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Invalid game-target movement return.");
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, GameTargetHandHp = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 340)]
    internal bool ResumeGameHandHp(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GameTargetHandHp: { } r } f || f.Id != id)
            return false;
        if (!ValidGameHandHpReceipt(f))
            throw new InvalidOperationException("Invalid whole-game target action receipt.");
        if (r.Stage == "choosing")
            return true;
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            ReturnGameTargetHandHpMovement(f);
        return true;
    }
}
