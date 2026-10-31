#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime))]
internal sealed partial class CoreGameEngineGameTargetHandHpChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> _resolve;
    private CoreGameEngineGameTargetHandHpChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameTargetHandHpChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> resolve) => new(resolve);
}
