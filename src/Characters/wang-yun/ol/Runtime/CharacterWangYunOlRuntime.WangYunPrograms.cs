#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYunOlRuntime
{
    internal const string WangYunPlanMoveReason = "program.wang-yun.plan-payment";
    internal static bool WangYunMaterial(Card card) => RuntimeSkills.RuntimeRules_wang_yun_ol_WangYunPrograms.WangYunMaterial(card);
    internal bool WangYunActivationCardAllowed(SkillProgramActivation a, Card card) => WangYunActivationCardAllowed(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, a, card);
    internal bool WangYunActivationCardAllowed(int ownerSeat, SkillProgramActivation a, Card card)
    {
        var features = ProgramInstructionResolver.Default.Features(a);
        if (features.HasOperation(SkillProgramEffectOp.ResolveLianjiPaidWeaponAndSlash))
            return _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(ownerSeat) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(ownerSeat, card, CardLocation.Hand(ownerSeat), OwnedCardMoveIntent.Discard);
        return !features.HasOperation(SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial) || WangYunMaterial(card);
    }

    internal CardConversionSource WangYunSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramEffect? WangYunPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool WangYunActivationMatches(ProgramSkillFrame f, SkillProgramEffectOp op) => f.TriggerId is null && f.WindowContext is null && f.InstructionIndex == 1 && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is [var instruction] && instruction.Op == op;
    internal static Dictionary<string, string> WangYunArgs(ProgramSkillFrame f, string branch) => RuntimeSkills.RuntimeRules_wang_yun_ol_WangYunPrograms.WangYunArgs(f, branch);
    internal int WangYunChooser(ProgramSkillFrame f) => f.WangYunPlan?.Stage == "branch" ? f.WangYunPlan.Actor : f.OwnerSeat;
    internal void PublishWangYun(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.WangYunPlan is { } r && !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishWangYun(f);
            return;
        }

        var choices = f.WangYunPlan is not null ? WangYunPlanChoices(f) : WangYunTrickChoices(f);
        if (choices.Count == 0)
        {
            FinishWangYun(f);
            return;
        }

        var chooser = WangYunChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：选择本次真实结算的后续效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("wang-yun-plan")]
    internal void ResolveWangYunChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.WangYunPlan is null && f.WangYunTrick is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != WangYunChooser(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], (f.WangYunPlan is not null ? WangYunPlanChoices(f) : WangYunTrickChoices(f)).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Wang Yun lost the exact actual chooser or current material/target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (f.WangYunTrick is not null)
        {
            IssueWangYunTrick(f, selected);
            return;
        }

        var r = f.WangYunPlan!;
        switch (selected.Parameters["branch"])
        {
            case "target":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunPlan = r with { Stage = "branch", SlashTarget = selected.Targets.Single() } });
                PublishWangYun(f);
                return;
            case "transfer":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunPlan = r with { Stage = "transfer" } });
                PublishWangYun(f);
                return;
            case "give-weapon":
                var card = selected.Cards.Single();
                var recipient = selected.Targets.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunPlan = r with { Recipient = recipient } });
                PayWangYunPlanMovement(f, card, CardLocation.Equipment(r.Actor), CardLocation.Hand(recipient), "complete");
                return;
            case "slash":
                IssueWangYunSlash(f, selected);
                return;
            default:
                throw new InvalidOperationException("Unknown paid plan choice.");
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22900)]
    internal bool ResumeWangYun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.WangYunPlan is null && f.WangYunTrick is null && f.WangYunAwakening is null)
            return false;
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnWangYunMovement(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.WangYunAwakening is not null || f.WangYunTrick is { NativeReturned: true })
        {
            FinishWangYun(f);
            return true;
        }

        if (f.WangYunPlan is { } r && r.NativeReturned)
        {
            if (r.NativeUse!.Kind == "slash")
            {
                FinishWangYun(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunPlan = r with { Stage = "target", NativeUse = null, NativeReturned = false, IssuingIds = Array.Empty<int>(), IssuingKind = null } });
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.WangYunPlan is { } p && !_coreMatchParticipantsPlayersAccess._players[p.Actor].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishWangYun(f);
            return true;
        }

        if (f.WangYunPlan?.Stage == "weapon")
        {
            IssueWangYunWeapon(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishWangYun(f);
        return true;
    }
    internal bool IsWangYunMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveLianjiPaidWeaponAndSlash && f.WangYunPlan is { Stage: "movement" } && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 22900)]
    internal bool ReturnWangYunMovement(ProgramSkillFrame f)
    {
        if (f.WangYunPlan is not { } r)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsWangYunMovement(f, WangYunPausedEffect(f), p))
            throw new InvalidOperationException("Lianji consumed a foreign movement return.");
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, WangYunPlan = r with { Stage = r.ReturnStage } });
        if (r.ReturnStage == "complete" || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive)
            FinishWangYun(f);
        else
            IssueWangYunWeapon(f);
        return true;
    }

    internal void FinishWangYun(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangYunPlan = null, WangYunTrick = null, WangYunAwakening = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome IWangYunProgramHost.ResolveLianjiPaidWeaponAndSlash(ProgramSkillFrame f) => ResolveLianjiPaidWeaponAndSlash(f);
    SkillProgramStepOutcome IWangYunProgramHost.AwakenFromIssuedLianjiSlashDamage(ProgramSkillFrame f) => AwakenFromIssuedLianjiSlashDamage(f);
    SkillProgramStepOutcome IWangYunProgramHost.UseThreeRandomTricksFromSelectedMaterial(ProgramSkillFrame f) => UseThreeRandomTricksFromSelectedMaterial(f);
}
