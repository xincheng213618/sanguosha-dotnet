#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineQiaoquHaoduoRuntime))]
internal sealed partial class CoreGameEngineQiaoquHaoduoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> _resolve;
    private CoreGameEngineQiaoquHaoduoEntries(Func<EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQiaoquHaoduoEntries Create(Func<EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> resolve) => new(resolve);
}
