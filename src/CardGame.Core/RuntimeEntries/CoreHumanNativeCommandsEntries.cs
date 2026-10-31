#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHumanNativeCommandsRuntime))]
internal sealed partial class CoreHumanNativeCommandsEntries
{
    private readonly Func<EngineOwners.CoreHumanNativeCommandsRuntime> _resolve;
    private CoreHumanNativeCommandsEntries(Func<EngineOwners.CoreHumanNativeCommandsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHumanNativeCommandsEntries Create(Func<EngineOwners.CoreHumanNativeCommandsRuntime> resolve) => new(resolve);
}
