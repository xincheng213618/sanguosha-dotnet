#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime))]
internal sealed partial class CoreGameEngineNativeDrawAttemptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> _resolve;
    private CoreGameEngineNativeDrawAttemptsEntries(Func<EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawAttemptsEntries Create(Func<EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> resolve) => new(resolve);
}
