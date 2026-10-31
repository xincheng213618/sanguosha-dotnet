#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvQianOlRuntime
{
    internal const string ZengdaoDepositReason = "skill-program.zengdao.deposit", ZengdaoConsumeReason = "skill-program.zengdao.consume";
    internal IReadOnlyList<PromptChoice> ZengdaoDamageChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.LvQian!.Choices.Select((entry, index) => new PromptChoice(new($"zengdao.{(f.Id)}.{(entry.CardId)}"), $"移去{(_coreAdvancedLifecycleOperations.GetAdvancedCard(entry.CardId).DisplayName)}", [entry.CardId], [], new Dictionary<string, string> { { "program-action", "zengdao-damage" }, { "choice", index.ToString() } })).ToArray());
    internal void PublishZengdaoChoice(ProgramSkillFrame f)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "赠刀：选择移去一张实际赠刀牌。", f.LvQian!.Choices.Select(c => c.CardId).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = ZengdaoDamageChoices(f),
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage)]
    internal PromptChoice SelectZengdaoDamageChoice(ProgramSkillFrame f) => ZengdaoDamageChoices(f).First();
    [ProgramAnswer("zengdao-damage")]
    internal void ResolveZengdaoDamageChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        if (f.LvQian is not { Stage: LvQianStage.DamageChoice } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ZengdaoDamageChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zengdao payment lost its real recipient chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        PayZengdaoDamage(f, r.Choices[int.Parse(choice.Parameters["choice"])]);
    }

    internal void PayZengdaoDamage(ProgramSkillFrame f, ZengdaoEntityChoice choice)
    {
        var r = f.LvQian!;
        var grant = choice.Grant;
        var cardId = choice.CardId;
        if (r.Stage != LvQianStage.DamageChoice || !r.Choices.Contains(choice) || !IsDurableZengdaoGrant(grant) || _cardZonesOperations._cardZones.GetLocation(cardId) != grant.Location || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame damageWindow || damageWindow.Id != r.WindowId || damageWindow.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, IsDelayedJudgmentDamage: false } attack || attack.ResolutionId != r.DamageAttemptResolutionId || attack.SourceSeat != f.OwnerSeat || attack.TargetSeat != r.Targets.Single())
            throw new InvalidOperationException("Zengdao cannot substitute a grant, physical card or actual damage after the recipient chooses.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = r with { Stage = LvQianStage.DamagePaymentChildren, Before = before, After = before, Grant = grant, Entities = [cardId] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), grant.Location, CardLocation.DiscardPile, new(ZengdaoConsumeReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.LvQian!with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualCount = 1
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvQian = r });
        attack.IncreaseFinalizedDamageAmount(1);
        _coreRuntimeOperations.ReplaceRuntimeFrame(damageWindow.Id, damageWindow with { Amount = checked(damageWindow.Amount + 1) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZengdaoDamagePaidEvent(f.Id, attack.ResolutionId, grant.Source, f.OwnerSeat, damageWindow.TargetSeat, cardId, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        IssueLvQianSegment(f);
    }
}
