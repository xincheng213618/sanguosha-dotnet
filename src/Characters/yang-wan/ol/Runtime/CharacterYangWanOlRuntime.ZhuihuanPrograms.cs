#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangWanOlRuntime
{
    internal SkillProgramStepOutcome ZhuihuanProgramArm(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active))
            return SkillProgramStepOutcome.Continue;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => player.Seat).ToArray();
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(seat => new PromptChoice(new ChoiceId($"zhuihuan-arm.frame-{(frame.Id)}.seat-{(seat)}"), $"秘密选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", [], [seat], new Dictionary<string, string> { ["program-action"] = "zhuihuan-arm", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["chosen"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请秘密选择一名角色。", [], candidates, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 秘密选择", "直到该角色的下个准备阶段，此期间内对其造成过伤害的角色将在该准备阶段受到清算。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("zhuihuan-arm")]
    internal void ResolveZhuihuanArmChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zhuihuan arm choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZhuihuanArm } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Zhuihuan arm choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || !int.TryParse(selected.Parameters.GetValueOrDefault("chosen"), System.Globalization.CultureInfo.InvariantCulture, out var chosen) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(chosen) || !_coreMatchParticipantsPlayersAccess._players[chosen].IsAlive)
            throw new InvalidOperationException("The Zhuihuan arm choice lost its chooser or candidate.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _characterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices[active.Id] = chosen;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramZhuihuanArmedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，秘密选择了一名角色。", active.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome ZhuihuanProgramRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } boundarySeat } || boundarySeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return SkillProgramStepOutcome.Continue;
        var chosenSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;
        var state = frame.ZhuihuanRetaliate;
        if (state is null || state.ChosenSeat != chosenSeat)
        {
            var damagers = CollectZhuihuanPendingDamagers(owner.Seat, chosenSeat);
            if (damagers.Count == 0)
                return SkillProgramStepOutcome.Continue;
            state = new ZhuihuanRetaliateState(chosenSeat, damagers);
        }

        var chosen = _coreMatchParticipantsPlayersAccess._players[state.ChosenSeat];
        while (state.PendingDamagers.Count > 0)
        {
            var damagerSeat = state.PendingDamagers[0];
            state = state with
            {
                PendingDamagers = state.PendingDamagers.Skip(1).ToArray()
            };
            if (!_coreMatchParticipantsPlayersAccess._players[damagerSeat].IsAlive)
                continue;
            if (!chosen.IsAlive)
            {
                _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】因所选角色已死亡，剩余清算取消。", active.OwnerSeat, chosenSeat);
                break;
            }

            var damager = _coreMatchParticipantsPlayersAccess._players[damagerSeat];
            if (damager.Hp > chosen.Hp)
            {
                // Keep the committed cursor while the damage child is active;
                // the typed receipt resumes this instruction after it returns.
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { ReexecuteParticipantInstruction = true, ZhuihuanRetaliate = state with { DamagedSeats = state.DamagedSeats.Append(damagerSeat).ToArray() } });
                return _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), damagerSeat, 2, explicitSourceSeat: state.ChosenSeat);
            }

            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(damager);
            var count = Math.Min(2, hand.Count);
            if (count > 0)
            {
                var picked = new List<Card>();
                var pool = hand.ToList();
                for (var index = 0; index < count; index++)
                {
                    var card = pool[_randomOperations._random.Next(pool.Count)];
                    pool.Remove(card);
                    picked.Add(card);
                }

                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(active.OwnerSeat, damagerSeat, picked, CardLocation.Hand(damagerSeat), new($"skill-program.{(active.SkillId)}.zhuihuan-discard"));
            }

            _coreMatchLogOperations.AddLog("SkillEffect", count > 0 ? $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】：{(_coreMatchParticipantsPlayersAccess._players[damagerSeat].Name)} 的体力值不大于 {(_coreMatchParticipantsPlayersAccess._players[chosenSeat].Name)}，随机弃置 {(count)} 张手牌。" : $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】：{(_coreMatchParticipantsPlayersAccess._players[damagerSeat].Name)} 没有手牌可弃置。", active.OwnerSeat, damagerSeat);
            state = state with
            {
                DiscardedSeats = state.DiscardedSeats.Append(damagerSeat).ToArray(),
                DiscardedCardCount = state.DiscardedCardCount + count
            };
        }

        var pendingArms = PendingZhuihuanArmFrameIds(owner.Seat, chosenSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { ZhuihuanRetaliate = null, ReexecuteParticipantInstruction = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramZhuihuanResolvedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, chosenSeat, pendingArms, state.DamagedSeats, state.DiscardedSeats, state.DiscardedCardCount));
        foreach (var armFrameId in pendingArms)
            _characterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices.Remove(armFrameId);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
    internal IReadOnlyList<int> CollectZhuihuanPendingDamagers(int ownerSeat, int chosenSeat)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var order = new List<long>();
        var ledgers = new Dictionary<long, List<int>>();
        var resolved = new HashSet<long>();
        foreach (var fact in facts)
        {
            switch (fact)
            {
                case ProgramZhuihuanArmedEvent armed when armed.OwnerSeat == ownerSeat && _characterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices.TryGetValue(armed.FrameId, out var armedSeat) && armedSeat == chosenSeat:
                    order.Add(armed.FrameId);
                    ledgers[armed.FrameId] = [];
                    break;
                case ProgramZhuihuanResolvedEvent resolution when resolution.OwnerSeat == ownerSeat:
                    foreach (var armFrameId in resolution.ArmFrameIds)
                    {
                        resolved.Add(armFrameId);
                        ledgers.Remove(armFrameId);
                    }

                    break;
                case DamageAppliedEvent damage when damage.TargetSeat == chosenSeat && damage.Amount > 0 && !damage.SourceLess:
                    foreach (var ledger in ledgers.Values)
                        if (!ledger.Contains(damage.SourceSeat))
                            ledger.Add(damage.SourceSeat);
                    break;
            }
        }

        return order.Where(armFrameId => !resolved.Contains(armFrameId)).SelectMany(armFrameId => ledgers[armFrameId]).ToArray();
    }

    internal IReadOnlyList<long> PendingZhuihuanArmFrameIds(int ownerSeat, int chosenSeat)
    {
        var resolved = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramZhuihuanResolvedEvent>().Where(resolution => resolution.OwnerSeat == ownerSeat).SelectMany(resolution => resolution.ArmFrameIds).ToHashSet();
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramZhuihuanArmedEvent>().Where(armed => armed.OwnerSeat == ownerSeat && !resolved.Contains(armed.FrameId) && _characterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices.TryGetValue(armed.FrameId, out var armedSeat) && armedSeat == chosenSeat).Select(armed => armed.FrameId).ToArray();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ZhuihuanArm)]
    internal PromptChoice SelectAiZhuihuanArmChoice(PendingDecision decision) => decision.Choices.OrderByDescending(choice => choice.Targets.Contains(decision.PlayerSeat)).ThenBy(choice => choice.Targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Hp).FirstOrDefault()).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
}
