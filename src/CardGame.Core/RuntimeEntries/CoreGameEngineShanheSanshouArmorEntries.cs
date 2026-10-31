#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSanshouArmorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> _resolve;
    private CoreGameEngineShanheSanshouArmorEntries(Func<EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSanshouArmorEntries Create(Func<EngineOwners.CoreGameEngineShanheSanshouArmorRuntime> resolve) => new(resolve);
}
