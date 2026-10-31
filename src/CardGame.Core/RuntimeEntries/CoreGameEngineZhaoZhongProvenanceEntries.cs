#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime))]
internal sealed partial class CoreGameEngineZhaoZhongProvenanceEntries
{
    private readonly Func<EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> _resolve;
    private CoreGameEngineZhaoZhongProvenanceEntries(Func<EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhaoZhongProvenanceEntries Create(Func<EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> resolve) => new(resolve);
}
