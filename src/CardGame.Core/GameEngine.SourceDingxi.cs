using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private sealed record SourceDingxiUsePlan(int CardId, CardKind Kind, LegalActionKind Action, CardKind? Required);
    private static bool SourceDingxiDamageCard(CardKind k) => k is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or
        CardKind.ColdSlash or CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage or
        CardKind.UnexpectedAssault or CardKind.AgainstCurrent or CardKind.WumiaoFlood or CardKind.Lightning;
    // Frozen Client.GetLeftSeat decrements, wrapping and skipping inoperable seats.
    private int SourceDingxiLeft(int owner)
    {
        for (var offset = 1; offset < _players.Count; offset++)
        {
            var seat = (owner - offset + _players.Count) % _players.Count;
            if (_players[seat].IsAlive) return seat;
        }
        return -1;
    }
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 24510)]
    private void CaptureSourceDingxiUseDiscards(CardMovementBatchContext batch)
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSourceDingxiUsedCard) || batch.ParentFrameId is not { } parent ||
            LifecycleCardUse(parent) is not { SourceDingxiUse: null, Action: { Type: CardActionType.Use } action } use ||
            !SourceDingxiDamageCard(use.CardKind)) return;
        // Cleanup movement windows may run after the whole native Use has popped.
        // Freeze its exact proof while that original frame still owns the batch.
        foreach (var m in batch.Movements.Where(m => IsNativeTrueUseDiscardMovement(batch, m) &&
                     action.PhysicalCards.Any(c => c.CardId == m.CardId) && SourceDingxiDamageCard(m.CardKind)))
            AdvanceEventRulesAndQueueFact(new SourceDingxiDiscardQualifiedEvent(batch.Id, use.Id, action.ActionId, action.ActorSeat, m.CardId, m.Sequence));
    }
    private int[] SourceDingxiCandidates(int owner, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { ParentFrameId: { } parent } batch }) return [];
        return batch.Movements.Where(m => m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && _cardMovements.Contains(m) &&
            SourceDingxiDamageCard(m.CardKind) && _cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile &&
            ProgramEventHistory<SourceDingxiDiscardQualifiedEvent>().Count(e => e.BatchId == batch.Id && e.OriginalUseId == parent &&
                e.OwnerSeat == owner && e.CardId == m.CardId && e.DiscardSequence == m.Sequence) == 1)
            .Select(m => m.CardId).Distinct().ToArray();
    }
    private SourceDingxiUsePlan? SourceDingxiPlan(int owner, int id, int target)
    {
        if (!IsValidPlayerSeat(target) || !_players[target].IsAlive || !_players[owner].IsAlive || _winner != Winner.None ||
            (_cardZones.GetLocation(id) != CardLocation.DiscardPile && _cardZones.GetLocation(id) != CardLocation.Processing)) return null;
        var card = GetAdvancedCard(id); var kind = card.Kind; var actor = _players[owner];
        var suit = GetProgramEffectiveSuit(actor, card); var color = SuitColor(suit);
        if (!SourceDingxiDamageCard(kind) || IsCardUseForbidden(owner, kind, CardActionType.Use) || IsTurnPhysicalUseForbidden(owner, [id]) ||
            !TargetRules.AllowsNativeUseMaterial(new(owner, kind, [id], null)) || IsCuiYanNextUseForbidden(owner, [id], suit)) return null;
        if (IsSlashCard(kind)) return CanUseSlashTarget(actor, _players[target], card, effectiveKind: kind, physicalCardIds: [id]) ? new(id, kind, LegalActionKind.Slash, null) : null;
        // Lightning retains its normal self-only target domain; "left" does not override it.
        if (!IsOrdinaryTrick(kind)) return null;
        var options = BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit,
            actualEffectiveColor: color, hasActualColor: true, physicalCardIds: [id]);
        var group = kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage;
        var option = options.FirstOrDefault(o => group ? o.TargetSeats.Contains(target) : o.TargetSeats.SequenceEqual([target]));
        return option is null ? null : new(id, kind, option.ActionKind, option.RequiredCardKind);
    }
    private SkillProgramStepOutcome StartSourceDingxi(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var left = SourceDingxiLeft(f.OwnerSeat);
        var ids = SourceDingxiCandidates(f.OwnerSeat, f.WindowContext).Where(id => SourceDingxiPlan(f.OwnerSeat, id, left) is not null).ToArray();
        if (ids.Length == 0) return SkillProgramStepOutcome.Continue;
        var batch = f.WindowContext!.MovementBatch!;
        var original = ProgramEventHistory<SourceDingxiDiscardQualifiedEvent>().First(e => e.BatchId == batch.Id && e.OwnerSeat == f.OwnerSeat && ids.Contains(e.CardId));
        ReplaceRuntimeTop(f = f with { SourceDingxi = new(batch.Id, original.OriginalUseId, original.OriginalActionId, left, Array.AsReadOnly(ids), SourceDingxiStage.Choose) });
        var choices = ids.Select(id => new PromptChoice(new ChoiceId($"source-dingxi.{f.Id}.{id}"), $"对左侧角色使用【{GetAdvancedCard(id).DisplayName}】", [id], [left],
            new Dictionary<string, string> { ["program-action"] = "source-dingxi-card" })).ToArray();
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "定西：选择原牌，对左侧角色使用。", ids, [left], f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = Array.AsReadOnly(choices), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChild;
    }
    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 24510)]
    private int[]? SelectSourceDingxiDiscards(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features)
    {
        if (!features.HasOperation(SkillProgramEffectOp.ResolveSourceDingxiUsedCard)) return null;
        if (candidate.SkillId != SourceBeigongIds.Dingxi || GetProgramTrigger(candidate) is not
            { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementOccurrence: SkillProgramMovementOccurrence.PerBatch,
                Effects: [{ Op: SkillProgramEffectOp.ResolveSourceDingxiUsedCard }] }) return [];
        var eligible = SourceDingxiCandidates(candidate.OwnerSeat, new(SkillProgramTriggerWindow.DiscardPileReceived, batch.ParentFrameId ?? 0,
            candidate.OwnerSeat, MovementBatch: batch)).ToHashSet();
        return batch.Movements.Select((m, i) => (m, i)).Where(p => eligible.Contains(p.m.CardId)).Select(p => p.i).ToArray();
    }
    [ProgramAnswer("source-dingxi-card")]
    private void AnswerSourceDingxi(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SourceDingxi: { Stage: SourceDingxiStage.Choose } r } f ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || selected.Cards is not [var id] || !r.OriginalCardIds.Contains(id) ||
            !SourceDingxiCandidates(f.OwnerSeat, f.WindowContext).Contains(id) || SourceDingxiPlan(f.OwnerSeat, id, r.LeftSeat) is null)
            throw new InvalidOperationException("Source3601 requires an available original damage-use discard and its fixed left target.");
        var discarded = f.WindowContext!.MovementBatch!.Movements.Single(m => m.CardId == id);
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect,
            SourceDingxi = r with { Stage = SourceDingxiStage.EntryChildren, CardId = id, DiscardSequence = discarded.Sequence,
                Before = _movementSequence, After = _movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MoveProgramCardsFromMultipleSources([id], CardLocation.Processing, new("skill-program.source-dingxi.enter"), (_, moves) =>
        {
            var current = GetActiveProgramFrame(f.Id); var entry = moves.Single();
            ReplaceRuntimeTop(current with { SourceDingxi = current.SourceDingxi! with { EntrySequence = entry.Sequence, After = _movementSequence } });
            AdvanceEventRulesAndQueueFact(new SourceDingxiEnteredEvent(f.Id, f.OwnerSeat, id, discarded.Sequence, entry.Sequence));
        });
        // A public binding owns the actual Processing entity while entry children run.
        SetProgramCardSet(f.Id, "source-dingxi-material", [id], SkillProgramCardSetVisibility.Public, [CardLocation.Processing]);
        ContinueSourceBeigong(f.Id);
    }
    private void ContinueSourceDingxi(ProgramSkillFrame f)
    {
        var r = f.SourceDingxi!;
        if (r.Stage == SourceDingxiStage.DrawChildren || r.Stage == SourceDingxiStage.StoreChildren) { FinishSourceBeigong(f); return; }
        if (r.Stage == SourceDingxiStage.EntryChildren)
        {
            if (_cardZones.GetLocation(r.CardId) != CardLocation.Processing || SourceDingxiPlan(f.OwnerSeat, r.CardId, r.LeftSeat) is not { } plan)
            {
                // Another real skill may have obtained the card; never reclaim or clone it.
                if (_cardZones.GetLocation(r.CardId) == CardLocation.Processing)
                    MoveCard(GetAdvancedCard(r.CardId), CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.source-dingxi.unused"));
                FinishSourceBeigong(f); return;
            }
            ReplaceRuntimeTop(f = f with { SourceDingxi = r with { Stage = SourceDingxiStage.Issuing } });
            IssueSourceDingxiUse(f, plan); return;
        }
        if (r.Stage != SourceDingxiStage.Returned) return;
        ReplaceRuntimeTop(f = f with { SourceDingxi = r with { Stage = SourceDingxiStage.StoreChildren, Before = _movementSequence, After = _movementSequence },
            PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if ((_cardZones.GetLocation(r.CardId) == CardLocation.Processing || _cardZones.GetLocation(r.CardId) == CardLocation.DiscardPile) && _players[f.OwnerSeat].IsAlive &&
            HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            var pile = EnsurePublicPileSource(f, int.MaxValue);
            MoveProgramCardsFromMultipleSources([r.CardId], pile.Location, new("skill-program.source-dingxi.store"), (_, moves) =>
            {
                var current = GetActiveProgramFrame(f.Id);
                ReplaceRuntimeTop(current with { SourceDingxi = current.SourceDingxi! with { After = _movementSequence } });
                AdvanceEventRulesAndQueueFact(new SourceDingxiStoredEvent(f.Id, f.OwnerSeat, r.CardId, moves.Single().Sequence));
            });
        }
        else if (_cardZones.GetLocation(r.CardId) == CardLocation.Processing)
            MoveCard(GetAdvancedCard(r.CardId), CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.source-dingxi.source-lost"));
        ContinueSourceBeigong(f.Id);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveSourceDingxiUsedCard)]
    private PromptChoice SourceDingxiAiChoice(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
