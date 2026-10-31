#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetBenefitWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitWindowsRuntime> resolve) => new(resolve);
}
