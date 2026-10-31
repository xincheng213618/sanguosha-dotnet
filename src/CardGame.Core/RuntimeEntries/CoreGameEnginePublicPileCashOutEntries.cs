#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPileCashOutRuntime))]
internal sealed partial class CoreGameEnginePublicPileCashOutEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPileCashOutRuntime> _resolve;
    private CoreGameEnginePublicPileCashOutEntries(Func<EngineOwners.CoreGameEnginePublicPileCashOutRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileCashOutEntries Create(Func<EngineOwners.CoreGameEnginePublicPileCashOutRuntime> resolve) => new(resolve);
}
