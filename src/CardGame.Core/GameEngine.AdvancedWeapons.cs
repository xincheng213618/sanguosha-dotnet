namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.HashSet<int> _permanentlyRemovedWeapons => ref _advancedWeapons.ReferenceToPermanentlyRemovedWeapons;
    private ref readonly global::System.Collections.Generic.HashSet<int> _generatedPhysicalCardIds => ref _advancedWeapons.ReferenceToGeneratedPhysicalCardIds;
    private ref readonly global::System.Collections.Generic.HashSet<string> _usedGeneralWeaponSources => ref _advancedWeapons.ReferenceToUsedGeneralWeaponSources;
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<string>> _copiedWeaponSkills => ref _advancedWeapons.ReferenceToCopiedWeaponSkills;
}
