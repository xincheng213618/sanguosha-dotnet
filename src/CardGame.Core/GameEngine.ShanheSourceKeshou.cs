using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason SourceKeshouCostReason = new("skill-program.shanhe:source-keshou-993.MoveBoundCards");

    private IEnumerable<SourceKeshouMaterial> SourceKeshouEligibleMaterials(int owner)
    {
        var player = _players[owner];
        foreach (var card in GetHand(player).Concat(GetEquipment(player)))
        {
            var from = _cardZones.GetLocation(card.Id);
            if (SuitColor(EffectiveSuit(player, card)) is { } red &&
                !IsForeignEquipmentDiscardPrevented(owner, card, from, OwnedCardMoveIntent.Discard))
                yield return new(card.Id, from, red);
        }
    }
    private bool HasSourceKeshouPair(int owner)
    {
        var red = 0; var black = 0;
        foreach (var material in SourceKeshouEligibleMaterials(owner))
            if (material.IsRed ? ++red >= 2 : ++black >= 2) return true;
        return false;
    }
    private IReadOnlyList<SourceKeshouMaterial[]> SourceKeshouPairs(int owner)
    {
        var cards = SourceKeshouEligibleMaterials(owner).ToArray();
        var pairs = new List<SourceKeshouMaterial[]>();
        for (var a = 0; a < cards.Length; a++) for (var b = a + 1; b < cards.Length; b++)
            if (cards[a].IsRed == cards[b].IsRed) pairs.Add([cards[a], cards[b]]);
        return pairs;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62061, SkillProgramEffectOp.ResolveShanheSourceKeshou)]
    private bool CanOfferSourceKeshou(ProgramTriggerLegalityContext c) => c.SkillId == SourceKeshouContract.SkillId &&
        c.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } window &&
        window.TargetSeat == c.OwnerSeat && c.Owner.IsAlive && HasSourceKeshouPair(c.OwnerSeat);

    [ProgramCapability]
    private sealed class SourceKeshouCapability(GameEngine engine) : ISourceKeshouHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, string step) => engine.ResolveSourceKeshou(f, step); }

    private SkillProgramStepOutcome ResolveSourceKeshou(ProgramSkillFrame supplied, string step)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (step == "select-cost")
        {
            if (f.SourceKeshou is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context ||
                _resolutionStack.Count < 2 || _resolutionStack[^2] is not BeforeDamageProgramWindowFrame w ||
                w.Id != context.ParentFrameId || w.TargetSeat != f.OwnerSeat || w.Prevented || w.Amount <= 0 ||
                w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].Candidate) ||
                !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !HasSourceKeshouPair(f.OwnerSeat))
                throw new InvalidOperationException("Source993 requires its original positive damage candidate and an unpaid same-colour pair.");
            ReplaceRuntimeTop(f = f with { SourceKeshou = new(w.Id, w.ContinuationAttackResolutionId ?? w.ParentFrameId,
                new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, w.Amount, _movementSequence) });
            var choices = SourceKeshouChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
            _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "弃置两张颜色相同的手牌或装备牌，令此伤害-1。",
                choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
            _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return SkillProgramStepOutcome.AwaitChoice;
        }
        if (step != "after-cost" || !ValidSourceKeshou(f) || !SourceKeshouWasPaid(f) || f.SourceKeshou!.SoleFactionAfterCost is not null ||
            !ProgramEventHistory<ProgramDamageReducedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId &&
                e.OwnerSeat == f.OwnerSeat && e.TargetSeat == f.OwnerSeat && e.AmountBefore == f.SourceKeshou.OriginalAmount &&
                e.AmountAfter == Math.Max(0, e.AmountBefore - 1)))
            throw new InvalidOperationException("Source993's faction branch requires its actual pair payment and original native reduction.");
        var faction = GetEffectiveFactionId(_players[f.OwnerSeat]);
        var sole = _players[f.OwnerSeat].IsAlive && faction is not null && !_players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && GetEffectiveFactionId(p) == faction);
        var receipt = f.SourceKeshou with { SoleFactionAfterCost = sole };
        ReplaceRuntimeTop(f with { SourceKeshou = receipt, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings
            .Append(new ProgramChoiceResultBinding("sole-faction", sole ? "sole" : "shared", f.OwnerSeat)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new SourceKeshouPaidBranchEvent(f.Id, receipt));
        return SkillProgramStepOutcome.Continue;
    }

    private IReadOnlyList<PromptChoice> SourceKeshouChoices(ProgramSkillFrame f) => Array.AsReadOnly(SourceKeshouPairs(f.OwnerSeat)
        .Select(pair => new PromptChoice(new($"source-keshou.{f.Id}.{pair[0].CardId}-{pair[1].CardId}"),
            string.Join("、", pair.Select(m => GetAdvancedCard(m.CardId).DisplayName)), pair.Select(m => m.CardId).ToArray(), [],
            new Dictionary<string, string> { ["program-action"] = "source-keshou-cost", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());

    [ProgramAnswer("source-keshou-cost")]
    private void AnswerSourceKeshou(PromptChoice selected)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Source993 lost its selection frame.");
        if (!ValidSourceKeshou(f) || f.SourceKeshou!.Materials.Count != 0 ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _pendingDecision?.PlayerSeat != f.OwnerSeat ||
            !SourceKeshouChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) &&
                c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Source993 must choose its exact published original same-colour pair.");
        var pair = selected.Cards.Select(id => SourceKeshouEligibleMaterials(f.OwnerSeat).Single(m => m.CardId == id)).ToArray();
        ClearPendingDecision();
        ReplaceRuntimeTop(f with { SourceKeshou = f.SourceKeshou with { Materials = pair } });
        SetProgramCardSet(f.Id, "keshou-cost", pair.Select(m => m.CardId).ToArray(), SkillProgramCardSetVisibility.Private,
            pair.Select(m => m.From).ToArray(), selectionActorSeat: f.OwnerSeat);
        AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceKeshou)]
    private PromptChoice SelectAiSourceKeshou(PendingDecision p, ProgramSkillFrame f) => p.Choices
        .OrderBy(c => c.Cards.Sum(id => GetKeepValue(GetAdvancedCard(id), _players[f.OwnerSeat]))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();

    private bool SourceKeshouWasPaid(ProgramSkillFrame f) => f.SourceKeshou is { Materials.Count: 2 } r && r.Materials.All(material =>
        _cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.CardId == material.CardId && m.From == material.From && m.Reason == SourceKeshouCostReason &&
            (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (GetAdvancedCard(m.CardId).IsGeneralWeapon || IsEphemeralCardDestroyedMaterial(m)))));

    private bool ValidSourceKeshou(ProgramSkillFrame f)
    {
        if (f.SourceKeshou is not { } r || r.Source != new CardConversionSource(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) ||
            r.GameplayHash != f.GameplayHash || f.WindowContext?.ParentFrameId != r.WindowId || r.OriginalAmount <= 0 || r.SequenceBefore < 0 ||
            r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Materials.Count is not (0 or 2) ||
            r.Materials.Count == 2 && (r.Materials.Select(m => m.CardId).Distinct().Count() != 2 || r.Materials[0].IsRed != r.Materials[1].IsRed ||
                r.Materials.Any(m => m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))) return false;
        var index = _resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStack[index - 1] is BeforeDamageProgramWindowFrame w && w.Id == r.WindowId &&
            w.TargetSeat == f.OwnerSeat && (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == r.AttackId;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62061)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62061)]
    private bool KeepPaidSourceKeshou(ProgramSkillFrame f) => ValidSourceKeshou(f) && SourceKeshouWasPaid(f);

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62061)]
    private void AssertSourceKeshou(ProgramSkillFrame f)
    {
        if (f.SourceKeshou is not null && (!ValidSourceKeshou(f) || f.SourceKeshou.SoleFactionAfterCost is not null && !SourceKeshouWasPaid(f)))
            throw new InvalidOperationException("Source993 lost its owning damage frame, original source or paid same-colour material set.");
    }
}
