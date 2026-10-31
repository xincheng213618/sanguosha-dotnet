#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaHuiOlRuntime
{
    internal static bool IsSimaHuiOp(SkillProgramEffectOp op) => RuntimeSkills.RuntimeRules_sima_hui_ol_SimaHuiPrograms.IsSimaHuiOp(op);
    internal CardConversionSource SimaHuiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SimaHuiReceipt NewSimaHuiReceipt(ProgramSkillFrame f, string stage) => new()
    {
        Source = SimaHuiSource(f),
        GameplayHash = f.GameplayHash,
        ParentFrameId = f.WindowContext?.ParentFrameId ?? f.ParentFrameId ?? 0,
        ActualTurn = f.WindowContext?.BeforeTurnPreludeProof?.UpcomingTurnNumber ?? _coreActualTurnProgressTurnNumberAccess._turnNumber,
        InstructionIndex = f.InstructionIndex,
        Stage = stage
    };
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 410)]
    internal bool CanOfferSimaHuiLifecycle(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [{ Op: var op }] || !IsSimaHuiOp(op))
            return true;
        var owner = candidate.OwnerSeat;
        if (op == SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn)
            return context.BeforeTurnPreludeProof is { } p && p.UpcomingOwnerSeat == owner && _characterBeiMiHuOlOperations.ValidBeiMiHuBeforeTurnPrelude(p) && SimaHuiOwnTurns(owner) == 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SimaHuiSealMovedEvent>().Any(e => e.Issuer.OwnerSeat == owner) && _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.Seat != owner) >= 2;
        if (op == SkillProgramEffectOp.TransferDeadDragonPhoenixSeal)
            return context.Window == SkillProgramTriggerWindow.CharacterDied && context.TargetSeat is { } victim && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.VictimSeat == victim && CharacterSimaHuiOlDefinitions.SimaHuiSeals.Any(m => SimaHuiHolder(new("ol:jianjie", "seals", owner, candidate.SkillInstanceId), m) == victim) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != victim);
        return true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 50)]
    internal bool CanActivateSimaHui(int owner, SkillProgramActivation activation)
    {
        var features = ProgramInstructionResolver.Default.Features(activation);
        if (features.HasOperation(SkillProgramEffectOp.TransferOwnedDragonOrPhoenixSeal))
            return SimaHuiOwnTurns(owner) > 1 && CharacterSimaHuiOlDefinitions.SimaHuiSeals.Any(m => _coreMatchParticipantsPlayersAccess._players.Any(p => p.MarkerSourceCounts.GetValueOrDefault((m, owner)) > 0));
        if (features.HasOperation(SkillProgramEffectOp.ConsumeDragonPhoenixForNativeYeyan))
            return CharacterSimaHuiOlDefinitions.SimaHuiSeals.All(m => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(m) > 0);
        return true;
    }

    internal SkillProgramStepOutcome BeginSimaHuiOperation(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SimaHui is not null || f.InstructionIndex != 1 || !IsSimaHuiOp(op))
            throw new InvalidOperationException("Sima Hui requires its exact unpaid leading instruction.");
        if (op == SkillProgramEffectOp.PreventDamageWithoutSealsAndArmor)
        {
            if (!CanPreventSimaHuiDamage(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!))
                throw new InvalidOperationException("Yinshi lost its actual damage or equipment qualification.");
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ConsumeDragonPhoenixForNativeYeyan)
            return PaySimaHuiYeyanSeals(f);
        if (op == SkillProgramEffectOp.DistributeTopCardsAfterElementalChainOrigin)
            return BeginSimaHuiDistribution(f);
        var initial = op == SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn;
        if (f.TriggerId is not null && !CanOfferSimaHuiLifecycle(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!))
            throw new InvalidOperationException("Jianjie lost its native turn/death eligibility.");
        if (!initial && f.TriggerId is null && !CanActivateSimaHui(f.OwnerSeat, ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation!))
            throw new InvalidOperationException("Jianjie cannot transfer in its first actual turn.");
        var stage = initial ? "initial-dragon" : "transfer-kind";
        var r = NewSimaHuiReceipt(f, stage)with
        {
            Candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (!initial || p.Seat != f.OwnerSeat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimaHuiChoiceStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.ActualTurn, r.Stage, r.Candidates));
        PublishSimaHuiChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SimaHuiChoices(ProgramSkillFrame f)
    {
        var r = f.SimaHui!;
        var result = new List<PromptChoice>();
        void Add(string value, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => result.Add(new(new($"sima-hui.{(f.Id)}.{(r.Stage)}.{(r.Cursor)}.{(value)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "sima-hui", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["value"] = value }));
        if (r.Stage == "transfer-kind")
        {
            foreach (var marker in CharacterSimaHuiOlDefinitions.SimaHuiSeals)
                if (SimaHuiHolder(SimaHuiIssuer(f), marker)is { } holder && (f.WindowContext?.Window != SkillProgramTriggerWindow.CharacterDied || holder == f.WindowContext.TargetSeat))
                    Add(marker.ToString(), "转移" + PlayerMarkerCatalog.GetDisplayName(marker));
            Add("finish", "结束转移");
        }
        else if (r.Stage == "distribute")
        {
            var card = r.OriginalCards[r.Cursor];
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                Add(p.Seat.ToString(CultureInfo.InvariantCulture), "将此牌交给" + p.Name, [card], [p.Seat]);
        }
        else
            foreach (var seat in r.Candidates.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && !r.SelectedSeats.Contains(s) && s != r.PreviousHolder))
                Add(seat.ToString(CultureInfo.InvariantCulture), "选择" + _coreMatchParticipantsPlayersAccess._players[seat].Name, targets: [seat]);
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishSimaHuiChoice(ProgramSkillFrame f)
    {
        var choices = SimaHuiChoices(f);
        if (choices.Count == 0)
        {
            FinishSimaHui(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.SimaHui!.Stage == "distribute",
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("sima-hui")]
    internal void ResolveSimaHuiChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing Sima Hui choice frame.");
        AssertSimaHui(f);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], SimaHuiChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Sima Hui choice lost its exact current entity/recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var r = f.SimaHui!;
        var value = selected.Parameters["value"];
        if (value == "finish")
        {
            FinishSimaHui(f);
            return;
        }

        if (r.Stage == "transfer-kind")
        {
            var marker = Enum.Parse<PlayerMarkerKind>(value);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = "transfer-target", Kind = value, PreviousHolder = SimaHuiHolder(SimaHuiIssuer(f), marker), Candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray() } });
            PublishSimaHuiChoice(f);
            return;
        }

        if (r.Stage == "distribute")
        {
            IssueSimaHuiGift(f, int.Parse(value, CultureInfo.InvariantCulture));
            return;
        }

        var target = selected.Targets.Single();
        var kind = r.Stage == "initial-dragon" ? PlayerMarkerKind.DragonSeal : r.Stage == "initial-phoenix" ? PlayerMarkerKind.PhoenixSeal : Enum.Parse<PlayerMarkerKind>(r.Kind);
        var nextStage = r.Stage == "initial-dragon" ? "initial-phoenix" : r.Stage == "initial-phoenix" ? "complete" : f.TriggerId is null ? "complete" : "transfer-kind";
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = "seal-children", Kind = nextStage, ChosenHolder = target, SelectedSeats = r.Stage == "initial-dragon" ? [target] : r.SelectedSeats }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        SetSimaHuiSeal(f, SimaHuiIssuer(f), kind, r.PreviousHolder, target);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23500)]
    internal bool ResumeSimaHui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SimaHui: { } r } f || f.Id != id || r.Stage == "yeyan-paid")
            return false;
        AssertSimaHui(f);
        if (r.Stage is "initial-dragon" or "initial-phoenix" or "transfer-kind" or "transfer-target" or "distribute")
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSimaHuiChoice(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.SimaHui!;
        if (r.Stage == "return-children")
        {
            FinishSimaHui(f);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            if (ReturnSimaHuiUnassignedCards(f))
                return true;
            FinishSimaHui(f);
            return true;
        }

        if (r.Stage == "seal-children")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = r.Kind, Kind = "", PreviousHolder = null }, PendingMovementContinuation = null });
            if (r.Kind == "complete")
                FinishSimaHui(f);
            else
                PublishSimaHuiChoice(f);
            return true;
        }

        var cursor = r.Cursor + (r.Stage == "gift-children" ? 1 : 0);
        while (cursor < r.OriginalCards.Count && _cardZonesOperations._cardZones.GetLocation(r.OriginalCards[cursor]) != CardLocation.Processing)
            cursor++;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = "distribute", Cursor = cursor, ChosenHolder = null }, PendingMovementContinuation = null });
        if (cursor == r.OriginalCards.Count)
            FinishSimaHui(f);
        else
            PublishSimaHuiChoice(f);
        return true;
    }

    internal void FinishSimaHui(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome ISimaHuiProgramHost.BeginSimaHuiOperation(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginSimaHuiOperation(frame, op);
}
