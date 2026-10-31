#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConversionStatesEntries
{
    internal void InitializeStructuredConversionSkills() => _resolve().InitializeStructuredConversionSkills();
    internal void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId) => _resolve().RegisterTaggedConversionSkill(player, skillId);
}
