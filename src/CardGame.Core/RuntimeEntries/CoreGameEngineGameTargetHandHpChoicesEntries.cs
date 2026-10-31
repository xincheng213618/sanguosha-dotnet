#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime))]
internal sealed partial class CoreGameEngineGameTargetHandHpChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> _resolve;
    private CoreGameEngineGameTargetHandHpChoicesEntries(Func<EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameTargetHandHpChoicesEntries Create(Func<EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> resolve) => new(resolve);
}
