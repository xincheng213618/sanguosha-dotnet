#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreActualTurnsRuntime))]
internal sealed partial class CoreActualTurnsEntries
{
    private readonly Func<EngineOwners.CoreActualTurnsRuntime> _resolve;
    private CoreActualTurnsEntries(Func<EngineOwners.CoreActualTurnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualTurnsEntries Create(Func<EngineOwners.CoreActualTurnsRuntime> resolve) => new(resolve);
}
