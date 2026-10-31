#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpDamageShieldsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> _resolve;
    private CoreGameEngineHpDamageShieldsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpDamageShieldsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> resolve) => new(resolve);
}
