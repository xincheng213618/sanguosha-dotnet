#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePhysicalCardDrawRuntime : global::CardGame.Core.IPhysicalCardDrawLogs, global::CardGame.Core.IPhysicalCardDrawPolicies
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries _characterGuoZhaoAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheCampaignsEntries _coreShanheCampaignsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly ICorePhysicalCardDrawPhysicalCardDrawOperations _physicalCardDrawOperations;
    internal CoreGameEnginePhysicalCardDrawRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries characterGuoZhaoAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheCampaignsEntries coreShanheCampaignsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, ICorePhysicalCardDrawPhysicalCardDrawOperations physicalCardDrawOperations)
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
