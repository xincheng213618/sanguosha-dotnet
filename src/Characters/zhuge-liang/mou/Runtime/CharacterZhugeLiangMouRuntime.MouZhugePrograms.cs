#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal const string MouWujingUsage = "first-nonhand-discard";
    internal bool HasMouOperation(CharacterState owner, SkillProgramEffectOp op) => _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(program => program.Activations.Any(a => ProgramInstructionResolver.Default.Features(a).HasOperation(op)) || program.Triggers.Any(t => ProgramInstructionResolver.Default.Features(t).HasOperation(op)));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 180)]
    internal void ObserveMouZhugeEvent(IGameEvent payload)
    {
        if (payload is TurnStartedEvent)
            _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances.Clear();
        if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } responded && responded.Action.ActorSeat == responded.Action.ProviderSeat && responded.Action.RequesterSeat is null && (responded.Action.EffectiveKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[responded.Action.ActorSeat], responded.Action.EffectiveKind)))
            _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances.Remove(responded.Action.ActorSeat);
        if (payload is CardUseDeclaredEvent declared && HasMouWujingAllowance(_coreMatchParticipantsPlayersAccess._players[declared.SourceSeat]))
        {
            _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances.Remove(declared.SourceSeat);
            _coreCardUseLifecycleOperations.TryMarkUnlimitedUse(declared.ResolutionId);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(declared.ResolutionId, use => use with { QuotaOnlyUseLimitExemption = true });
        }
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 30)]
    internal bool CanStartMouZhuge(CharacterState owner, ProgramExecutionPlan plan)
    {
        if (plan.Features.HasOperation(SkillProgramEffectOp.MouZhitianView))
            return owner.IsHuman && MouZhitianTop(owner.Seat).Count > 0;
        if (!plan.Features.HasOperation(SkillProgramEffectOp.MouZhijueUse))
            return true;
        if (MouZhijueYang(owner.Seat))
            return MouYangOptions(owner).Count > 0;
        return new[]
        {
            true,
            false
        }.Any(red => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(card => CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)) == red) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, "mou:zhijue", "color:" + red, SkillUsageScope.Turn) == 0);
    }

    SkillProgramStepOutcome IMouZhugeProgramHost.ExecuteMouZhuge(ProgramSkillFrame frame, SkillProgramEffect effect) => ExecuteMouZhuge(frame, effect);
    internal SkillProgramStepOutcome ExecuteMouZhuge(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (effect.Op == SkillProgramEffectOp.MouWujingDraw)
        {
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, MouWujingUsage, SkillUsageScope.Turn, 1))
                return SkillProgramStepOutcome.Continue;
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], 1, log: true, reason: new("mou.wujing.draw"));
            return SkillProgramStepOutcome.Continue;
        }

        if (effect.Op == SkillProgramEffectOp.MouZhitianView)
        {
            var cards = MouZhitianTop(frame.OwnerSeat);
            PublishMouPrompt(frame, [new(new($"mou-top.{(frame.Id)}.confirm"), "已观看牌堆顶，继续出牌", [], [], new Dictionary<string, string> { ["program-action"] = "mou-zhitian-confirm" })], "牌堆顶按从顶至底显示：" + string.Join("、", cards.Select(card => $"{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))}{(card.RankText)}【{(card.DisplayName)}】")));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MouZhijue = new(MouZhijueYang(frame.OwnerSeat) ? "yang" : "color", MouZhijueYang(frame.OwnerSeat), [], []) });
        ResumeMouZhijue(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }
}
