#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEndingPairDrawsRuntime))]
internal sealed partial class CoreGameEngineEndingPairDrawsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEndingPairDrawsRuntime> _resolve;
    private CoreGameEngineEndingPairDrawsEntries(Func<EngineOwners.CoreGameEngineEndingPairDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingPairDrawsEntries Create(Func<EngineOwners.CoreGameEngineEndingPairDrawsRuntime> resolve) => new(resolve);
}
