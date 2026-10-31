#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEnginePhysicalCardDrawRuntime : IPhysicalCardDrawLogs, IPhysicalCardDrawPolicies
{
    private readonly RuntimeEntries.CharacterGuoZhaoAnniversaryEntries _characterGuoZhaoAnniversaryOperations;
    private readonly RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheCampaignsEntries _coreShanheCampaignsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly ICorePhysicalCardDrawPhysicalCardDrawOperations _physicalCardDrawOperations;
    internal CoreGameEnginePhysicalCardDrawRuntime(RuntimeEntries.CharacterGuoZhaoAnniversaryEntries characterGuoZhaoAnniversaryOperations, RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, IKernelMatchModeRulesAccess modeRulesOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineShanheCampaignsEntries coreShanheCampaignsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, ICorePhysicalCardDrawPhysicalCardDrawOperations physicalCardDrawOperations)
    {
        this._characterGuoZhaoAnniversaryOperations = characterGuoZhaoAnniversaryOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreShanheCampaignsOperations = coreShanheCampaignsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._physicalCardDrawOperations = physicalCardDrawOperations;
    }
}
