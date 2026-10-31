#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeJinOlRuntime
{
    internal const string HeJinGiftReason = "skill-program.he-jin.mouzhu.give", HeJinDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.HeJinDiscardReason;
    internal static CardConversionSource HeJinSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_he_jin_ol_HeJin.HeJinSource(f);
    internal SkillProgramStepOutcome BeginHeJinMouzhu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HeJinMouzhu is not null || f.SkillId != "ol:mouzhu" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || actor == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Count == 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            throw new InvalidOperationException("Mouzhu requires its own once-per-Play hand demand against one other living hand owner.");
        var r = new HeJinMouzhuReceipt(HeJinSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            HandIds = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Select(c => c.Id).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HeJinMouzhuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, actor, r.StartedAt, r.HandIds.Count));
        PublishHeJinPrompt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HeJinMouzhuChoices(ProgramSkillFrame f)
    {
        var r = f.HeJinMouzhu!;
        Dictionary<string, string> Params(string branch) => new()
        {
            ["program-action"] = "he-jin-mouzhu",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == HeJinMouzhuStage.ChoosingGift)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.ActorSeat, r.ActorSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => r.HandIds.Contains(card.Id)).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "he-jin-mouzhu", ["branch"] = "gift" } }).ToArray());
        if (r.Stage != HeJinMouzhuStage.ChoosingAssault)
            return[];
        return Array.AsReadOnly(new[] { CardKind.Slash, CardKind.Duel }.Where(k => CanIssueHeJinMouzhuCard(r.ActorSeat, f.OwnerSeat, k)).Select(k => new PromptChoice(new($"he-jin.{(f.Id)}.assault.{(k)}"), k == CardKind.Slash ? "视为对何进使用【杀】" : "视为对何进使用【决斗】", [], [f.OwnerSeat], new Dictionary<string, string>(Params("assault")) { ["card-kind"] = k.ToString() })).ToArray());
    }

    internal void PublishHeJinPrompt(ProgramSkillFrame f)
    {
        var mouzhu = f.HeJinMouzhu;
        var seat = mouzhu?.ActorSeat ?? f.OwnerSeat;
        var choices = mouzhu is not null ? HeJinMouzhuChoices(f) : HeJinYanhuoChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("He Jin's exact private decision has no legal choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, mouzhu?.Stage == HeJinMouzhuStage.ChoosingGift ? "选择交给何进的一张手牌。" : mouzhu is not null ? "选择视为对何进使用的牌。" : $"延祸：选择弃置杀手至多{(f.HeJinYanhuo!.Limit)}张牌，然后确认。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("he-jin-mouzhu")]
    internal void AnswerHeJinMouzhu(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Mouzhu lost its owning frame.");
        var r = f.HeJinMouzhu!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != r.ActorSeat || !HeJinMouzhuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mouzhu needs the actual donor's exact private answer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == HeJinMouzhuStage.ChoosingGift)
        {
            if (choice.Cards is not [var id] || !r.HandIds.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.ActorSeat))
                throw new InvalidOperationException("Mouzhu's original donor card moved before payment.");
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = r with { Stage = HeJinMouzhuStage.GiftChildren, GiftId = id, Before = before, After = before } });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(id), CardLocation.Hand(r.ActorSeat), CardLocation.Hand(f.OwnerSeat), new(HeJinGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, r.ActorSeat, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = f.HeJinMouzhu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HeJinMouzhuGiftPaidEvent(f.Id, r.ActorSeat, f.OwnerSeat, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var kind = Enum.Parse<CardKind>(choice.Parameters["card-kind"]);
        var origin = new HeJinMouzhuUseOrigin(f.Id, r.InstructionIndex, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, r.ActorSeat, f.OwnerSeat, kind);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = r with { Stage = HeJinMouzhuStage.AssaultIssued, Assault = origin } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HeJinMouzhuAssaultIssuedEvent(origin));
        BeginHeJinMouzhuCard(f, origin);
    }

    internal bool DrainHeJinChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26710)]
    internal bool ResumeHeJin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.HeJinMouzhu is null && f.HeJinYanhuo is null)
            return false;
        if (f.HeJinYanhuo is not null)
        {
            ResumeHeJinYanhuo(f);
            return true;
        }

        var r = f.HeJinMouzhu!;
        if (r.Stage is HeJinMouzhuStage.ChoosingGift or HeJinMouzhuStage.ChoosingAssault)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishHeJinPrompt(f);
            return true;
        }

        if (r.Stage == HeJinMouzhuStage.AssaultIssued)
            throw new InvalidOperationException("Mouzhu must return through its exact native card use.");
        if (DrainHeJinChildren(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.HeJinMouzhu!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat]).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count)
        {
            FinishHeJinMouzhu(f, false);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = r with { Stage = HeJinMouzhuStage.ChoosingAssault } });
        if (HeJinMouzhuChoices(f).Count == 0)
        {
            FinishHeJinMouzhu(f, false);
            return true;
        }

        PublishHeJinPrompt(f);
        return true;
    }

    internal bool MatchesHeJinMouzhu(ProgramSkillFrame f, HeJinMouzhuUseOrigin origin) => ValidHeJinMouzhu(f) && f.HeJinMouzhu is { Stage: HeJinMouzhuStage.AssaultIssued, Assault: { } issued } && issued == (origin with
    {
        AttackStarted = false
    }

    );
    internal void FinishHeJinMouzhu(ProgramSkillFrame f, bool issued)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HeJinMouzhuCompletedEvent(f.Id, issued));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HeJinMouzhu = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouzhuHandDemandAndAssault, SkillProgramEffectOp.ResolveYanhuoKillerDiscard)]
    internal PromptChoice ChooseHeJinAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.HeJinMouzhu is { Stage: HeJinMouzhuStage.ChoosingAssault })
            return p.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("card-kind") == "Slash").First();
        if (f.HeJinMouzhu is not null)
            return p.Choices.OrderBy(c => c.Cards is [var id] && _coreCardAttackStateOperations.GetAttackCard(id).Kind is CardKind.Peach or CardKind.Dodge ? 1 : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.Count > 0) ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.First();
    }

    SkillProgramStepOutcome IHeJinProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouzhuHandDemandAndAssault ? BeginHeJinMouzhu(f) : BeginHeJinYanhuo(f);
}
