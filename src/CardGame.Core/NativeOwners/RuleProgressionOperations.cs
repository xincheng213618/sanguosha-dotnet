#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuleProgressionOperations(KernelQueueGameEventCall queueGameEvent, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, KernelPublishStateCall publishState, Func<SkillStateApplications?> skillStates, Func<IAppliedResponseRules> responseRules, Func<IProgramRuntimeFrames> frames) : IRuleProgressionOperations
{
    public void CaptureNativeResponseRules(IGameEvent payload)
    {
        // Actionless legacy producers declare their genuine native frame here;
        // no ActionId is manufactured for them.
        if (payload is CardUseDeclaredEvent declared &&
            frames().OfType<CardUseFrame>().SingleOrDefault(use => use.Id == declared.ResolutionId) is { Action: null } use)
            responseRules().CaptureNativeUse(use, declared);
    }
    public void ApplySkillStateDeclarations(IGameEvent payload, SkillStateBoundaryTiming timing) => skillStates()?.ApplyEvent(payload, timing);
    public void QueueFact(IGameEvent payload) => queueGameEvent(payload);
    public bool BeginCharacterStateWindow() => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow();
    public bool BeginHpChangedWindow() => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow();
    public bool BeginCardsMovedWindow() => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow();
    public void RequestStatePublication() => publishState();
}
