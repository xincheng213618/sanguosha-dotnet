#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed partial class CoreGameEngineFixedRecipientBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _resolve;
    private CoreGameEngineFixedRecipientBenefitsEntries(Func<EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedRecipientBenefitsEntries Create(Func<EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> resolve) => new(resolve);
}
