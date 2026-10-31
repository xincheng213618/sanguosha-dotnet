#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhangLingOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess
{
    private bool _synchronizingCategoryTalismans = default !;
    internal ref bool ReferenceToSynchronizingCategoryTalismans => ref _synchronizingCategoryTalismans;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess._synchronizingCategoryTalismans => ref _synchronizingCategoryTalismans;
}
