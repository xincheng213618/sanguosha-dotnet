#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHpDamageShieldsRuntime
{
    internal SkillProgramStepOutcome BeginHpDamageShieldPayment(ProgramSkillFrame frame, PlayerMarkerKind marker)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || active.HpDamageShieldDraft is not null || active.HpDamageShieldReceipt is not null || !owner.IsAlive || owner.Hp <= 0 || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != owner.Seat)
            throw new InvalidOperationException("HP-paid shields require their unpaid owner's turn-ending program.");
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner.Seat).Select(p => p.Seat).ToArray();
        var maximum = Math.Min(owner.Hp, candidates.Length);
        if (maximum == 0)
            return SkillProgramStepOutcome.Continue;
        active = active with
        {
            HpDamageShieldDraft = new(active.InstructionIndex, marker, owner.Hp, maximum, Array.AsReadOnly(candidates), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, owner.Seat, $"【{(skill.Name)}】选择其他角色；每选择一名角色失去1点体力。", [], candidates, owner.Seat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(active.SkillId, skill.Name, $"{(skill.Name)} · 失去体力与保护", skill.Description),
            Choices = HpDamageShieldChoices(active)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HpDamageShieldChoices(ProgramSkillFrame frame)
    {
        var draft = frame.HpDamageShieldDraft ?? throw new InvalidOperationException("Missing shield quantity draft.");
        var choices = new List<PromptChoice>();
        void AddCombinations(int size, int from, List<int> selected)
        {
            if (selected.Count == size)
            {
                var targets = Array.AsReadOnly(selected.ToArray());
                choices.Add(new(new ChoiceId($"hp-shield.frame-{(frame.Id)}.{(string.Join('-', targets))}"), $"失去{(size)}点体力，保护{(string.Join('、', targets.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)))}", [], targets, new Dictionary<string, string> { ["program-action"] = "hp-damage-shield", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["quantity"] = size.ToString(CultureInfo.InvariantCulture) }));
                return;
            }

            for (var i = from; i <= draft.CandidateSeats.Count - (size - selected.Count); i++)
            {
                selected.Add(draft.CandidateSeats[i]);
                AddCombinations(size, i + 1, selected);
                selected.RemoveAt(selected.Count - 1);
            }
        }

        for (var count = 1; count <= draft.Maximum; count++)
            AddCombinations(count, 0, []);
        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAnswer("hp-damage-shield")]
    internal void ResolveHpDamageShieldPayment(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The shield choice lost its owning program.");
        var draft = frame.HpDamageShieldDraft ?? throw new InvalidOperationException("The shield choice lost its unpaid draft.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [HpDamageShieldChoices(frame).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("The shield choice differs from its frozen owning prompt.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!owner.IsAlive || owner.Hp != draft.HpBefore || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber != draft.TurnNumber || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != draft.TurnOwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId) || choice.Targets.Any(s => !_coreMatchParticipantsPlayersAccess._players[s].IsAlive))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "付款前体力、目标或技能实例失效，剩余结算取消。");
            return;
        }

        var requested = choice.Targets.Count;
        if (requested < 1 || requested > draft.Maximum || requested > owner.Hp)
            throw new InvalidOperationException("The frozen shield targets exceed the affordable HP quantity.");
        var before = owner.Hp;
        owner.Hp -= requested;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HpDamageShieldDraft = null, HpDamageShieldReceipt = new(frame.InstructionIndex, draft.Marker, frame.OwnerSeat, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId, frame.GameplayHash, before, owner.Hp, before - owner.Hp, Array.AsReadOnly(choice.Targets.ToArray()), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) });
        _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, null, owner.Seat, before, owner.Hp, HpChangeKind.Loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(frame.Id, frame.SkillId, owner.Seat, before - owner.Hp, owner.Hp));
        if (owner.Hp == 0)
            _coreSkillProgramsOperations.BeginProgramSkillDying(frame.Id, owner);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    // Invoked before ordinary executor cancellation. The physical cost is already
    // paid; source death or skill loss cannot invalidate the surviving recipients.
    [ProgramContinuation(ProgramContinuationKind.Resume, 790)]
    internal bool ResumeHpDamageShieldPayment(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.HpDamageShieldReceipt is not { Granted: false } receipt)
            return false;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
        {
            // The completed match cannot acquire new rule effects. Consume only
            // this producer's unobservable HP obligation and cancel its tail.
            _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.RemoveAll(change => change.ParentFrameId == frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HpDamageShieldReceipt = receipt with { Granted = true } });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), completed: false);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HpDamageShieldReceipt = receipt with { Granted = true } });
        foreach (var seat in receipt.TargetSeats)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[seat];
            if (!target.IsAlive)
                continue;
            var shield = new OneUseDamageShield(frame.Id, receipt.InstructionIndex, receipt.OwnerSeat, seat, receipt.Marker, receipt.SkillId, receipt.BindingId, receipt.SkillInstanceId, receipt.GameplayHash);
            target.OneUseDamageShields.Add(shield);
            var source = (receipt.Marker, receipt.OwnerSeat);
            target.MarkerSourceCounts[source] = target.MarkerSourceCounts.GetValueOrDefault(source) + 1;
            target.Markers[receipt.Marker] = target.Markers.GetValueOrDefault(receipt.Marker) + 1;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, seat, receipt.Marker, 1, target.Markers[receipt.Marker], receipt.OwnerSeat, "program.hp-paid-damage-shield"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OneUseDamageShieldGrantedEvent(shield));
        }

        return false;
    }

    internal bool TryConsumeOneUseDamageShield(GameEngine.IDamageAttempt attack, int amount)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (amount <= 0 || target.OneUseDamageShields.FirstOrDefault()is not { } shield)
            return false;
        var source = (shield.Marker, shield.OwnerSeat);
        var count = target.MarkerSourceCounts.GetValueOrDefault(source);
        var total = target.Markers.GetValueOrDefault(shield.Marker);
        if (shield.TargetSeat != target.Seat || count <= 0 || total <= 0)
            throw new InvalidOperationException("The independent damage shield lost its paid marker.");
        target.OneUseDamageShields.RemoveAt(0);
        if (count == 1)
            target.MarkerSourceCounts.Remove(source);
        else
            target.MarkerSourceCounts[source] = count - 1;
        if (total == 1)
            target.Markers.Remove(shield.Marker);
        else
            target.Markers[shield.Marker] = total - 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(attack.ResolutionId, target.Seat, shield.Marker, -1, total - 1, shield.OwnerSeat, "program.one-use-damage-shield.consume"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OneUseDamageShieldConsumedEvent(attack.ResolutionId, shield, amount));
        _coreMatchLogOperations.AddLog("DamagePrevented", $"{(target.Name)} 移除“{(PlayerMarkerCatalog.GetDisplayName(shield.Marker))}”并防止{(amount)}点伤害。", attack.SourceSeat, target.Seat);
        return true;
    }

    internal int IndependentMarkerCount(CharacterState target, PlayerMarkerKind marker, int source) => target.OneUseDamageShields.Count(s => s.Marker == marker && s.OwnerSeat == source);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 650)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 790)]
    internal bool CanContinuePaidHpDamageShield(ProgramSkillFrame frame)
    {
        if (frame.HpDamageShieldReceipt is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PayHpToGrantOneUseDamageShield)]
    internal PromptChoice SelectAiHpDamageShield(PendingDecision decision)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var count = Math.Max(1, Math.Min(2, owner.Hp - 1));
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var hint = new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)
        {
            TargetValueAdjustment = 28d
        };
        return decision.Choices.Where(c => c.Targets.Count <= count).OrderByDescending(c => c.Targets.Sum(s => _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(view, s, hint)) - c.Targets.Count * 22d).ThenBy(c => c.Targets.Count).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    internal double GetHpDamageShieldAiTargetValue(CharacterState owner)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var hint = new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)
        {
            TargetValueAdjustment = 28d
        };
        // The optional trigger evaluates the same public relation and HP price
        // as its later quantity choice. The known enemy Lord cannot become a
        // beneficial target merely because the owner has HP to spend.
        return view.Players.Where(p => p.IsAlive && p.Seat != owner.Seat).Select(p => _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(view, p.Seat, hint)).DefaultIfEmpty(-1000d).Max();
    }

    SkillProgramStepOutcome IHpDamageShieldProgramHost.PayHpToGrantOneUseDamageShield(ProgramSkillFrame frame, PlayerMarkerKind marker) => BeginHpDamageShieldPayment(frame, marker);
    [ProgramDamageHook(ProgramDamagePhase.BeforeProgramWindow, 20)]
    internal ProgramDamageInterception InterceptOneUseDamageShield(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => TryConsumeOneUseDamageShield(attack, amount) ? ProgramDamageInterception.Prevented : ProgramDamageInterception.Continue;
}
