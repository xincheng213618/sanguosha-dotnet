#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramNativeSlashQueriesEntries
{
    internal bool IsNativeForcedSlashUseFrontier(global::CardGame.Core.CharacterState owner) => _resolve().IsNativeForcedSlashUseFrontier(owner);
    internal bool IsNativePureProviderFrontier(int actor) => _resolve().IsNativePureProviderFrontier(actor);
}
