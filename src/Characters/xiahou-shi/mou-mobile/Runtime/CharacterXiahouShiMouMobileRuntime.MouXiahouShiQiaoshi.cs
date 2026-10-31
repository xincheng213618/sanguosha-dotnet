#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouShiMouMobileRuntime
{
    internal bool MouXiahouShiQiaoshiSpent(int owner, MouXiahouShiTurnKey turn) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouShiQiaoshiBranchAcceptedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MouXiahouShiComposition.Qiaoshi && e.Turn == turn && e.Branch == "recover-and-draw");
    [ProgramIssuedTrigger(60, ProgramIssuedTriggerMatch.RejectWhenFalse)]
    internal bool CanMouXiahouShiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == MouXiahouShiComposition.Yanyu && candidate.BindingId == "support-at-play-ending")
            return CurrentMouXiahouShiTurn()is { } turn && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == candidate.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && context.SourceSeat == candidate.OwnerSeat && context.Window == SkillProgramTriggerWindow.PlayEnding && MouXiahouShiTurnDiscards(candidate.OwnerSeat, turn) > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat);
        if (candidate.SkillId != MouXiahouShiComposition.Qiaoshi || candidate.BindingId != "foreign-damage-source-choice")
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == candidate.OwnerSeat && context.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && source != candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && context.Amount > 0 && CurrentMouXiahouShiTurn()is { } actual && !MouXiahouShiQiaoshiSpent(candidate.OwnerSeat, actual);
    }

    internal SkillProgramStepOutcome BeginMouXiahouShiQiaoshi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouXiahouShiQiaoshi is not null || !ExactMouXiahouShiDamageContext(f) || CurrentMouXiahouShiTurn()is not { } turn || f.WindowContext!.SourceSeat is not { } source || source == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || MouXiahouShiQiaoshiSpent(f.OwnerSeat, turn) || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Qiaoshi requires its exact native foreign after-damage occurrence and unused actual turn.");
        var w = (DamageTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(i => i.Id == f.WindowContext.ParentFrameId);
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(w);
        var r = new MouXiahouShiQiaoshiReceipt(MouXiahouShiSource(f), f.GameplayHash, turn, w.Id, w.ParentFrameId, attack.ResolutionId, source, f.WindowContext.Amount)
        {
            Stage = MouXiahouShiQiaoshiStage.SourceChoice,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiQiaoshi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiQiaoshiStartedEvent(f.Id, r.Source, r.GameplayHash, turn, r.WindowId, r.DamageFrameId, r.AttackResolutionId, source, r.DamageAmount));
        PublishMouXiahouShi(f, source, MouXiahouShiQiaoshiChoices(f), $"樵拾：是否令{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}回复{(r.DamageAmount)}点体力，然后你摸两张牌？");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouXiahouShiQiaoshiChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { ("recover-and-draw", $"令{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}回复{(f.MouXiahouShiQiaoshi!.DamageAmount)}点体力，然后摸两张牌"), ("decline", "不发动樵拾") }.Select(p => new PromptChoice(new($"mou-xiahou-shi.{(f.Id)}.qiaoshi.{(p.Item1)}"), p.Item2, [], [], new Dictionary<string, string> { ["program-action"] = "mou-xiahou-shi-qiaoshi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = p.Item1 })).ToArray());
    [ProgramAnswer("mou-xiahou-shi-qiaoshi")]
    internal void AnswerMouXiahouShiQiaoshi(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouXiahouShiQiaoshi!;
        if (!ValidMouXiahouShiQiaoshi(f) || r.Stage != MouXiahouShiQiaoshiStage.SourceChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.DamageSourceSeat || !_coreMatchParticipantsPlayersAccess._players[r.DamageSourceSeat].IsAlive || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXiahouShiQiaoshiChoices(f).Where(c => c.Id == selected.Id).ToArray()) || MouXiahouShiQiaoshiSpent(f.OwnerSeat, r.Turn))
            throw new InvalidOperationException("Qiaoshi requires its actual damage source's current unused private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var baseRecovery = branch == "recover-and-draw" && owner.IsAlive && owner.HasHpValue ? Math.Max(0, Math.Min(r.DamageAmount, owner.MaxHp - owner.Hp)) : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiQiaoshi = r with { Branch = branch, Stage = MouXiahouShiQiaoshiStage.RecoveryChildren, RequestedRecovery = branch == "recover-and-draw" ? r.DamageAmount : 0, BaseRecovery = baseRecovery, RecoveryHpBefore = owner.HasHpValue ? owner.Hp : 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiQiaoshiBranchAcceptedEvent(f.Id, r.Source, r.Turn, r.DamageSourceSeat, branch));
        if (branch == "decline")
        {
            FinishMouXiahouShi(f);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiQiaoshiRecoveryIssuedEvent(f.Id, r.DamageSourceSeat, f.OwnerSeat, r.DamageAmount, r.DamageAmount, owner.HasHpValue ? owner.Hp : 0, baseRecovery));
        // The native host preserves recovery replacement and real HP invoices. Its
        // source is the original damage source, independently of the skill owner.
        if (owner.IsAlive)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, r.DamageSourceSeat, f.OwnerSeat, r.DamageAmount, null, null);
        SealMouXiahouShiMovements(f.Id, r.Before);
        ContinueMouXiahouShi(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouXiahouShiQiaoshi)]
    internal PromptChoice AiMouXiahouShiQiaoshi(PendingDecision pending, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(pending.PlayerSeat);
        var q = f.MouXiahouShiQiaoshi!;
        var recovery = Math.Max(0, Math.Min(q.DamageAmount, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
        var value = _coreAiBrainsAiBrainsAccess._aiBrains[pending.PlayerSeat].ScoreProgramTarget(view, f.OwnerSeat, new(OwnerDraw: 0, OwnerRecovery: 0, OwnerHpLoss: 0, TargetDraw: 0, TargetRecovery: recovery, TargetHpLoss: 0, GivesSelected: false, DiscardsSelected: false));
        var branch = recovery == 0 || value >= -2 ? "recover-and-draw" : "decline";
        return pending.Choices.Single(c => c.Parameters["branch"] == branch);
    }
}
