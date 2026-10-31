#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramChoicesRuntime
{
    internal SkillProgramStepOutcome ChooseProgramOption(ProgramSkillFrame frame, int chooserSeat, string resultBind, IReadOnlyList<SkillProgramChoiceOption> options) => _programPaidCardOperations.Get().ChooseProgramOption(frame, chooserSeat, resultBind, options);
    internal void ResolveProgramOptionChoice(PromptChoice selected) => _programPaidCardOperations.Get().ResolveProgramOptionChoice(selected);
    internal bool IsClaimableProgramOptionStillAvailable(PromptChoice selected) => _programPaidCardOperations.Get().IsClaimableProgramOptionStillAvailable(selected);
    internal bool IsProgramOptionAvailable(ProgramSkillFrame frame, int chooserSeat, SkillProgramChoiceOption option) => _programPaidCardOperations.Get().IsProgramOptionAvailable(frame, chooserSeat, option);
    internal int CountChooserProgramBoundCards(ProgramSkillFrame frame, string bind, int chooserSeat) => _programPaidCardOperations.Get().CountChooserProgramBoundCards(frame, bind, chooserSeat);
    internal static int ResolveProgramEffectTarget(ProgramSkillFrame frame, SkillProgramEffectTarget target) => RuntimeSkills.RuntimeRules_Core_ProgramChoices.ResolveProgramEffectTarget(frame, target);
    [ProgramAiChoice(SkillProgramEffectOp.ChooseOption)]
    internal PromptChoice SelectAiProgramOption(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (_characterXuChuBoundaryMobileOperations.Value.TrySelectBoundaryXuChuAiOption(decision, frame, out var xuChuChoice))
            return xuChuChoice;
        var program = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        var effect = plan.GetPausedInstruction(frame.InstructionIndex).Effect;
        if (_characterPanZhangMaZhongBoundaryMobileOperations.Value.TrySelectBoundaryAnjianOption(decision, frame, out var anjianChoice))
            return anjianChoice;
        if (_coreShanheS5XuchuAiOperations.Value.TrySelectShanheS5LuoyiAiOption(decision, frame, plan, out var luoyiChoice))
            return luoyiChoice;
        var alternating = plan.Instructions.Skip(frame.InstructionIndex).FirstOrDefault(e => e.Op == SkillProgramEffectOp.ApplyAlternatingChoiceBenefit && e.SourceBind == effect.ResultBind);
        if (alternating is not null)
        {
            var last = _coreAlternatingBenefitsOperations.GetAlternatingChoicePredecessor(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, alternating);
            var preferred = last?.PendingOptionId == "draw" ? "targets" : "draw";
            return decision.Choices.First(c => c.Parameters.GetValueOrDefault("option-id") == preferred);
        }

        var owner = _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var chooser = _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]);
        var context = _coreProgramLifecycleOperations.CreateProgramAiPublicContext(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])with
        {
            SelectedTarget = frame.SelectedTargetSeats.Count == 1 ? _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[frame.SelectedTargetSeats[0]]) : null,
            Actor = frame.WindowContext?.CardUse is { } card ? _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[card.ActorSeat]) : null,
            BooleanState = stateId => _coreProgramPersistentStateOperations.GetProgramBooleanState2(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId),
            PindianWon = bind => frame.PindianResultBindings.SingleOrDefault(item => item.Name == bind)?.SourceWon ?? false,
            PindianOutcome = bind => frame.PindianResultBindings.SingleOrDefault(item => item.Name == bind)?.SourceWon,
            HasClaimableDamageCards = effect.Options.Any(option => option.Condition.ContainsHasClaimableDamageCards()) && _coreProgramLifecycleOperations.GetClaimableProgramDamageCards(frame).Length > 0,
            HasOwnedCardCategory = (zones, categories, kinds) => (zones.Contains(CardZoneKind.Equipment) && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]).Any(card => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardFilter(card.Kind, categories, kinds))) || (zones.Contains(CardZoneKind.Hand) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]).Any(card => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardFilter(card.Kind, categories, kinds)))
        };
        return decision.Choices.Select(choice =>
        {
            var facts = context with
            {
                ChoiceResult = bind => bind == choice.Parameters["result-bind"] ? choice.Parameters["option-id"] : frame.ChoiceBindings.SingleOrDefault(item => item.Name == bind)?.OptionId
            };
            var score = ProgramChoiceAi.Score(plan.Instructions.Skip(frame.InstructionIndex), owner, chooser, facts, bind => TryCountChooserProgramBoundCards(frame, bind, decision.PlayerSeat));
            return (Choice: choice, Score: score);
        }).OrderByDescending(item => item.Score).ThenBy(item => item.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    }

    internal int? TryCountChooserProgramBoundCards(ProgramSkillFrame frame, string bind, int chooserSeat) => _programPaidCardOperations.Get().TryCountChooserProgramBoundCards(frame, bind, chooserSeat);
}
