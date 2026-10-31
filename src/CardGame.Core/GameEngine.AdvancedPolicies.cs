namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.HashSet<int> _nextUnlimitedCardOwners => ref _advancedPolicies.ReferenceToNextUnlimitedCardOwners;
}
