#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageSourceHandChoiceRuntime
{
    internal const string DamageSourceHandDrawReason = "skill-program.damage-source-hand.draw";
    internal const string DamageSourceHandDiscardReason = "skill-program.damage-source-hand.discard";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 270)]
    internal bool CanOfferDamageSourceHandChoice(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (!t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveDamageSourceHandChoice))
            return true;
        return context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, DamageFrameId: not null, Amount: > 0 } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source);
    }

    internal SkillProgramStepOutcome BeginDamageSourceHandChoice(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.DamageSourceHandChoice is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, DamageFrameId: { } damageId, Amount: > 0 } context || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } window || window.Id != context.ParentFrameId || window.ParentFrameId != damageId || context.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("The hand choice requires its actual sourced damage occurrence.");
        var r = new DamageSourceHandChoiceReceipt
        {
            Source = CoreGameEngineNamedAmbushRuntime.AmbushSource(f),
            GameplayHash = f.GameplayHash,
            InstructionIndex = f.InstructionIndex,
            WindowFrameId = window.Id,
            DamageFrameId = damageId,
            DamageSourceSeat = source,
            DamageAmount = context.Amount,
            SourceHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[source]).Count,
            OwnerHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageSourceHandChoiceOfferedEvent(f.Id, r));
        PublishDamageSourceHandChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DamageSourceHandChoices(ProgramSkillFrame f)
    {
        var r = f.DamageSourceHandChoice!;
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"damage-source-hand.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "damage-source-hand", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        var choices = new List<PromptChoice>();
        if (r.Stage == DamageSourceHandChoiceStage.Branch)
        {
            var draw = Math.Max(0, Math.Min(5, r.SourceHandCount) - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
            choices.Add(Choice("draw", $"摸 {(draw)} 张牌（至多摸至五张）"));
            if (_coreMatchParticipantsPlayersAccess._players[r.DamageSourceSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count >= r.SourceHandCount + 1)
                choices.Add(Choice("retaliate", $"弃置至少 {(r.SourceHandCount + 1)} 张手牌，对伤害来源造成1点伤害"));
        }
        else if (r.Stage == DamageSourceHandChoiceStage.DiscardSelection)
        {
            choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.SelectedCards.Contains(c.Id)).Select(c => Choice("card", $"选择弃置【{(c.DisplayName)}】", [c.Id])));
            if (r.SelectedCards.Count >= r.SourceHandCount + 1 && _coreMatchParticipantsPlayersAccess._players[r.DamageSourceSeat].IsAlive)
                choices.Add(Choice("pay", $"确认弃置 {(r.SelectedCards.Count)} 张手牌"));
        }

        choices.Add(Choice("decline", "取消"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishDamageSourceHandChoice(ProgramSkillFrame f)
    {
        var choices = DamageSourceHandChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = f.DamageSourceHandChoice!with { Choices = choices } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], rSource(f))
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.DamageSourceHandChoice!.Stage == DamageSourceHandChoiceStage.DiscardSelection,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        static int rSource(ProgramSkillFrame frame) => frame.DamageSourceHandChoice!.DamageSourceSeat;
    }

    [ProgramAnswer("damage-source-hand")]
    internal void ResolveDamageSourceHandChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Damage hand choice lost owner.");
        RequireDamageSourceHandChoice(f);
        var r = f.DamageSourceHandChoice!;
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], DamageSourceHandChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Damage hand choice no longer matches its published branch or hand cost.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "decline" || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            CompleteDamageSourceHandChoice(f, false);
            return;
        }

        if (r.Stage == DamageSourceHandChoiceStage.Branch && branch == "retaliate")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = r with { Stage = DamageSourceHandChoiceStage.DiscardSelection, Choices = [] } });
            PublishDamageSourceHandChoice(f);
            return;
        }

        if (r.Stage == DamageSourceHandChoiceStage.DiscardSelection && branch == "card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = r with { SelectedCards = r.SelectedCards.Append(choice.Cards.Single()).ToArray(), Choices = [] } });
            PublishDamageSourceHandChoice(f);
            return;
        }

        var draw = r.Stage == DamageSourceHandChoiceStage.Branch && branch == "draw";
        if (!draw && (branch != "pay" || r.SelectedCards.Count < r.SourceHandCount + 1 || r.SelectedCards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat))))
            throw new InvalidOperationException("Retaliation must pay at least the frozen source hand count plus one from hand.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = r with { Draw = draw, Stage = DamageSourceHandChoiceStage.MovementChildren, MovementBefore = before, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var actual = draw ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], Math.Max(0, Math.Min(5, r.SourceHandCount) - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count), true, new(DamageSourceHandDrawReason)).Count : 0;
        if (!draw)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.SelectedCards, new(DamageSourceHandDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = f.DamageSourceHandChoice!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawActual = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageSourceHandChoicePaidEvent(f.Id, f.DamageSourceHandChoice!));
        if (!_coreNamedAmbushOperations.DrainNamedHandChoiceChildren(f))
            ReturnDamageSourceHandMovement(f);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 20702)]
    internal bool ReturnDamageSourceHandMovement(ProgramSkillFrame f)
    {
        if (f.DamageSourceHandChoice is null)
            return false;
        RequireDamageSourceHandChoice(f);
        if (f.DamageSourceHandChoice.Stage != DamageSourceHandChoiceStage.MovementChildren || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Damage hand adjustment lost typed paid return.");
        if (_coreNamedAmbushOperations.DrainNamedHandChoiceChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        var r = f.DamageSourceHandChoice!;
        if (r.Draw || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.DamageSourceSeat].IsAlive)
        {
            CompleteDamageSourceHandChoice(f, true);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = r with { Stage = DamageSourceHandChoiceStage.DamageIssued } });
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.DamageSourceSeat, 1);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20702)]
    internal bool ResumeDamageSourceHandChoice(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DamageSourceHandChoice: { } r } f || f.Id != id)
            return false;
        RequireDamageSourceHandChoice(f);
        if (f.PendingMovementContinuation is not null)
        {
            ReturnDamageSourceHandMovement(f);
            return true;
        }

        if (r.Stage == DamageSourceHandChoiceStage.DamageIssued)
        {
            if (f.AttackAttempt is not null)
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(f.Id));
            else
                CompleteDamageSourceHandChoice(f, true);
        }

        return true;
    }

    internal void CompleteDamageSourceHandChoice(ProgramSkillFrame f, bool completed)
    {
        var r = f.DamageSourceHandChoice!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageSourceHandChoiceCompletedEvent(f.Id, r.DamageFrameId, r.DamageSourceSeat, r.Draw));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageSourceHandChoice = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    internal void RequireDamageSourceHandChoice(ProgramSkillFrame f)
    {
        if (f.DamageSourceHandChoice is not { } r || r.Source != CoreGameEngineNamedAmbushRuntime.AmbushSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.ParentFrameId != r.WindowFrameId || context.DamageFrameId != r.DamageFrameId || context.TargetSeat != f.OwnerSeat || context.SourceSeat != r.DamageSourceSeat || context.Amount != r.DamageAmount || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveDamageSourceHandChoice || r.SourceHandCount < 0 || r.OwnerHandCount < 0 || r.SelectedCards.Distinct().Count() != r.SelectedCards.Count || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageSourceHandChoiceOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.WindowFrameId == r.WindowFrameId && e.Receipt.DamageFrameId == r.DamageFrameId && e.Receipt.SourceHandCount == r.SourceHandCount) != 1)
            throw new InvalidOperationException("Damage hand choice lost its exact sourced damage, instruction or frozen count.");
        if (r.Stage is not (DamageSourceHandChoiceStage.MovementChildren or DamageSourceHandChoiceStage.DamageIssued))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageSourceHandChoicePaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.MovementBefore == r.MovementBefore && e.Receipt.MovementAfter == r.MovementAfter && e.Receipt.Draw == r.Draw && e.Receipt.SelectedCards.SequenceEqual(r.SelectedCards)) != 1)
            throw new InvalidOperationException("Damage hand choice lost paid issuance.");
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == (r.Draw ? DamageSourceHandDrawReason : DamageSourceHandDiscardReason)).ToArray();
        if (r.Draw ? moves.Length != r.DrawActual || moves.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(f.OwnerSeat)) : r.SelectedCards.Count < r.SourceHandCount + 1 || moves.Length != r.SelectedCards.Count || !moves.Select(m => m.CardId).Order().SequenceEqual(r.SelectedCards.Order()) || moves.Any(m => m.From != CardLocation.Hand(f.OwnerSeat) || m.To != CardLocation.DiscardPile))
            throw new InvalidOperationException("Damage hand adjustment lost its exact real draw or paid hand discard.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 430)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 430)]
    internal bool CanContinueDamageSourceHandChoice(ProgramSkillFrame f)
    {
        if (f.DamageSourceHandChoice is not { Stage: DamageSourceHandChoiceStage.MovementChildren or DamageSourceHandChoiceStage.DamageIssued })
            return false;
        RequireDamageSourceHandChoice(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveDamageSourceHandChoice)]
    internal PromptChoice SelectAiDamageSourceHandChoice(PendingDecision d, ProgramSkillFrame f)
    {
        if (f.DamageSourceHandChoice!.Stage == DamageSourceHandChoiceStage.Branch)
            return d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "draw") ?? d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "retaliate") ?? d.Choices[0];
        return d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? d.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? d.Choices[0];
    }

    SkillProgramStepOutcome IDamageSourceHandChoiceHost.ResolveDamageSourceHandChoice(ProgramSkillFrame f) => BeginDamageSourceHandChoice(f);
}
