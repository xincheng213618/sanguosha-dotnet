#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangQiYingOlRuntime
{
    internal const string ZhenyiGainReason = "skill-program.zhenyi.three-categories";
    internal static CardConversionSource ZhangQiYingSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhang_qi_ying_ol_ZhangQiYingPrograms.ZhangQiYingSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 480)]
    internal bool CanOfferZhangQiYing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        SkillProgramEffectOp? op = null;
        for (var i = 0; i < trigger.Effects.Count; i++)
            if (ZhangQiYingComposition.Own(trigger.Effects[i].Op))
            {
                op = trigger.Effects[i].Op;
                break;
            }

        if (op is null)
            return true;
        var seat = candidate.OwnerSeat;
        return op switch
        {
            SkillProgramEffectOp.InitializeFaluSuitTokens => context.Window == SkillProgramTriggerWindow.GameStarting && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FaluTokenChangedEvent>().Any(e => e.OwnerSeat == seat && e.Reason == "gameStarting"),
            SkillProgramEffectOp.GainFaluTokensAfterActualDiscard => context.MovementBatch is { } b && b.Movements.Any(m => FaluDiscardOwnedSource(m)?.OwnerSeat == seat),
            SkillProgramEffectOp.ReorderTopByCurrentFaluTokens => seat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && FaluTokenCount(seat) > 0,
            SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei => context.Window == SkillProgramTriggerWindow.JudgmentBeforeResult && _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.Ziwei) > 0 && context.Judgment is { } j && _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is { } pending && pending.Id == j.JudgmentFrameId,
            SkillProgramEffectOp.IncreaseActualSourceDamageByYuqing => context.SourceSeat == seat && context.Amount > 0 && _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.Yuqing) > 0,
            SkillProgramEffectOp.GainThreeDeckCategoriesAfterElementalDamage => context.TargetSeat == seat && context.Amount > 0 && context.DamageFrameId is { } id && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } actual && actual.TargetSeat == seat && actual.DamageWasApplied && _coreNativeCardClassificationsOperations.GetDamageNature(actual) != DamageNature.Normal && _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.Gouchen) > 0,
            _ => false
        };
    }

    internal SkillProgramStepOutcome ExecuteZhangQiYing(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext ?? throw new InvalidOperationException("Zhang Qi Ying requires an actual native window.");
        if (f.InstructionIndex != 1 || f.TriggerId is null || f.ZhangQiYing is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferZhangQiYing(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Zhang Qi Ying lost its exact unpaid source and native window.");
        if (op == SkillProgramEffectOp.InitializeFaluSuitTokens)
        {
            foreach (var marker in CharacterZhangQiYingOlDefinitions.FaluTokens)
                SetFaluToken(f, marker, 1, "gameStarting");
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.GainFaluTokensAfterActualDiscard)
        {
            var batch = c.MovementBatch!;
            foreach (var m in batch.Movements.Where(m => FaluDiscardOwnedSource(m)?.OwnerSeat == f.OwnerSeat))
            {
                if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
                    throw new InvalidOperationException("Falu needs an actual committed movement.");
                var suit = _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Suit;
                var marker = suit switch
                {
                    Suit.Spade => PlayerMarkerKind.Ziwei,
                    Suit.Club => PlayerMarkerKind.Houtu,
                    Suit.Heart => PlayerMarkerKind.Yuqing,
                    Suit.Diamond => PlayerMarkerKind.Gouchen,
                    _ => throw new InvalidOperationException("Falu discard has no printed suit.")};
                SetFaluToken(f, marker, 1, "discard");
            }

            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ReorderTopByCurrentFaluTokens)
            return _coreProgramTopReorderOperations.BeginProgramTopReorder(f, FaluTokenCount(f.OwnerSeat), null);
        if (op == SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangQiYing = new(ZhangQiYingSource(f), f.GameplayHash, c.ParentFrameId, f.InstructionIndex, "judgment-choice") });
            PublishZhenyiJudgment(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (op == SkillProgramEffectOp.IncreaseActualSourceDamageByYuqing)
        {
            if (_resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame w || w.Id != c.ParentFrameId || w.SourceSeat != f.OwnerSeat || w.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.SourceSeat != w.SourceSeat || attack.TargetSeat != w.TargetSeat)
                throw new InvalidOperationException("Yuqing needs the exact source's actual unprevented damage.");
            SetFaluToken(f, PlayerMarkerKind.Yuqing, 0, "yuqing");
            attack.IncreaseFinalizedDamageAmount(1);
            _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { Amount = checked(w.Amount + 1) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyiDamageIncreasedEvent(f.Id, w.Id, w.SourceSeat, w.TargetSeat, w.Amount, w.Amount + 1));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.GainThreeDeckCategoriesAfterElementalDamage)
        {
            _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            var cards = new[]
            {
                SkillProgramCardCategory.Basic,
                SkillProgramCardCategory.Trick,
                SkillProgramCardCategory.Equipment
            }.Select(k => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(card => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(card.Kind, k))).Where(card => card is not null).Select(card => card!.Id).ToArray();
            var r = new ZhangQiYingReceipt(ZhangQiYingSource(f), f.GameplayHash, c.ParentFrameId, f.InstructionIndex, "gain-children", _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            {
                Cards = cards
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangQiYing = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            SetFaluToken(f, PlayerMarkerKind.Gouchen, 0, "gouchen");
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && cards.Length > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(f.OwnerSeat), new(ZhenyiGainReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.ZhangQiYing!with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangQiYing = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyiDeckGainPaidEvent(f.Id, r.Source, r.GameplayHash, r.Before, r.After, cards.Length));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        throw new InvalidOperationException("Unknown Zhang Qi Ying operation.");
    }

    SkillProgramStepOutcome IZhangQiYingProgramHost.ExecuteZhangQiYing(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteZhangQiYing(f, op);
}
