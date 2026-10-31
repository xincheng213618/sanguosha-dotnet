#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime))]
internal sealed partial class CoreLeastHandMarkerDrawProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> _resolve;
    private CoreLeastHandMarkerDrawProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreLeastHandMarkerDrawProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> resolve) => new(resolve);
}
