#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal IEnumerable<(PublicPersistentPileSource Pile, Card Card)> YouqueBaits(int owner) => XieweiPiles(owner).SelectMany(p => _cardZonesOperations._cardZones.CardsAt(p.Location).Select(c => (Pile: p, Card: c)));
    internal bool YouquePhaseActive(int owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<YouquePhaseDisabledEvent>().Any(e => e.OwnerSeat == owner && e.Phase == p);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27579, SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian)]
    internal bool CanActivateYouque(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(c.Owner) && YouquePhaseActive(c.OwnerSeat) && YouqueBaits(c.OwnerSeat).Any() && _coreMatchParticipantsPlayersAccess._players.Any(p => _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(c.OwnerSeat, p.Seat));
    internal IReadOnlyList<PromptChoice> YouqueBaitChoices(ProgramSkillFrame f)
    {
        var baits = YouqueBaits(f.OwnerSeat).ToArray();
        if (baits.Length == 0)
            return[];
        var rank = baits.Min(b => b.Card.Rank);
        return Array.AsReadOnly(baits.Where(b => b.Card.Rank == rank).Select(b => new PromptChoice(new($"youque.{(f.Id)}.bait.{(b.Card.Id)}"), $"以饵【{(b.Card.DisplayName)}】（{(b.Card.Rank)}）拼点", [b.Card.Id], [], new Dictionary<string, string> { ["program-action"] = "guan-hai", ["branch"] = "bait" })).Append(new(new($"youque.{(f.Id)}.cancel"), "取消诱阙", [], [], new Dictionary<string, string> { ["program-action"] = "guan-hai", ["branch"] = "cancel" })).ToArray());
    }

    internal void PublishYouque(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !YouquePhaseActive(f.OwnerSeat) || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(f.OwnerSeat, f.GuanHai!.OpponentSeat) || !YouqueBaits(f.OwnerSeat).Any())
        {
            FinishGuanHai(f, false);
            return;
        }

        var choices = YouqueBaitChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "诱阙：选择点数最小的饵拼点。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal static string YouqueResultBind(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_guan_hai_ol_Youque.YouqueResultBind(f);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveYouqueIssuedSlashDamageBenefit)]
    internal void ReturnYouqueDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.GuanHai is not { Paid: true, Stage: GuanHaiStage.RewardDraw } r || f.NativeDraw is not null || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != 2 || o.SequenceBefore < r.Before || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || o.SequenceAfter < o.SequenceBefore || o.ActualDrawIds.Count > 2 || o.Replacement is not null && o.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YouqueRewardDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Youque lost its exact once-issued native Draw2 return.");
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == YouqueDrawReason).ToArray();
        if (!moves.Select(m => m.CardId).Order().SequenceEqual(o.ActualDrawIds.Order()))
            throw new InvalidOperationException("Youque cannot count reshuffle or a replacement as a drawn entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanHai = r with { Stage = GuanHaiStage.RewardChildren, Before = o.SequenceBefore, After = o.SequenceAfter, ActualDrawCount = o.ActualDrawIds.Count }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YouqueRewardDrawReturnedEvent(f.Id, r.NativeUseId, r.DamageFrameId, f.OwnerSeat, 2, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter, o.Replacement is not null));
        ContinueGuanHai(f.Id);
    }
}
