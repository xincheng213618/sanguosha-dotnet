#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed partial class CoreGameEngineConvertingGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConvertingGiftsRuntime> _resolve;
    private CoreGameEngineConvertingGiftsEntries(Func<EngineOwners.CoreGameEngineConvertingGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConvertingGiftsEntries Create(Func<EngineOwners.CoreGameEngineConvertingGiftsRuntime> resolve) => new(resolve);
}
