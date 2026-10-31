#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuleProgressionOperations(global::CardGame.Core.NativeOwners.KernelQueueGameEventCall queueGameEvent, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::CardGame.Core.NativeOwners.KernelPublishStateCall publishState) : IRuleProgressionOperations
{
    public void QueueFact(IGameEvent payload) => queueGameEvent(payload);
    public bool BeginCharacterStateWindow() => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow();
    public bool BeginHpChangedWindow() => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow();
    public bool BeginCardsMovedWindow() => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow();
    public void RequestStatePublication() => publishState();
}
