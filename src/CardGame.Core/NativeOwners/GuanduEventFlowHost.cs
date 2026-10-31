#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanduEventFlowHost(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGuanduEventProgramsEntries> getCoreGuanduEventProgramsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.CoreGameEngineProgramNativeDrawEntries> getCoreGameEngineProgramNativeDrawRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineActivityBattleRulesRuntime>> getCoreGameEngineActivityBattleRulesRuntime) : IGuanduEventFlowHost
{
    public long? DamageRewardUse(long frameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        return frame.WindowContext is { } context ? getCoreGuanduEventProgramsRuntime().GuanduDamageRewardUse(frame.OwnerSeat, context)?.Id : null;
    }

    public void IssueRepeat(long frameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        var context = frame.WindowContext ?? throw new InvalidOperationException("A repeated card lost its exact program context.");
        var use = getCoreGuanduEventProgramsRuntime().GuanduTenthUse(frame.OwnerSeat, context) ?? throw new InvalidOperationException("The tenth-card event lost its actual use.");
        var window = getFrameStore().OfType<ProgramCardTriggerWindowFrame>().Single(item => item.Id == context.ParentFrameId);
        var source = new CardConversionSource(frame.SkillId, EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(use.Id, use with { ExtraCardEffect = new() { Source = source, ProgramFrameId = frame.Id, ActionId = use.Action!.ActionId, AutomaticGameplayHash = frame.GameplayHash, Simple = window.SimpleContinuation, Trick = window.TrickContinuation } });
        advanceEventRulesAndQueueFact(new AutomaticExtraCardEffectIssuedEvent(frame.Id, source, use.Id, use.Action!.ActionId, frame.GameplayHash));
    }

    public void IssueFlow(long frameId, SkillProgramEffectOp operation, long? useFrameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        if (frame.GuanduEventFlow is not null)
            throw new InvalidOperationException("The event flow was already issued.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { GuanduEventFlow = new(operation, actualTurnProgressState.ReferenceToTurnNumber, useFrameId) });
        advanceEventRulesAndQueueFact(new GuanduEventFlowIssuedEvent(frameId, operation, frame.OwnerSeat, actualTurnProgressState.ReferenceToTurnNumber, useFrameId));
    }

    public void BeginRewardDraw(long frameId, int ownerSeat)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        if (frame.OwnerSeat != ownerSeat || frame.GuanduEventFlow is not { Operation: SkillProgramEffectOp.RewardGuanduDamage, UseFrameId: { } useFrameId })
            throw new InvalidOperationException("The event reward lost its issued native use receipt.");
        advanceEventRulesAndQueueFact(new GuanduDamageRewardedEvent(useFrameId, ownerSeat));
        getCoreGameEngineProgramNativeDrawRuntime().BeginOperationProgramDraw(frame, ownerSeat, 1, new("guandu.grain-shortage.draw"));
    }

    public bool LoseOwnerHp(long frameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        return createProgramSkillHost().LoseHp(frameId, frame.SkillId, frame.OwnerSeat, 1) == SkillProgramStepOutcome.AwaitChild;
    }

    public ModeGuanduFlowCursor? Current(long frameId) => getFrameStore().LastOrDefault()is ProgramSkillFrame { GuanduEventFlow: { } receipt } frame && frame.Id == frameId ? new(frameId, frame.OwnerSeat, receipt, frame.NativeDraw is not null) : null;
    public bool DrainChildren(long frameId) => getCoreGameEngineActivityBattleRulesRuntime().Value.DrainActivityRuleChildren(frameId);
    public void ClearFlow(long frameId)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        if (frame.GuanduEventFlow is null)
            throw new InvalidOperationException("The event continuation lost its issued receipt.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { GuanduEventFlow = null });
    }
}
