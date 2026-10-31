#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSanshouArmorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> _resolve;
    private CoreGameEngineShanheSanshouArmorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSanshouArmorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> resolve) => new(resolve);
}
