#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiRuntime
{
    internal static ProgramAiPublicContext WithPhaseHandExtraTurnTargetPricing(ProgramAiPublicContext context, SkillProgram program, SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_Core_PhaseHandExtraTurnAi.WithPhaseHandExtraTurnTargetPricing(context, program, trigger);
    internal ProgramAiEstimate IncludePhaseHandExtraTurnDeclarationForAi(ProgramAiEstimate baseline, CharacterState owner, string skillId, string skillInstanceId, SkillProgram program, SkillProgramTrigger declaration, ProgramAiPublicContext context, ProgramSkillWindowContext windowContext)
    {
        if (windowContext.Window != SkillProgramTriggerWindow.AfterNormalDraw || windowContext.OwnerSeat != owner.Seat || owner.Seat != _coreActualTurnProgressCurrentSeatAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault(parent => parent.Id == windowContext.ParentFrameId)is not { Window: SkillProgramTriggerWindow.AfterNormalDraw, Continuation: ProgramLifecycleContinuation.CompleteAfterNormalDraw } boundary || boundary.OwnerSeat != owner.Seat || !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, skillId, skillInstanceId) || !ProgramPhaseHandExtraTurnAi.TryGetPaymentAfterDeclaration(program, declaration, out var payment))
            return baseline;
        var publicOwner = _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
        if (publicOwner.HandCount < 1 || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, payment.Id, SkillUsageScope.Turn) > 0)
            return baseline;
        var stateId = declaration.Effects[1].StateId!;
        if (context.BooleanState?.Invoke(stateId) == true)
            return baseline;
        var targets = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat).Players.Where(target => target.IsAlive && target.Seat != owner.Seat).Select(target => target.Seat).ToArray();
        if (targets.Length == 0)
            return baseline;
        // Exactly one successor is evaluated. Its ordinary MoveBoundCards
        // estimate already prices the one hand-card cost, so do not add it twice.
        var futureContext = context with
        {
            PricePhaseHandExtraTurnTarget = true,
            BooleanState = name => name == stateId || context.BooleanState?.Invoke(name) == true
        };
        var future = _coreProgramLifecycleOperations.EstimateCompositionForAi(owner, payment.Effects, futureContext, targets, windowContext).Estimate;
        var opportunity = ProgramPhaseHandExtraTurnAi.PublicSkippedPlayOpportunity(publicOwner);
        var gain = future.Score - opportunity;
        return baseline with
        {
            Score = baseline.Score + gain,
            IsSelfLethal = baseline.IsSelfLethal || future.IsSelfLethal,
            Approximation = baseline.Approximation + $"同实例公开阶段声明关联一个弃手牌后继：已含一次卡价，最高公开受益估值 {(future.Score):0.##}，放弃出牌机会先验 {(opportunity):0.##}。"};
    }
}
