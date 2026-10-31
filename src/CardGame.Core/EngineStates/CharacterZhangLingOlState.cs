#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhangLingOlState : Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess
{
    private bool _synchronizingCategoryTalismans = default !;
    internal ref bool ReferenceToSynchronizingCategoryTalismans => ref _synchronizingCategoryTalismans;

    ref bool Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess._synchronizingCategoryTalismans => ref _synchronizingCategoryTalismans;
}
