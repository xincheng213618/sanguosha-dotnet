#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreWushuangModeProjectionRuntime))]
internal sealed class CoreWushuangAuditCombatCoreWushuangModeProjectionAdapter(Func<CoreWushuangModeProjectionRuntime> targetFactory) : ICoreWushuangAuditCombatCoreWushuangModeProjectionOperations
{
    private readonly Func<CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}
