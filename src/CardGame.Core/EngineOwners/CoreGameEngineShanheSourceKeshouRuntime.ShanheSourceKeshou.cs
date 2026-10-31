#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceKeshouRuntime
{
    internal IEnumerable<SourceKeshouMaterial> SourceKeshouEligibleMaterials(int owner)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[owner];
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(player).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(player)))
        {
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            if (CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(player, card))is { } red && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, from, OwnedCardMoveIntent.Discard))
                yield return new(card.Id, from, red);
        }
    }

    internal bool HasSourceKeshouPair(int owner)
    {
        var red = 0;
        var black = 0;
        foreach (var material in SourceKeshouEligibleMaterials(owner))
            if (material.IsRed ? ++red >= 2 : ++black >= 2)
                return true;
        return false;
    }

    internal IReadOnlyList<SourceKeshouMaterial[]> SourceKeshouPairs(int owner)
    {
        var cards = SourceKeshouEligibleMaterials(owner).ToArray();
        var pairs = new List<SourceKeshouMaterial[]>();
        for (var a = 0; a < cards.Length; a++)
            for (var b = a + 1; b < cards.Length; b++)
                if (cards[a].IsRed == cards[b].IsRed)
                    pairs.Add([cards[a], cards[b]]);
        return pairs;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62061, SkillProgramEffectOp.ResolveShanheSourceKeshou)]
    internal bool CanOfferSourceKeshou(ProgramTriggerLegalityContext c) => c.SkillId == SourceKeshouContract.SkillId && c.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } window && window.TargetSeat == c.OwnerSeat && c.Owner.IsAlive && HasSourceKeshouPair(c.OwnerSeat);
    SkillProgramStepOutcome ISourceKeshouHost.Resolve(ProgramSkillFrame f, string step) => ResolveSourceKeshou(f, step);
    internal SkillProgramStepOutcome ResolveSourceKeshou(ProgramSkillFrame supplied, string step)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (step == "select-cost")
        {
            if (f.SourceKeshou is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame w || w.Id != context.ParentFrameId || w.TargetSeat != f.OwnerSeat || w.Prevented || w.Amount <= 0 || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].Candidate) || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !HasSourceKeshouPair(f.OwnerSeat))
                throw new InvalidOperationException("Source993 requires its original positive damage candidate and an unpaid same-colour pair.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceKeshou = new(w.Id, w.ContinuationAttackResolutionId ?? w.ParentFrameId, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, w.Amount, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
            var choices = SourceKeshouChoices(f);
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "弃置两张颜色相同的手牌或装备牌，令此伤害-1。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (step != "after-cost" || !ValidSourceKeshou(f) || !SourceKeshouWasPaid(f) || f.SourceKeshou!.SoleFactionAfterCost is not null || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramDamageReducedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == f.OwnerSeat && e.AmountBefore == f.SourceKeshou.OriginalAmount && e.AmountAfter == Math.Max(0, e.AmountBefore - 1)))
            throw new InvalidOperationException("Source993's faction branch requires its actual pair payment and original native reduction.");
        var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
        var sole = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && faction is not null && !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == faction);
        var receipt = f.SourceKeshou with
        {
            SoleFactionAfterCost = sole
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceKeshou = receipt, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("sole-faction", sole ? "sole" : "shared", f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceKeshouPaidBranchEvent(f.Id, receipt));
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<PromptChoice> SourceKeshouChoices(ProgramSkillFrame f) => Array.AsReadOnly(SourceKeshouPairs(f.OwnerSeat).Select(pair => new PromptChoice(new($"source-keshou.{(f.Id)}.{(pair[0].CardId)}-{(pair[1].CardId)}"), string.Join("、", pair.Select(m => _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)), pair.Select(m => m.CardId).ToArray(), [], new Dictionary<string, string> { ["program-action"] = "source-keshou-cost", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    [ProgramAnswer("source-keshou-cost")]
    internal void AnswerSourceKeshou(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Source993 lost its selection frame.");
        if (!ValidSourceKeshou(f) || f.SourceKeshou!.Materials.Count != 0 || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !SourceKeshouChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Source993 must choose its exact published original same-colour pair.");
        var pair = selected.Cards.Select(id => SourceKeshouEligibleMaterials(f.OwnerSeat).Single(m => m.CardId == id)).ToArray();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceKeshou = f.SourceKeshou with { Materials = pair } });
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "keshou-cost", pair.Select(m => m.CardId).ToArray(), SkillProgramCardSetVisibility.Private, pair.Select(m => m.From).ToArray(), selectionActorSeat: f.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceKeshou)]
    internal PromptChoice SelectAiSourceKeshou(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => c.Cards.Sum(id => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    internal bool SourceKeshouWasPaid(ProgramSkillFrame f) => f.SourceKeshou is { Materials.Count: 2 } r && r.Materials.All(material => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.CardId == material.CardId && m.From == material.From && m.Reason == CoreGameEngineShanheSourceKeshouDefinitions.SourceKeshouCostReason && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)))));
    internal bool ValidSourceKeshou(ProgramSkillFrame f)
    {
        if (f.SourceKeshou is not { } r || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || f.WindowContext?.ParentFrameId != r.WindowId || r.OriginalAmount <= 0 || r.SequenceBefore < 0 || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Materials.Count is not (0 or 2) || r.Materials.Count == 2 && (r.Materials.Select(m => m.CardId).Distinct().Count() != 2 || r.Materials[0].IsRed != r.Materials[1].IsRed || r.Materials.Any(m => m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is BeforeDamageProgramWindowFrame w && w.Id == r.WindowId && w.TargetSeat == f.OwnerSeat && (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == r.AttackId;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62061)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62061)]
    internal bool KeepPaidSourceKeshou(ProgramSkillFrame f) => ValidSourceKeshou(f) && SourceKeshouWasPaid(f);
}
