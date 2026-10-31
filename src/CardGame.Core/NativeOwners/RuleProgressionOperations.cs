#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuleProgressionOperations(KernelQueueGameEventCall queueGameEvent, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, KernelPublishStateCall publishState) : IRuleProgressionOperations
{
    public void QueueFact(IGameEvent payload) => queueGameEvent(payload);
    public bool BeginCharacterStateWindow() => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow();
    public bool BeginHpChangedWindow() => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow();
    public bool BeginCardsMovedWindow() => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow();
    public void RequestStatePublication() => publishState();
}
