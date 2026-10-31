#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhaoZhongProvenanceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> _resolve;
    private CoreGameEngineZhaoZhongProvenanceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhaoZhongProvenanceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoZhongProvenanceRuntime> resolve) => new(resolve);
}
