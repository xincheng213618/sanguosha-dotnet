#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiDamageHealthEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> _resolve;
    private CoreGameEngineWenyiDamageHealthEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenyiDamageHealthEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> resolve) => new(resolve);
}
