#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuleProgressionOperations(KernelQueueGameEventCall queueGameEvent, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, KernelPublishStateCall publishState, Func<SkillStateApplications?> skillStates) : IRuleProgressionOperations
{
    public void ApplySkillStateDeclarations(IGameEvent payload, SkillStateBoundaryTiming timing) => skillStates()?.ApplyEvent(payload, timing);
    public void QueueFact(IGameEvent payload) => queueGameEvent(payload);
    public bool BeginCharacterStateWindow() => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow();
    public bool BeginHpChangedWindow() => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow();
    public bool BeginCardsMovedWindow() => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow();
    public void RequestStatePublication() => publishState();
}
