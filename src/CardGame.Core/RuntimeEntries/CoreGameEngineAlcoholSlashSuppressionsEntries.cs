#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _resolve;
    private CoreGameEngineAlcoholSlashSuppressionsEntries(Func<EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholSlashSuppressionsEntries Create(Func<EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> resolve) => new(resolve);
}
