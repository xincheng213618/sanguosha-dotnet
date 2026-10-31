#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFireTargetBenefitsRuntime))]
internal sealed partial class CoreGameEngineFireTargetBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> _resolve;
    private CoreGameEngineFireTargetBenefitsEntries(Func<EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFireTargetBenefitsEntries Create(Func<EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> resolve) => new(resolve);
}
