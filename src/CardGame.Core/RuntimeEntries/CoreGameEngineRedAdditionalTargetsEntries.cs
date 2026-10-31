#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime))]
internal sealed partial class CoreGameEngineRedAdditionalTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> _resolve;
    private CoreGameEngineRedAdditionalTargetsEntries(Func<EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRedAdditionalTargetsEntries Create(Func<EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> resolve) => new(resolve);
}
