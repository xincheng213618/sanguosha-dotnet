#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePhysicalCardDrawRuntime : global::CardGame.Core.IPhysicalCardDrawLogs, global::CardGame.Core.IPhysicalCardDrawPolicies
{
    private readonly ICorePhysicalCardDrawCharacterGuoZhaoAnniversaryOperations _characterGuoZhaoAnniversaryOperations;
    private readonly ICorePhysicalCardDrawCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICorePhysicalCardDrawModeRulesOperations _modeRulesOperations;
    private readonly ICorePhysicalCardDrawModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICorePhysicalCardDrawCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePhysicalCardDrawCoreShanheCampaignsOperations _coreShanheCampaignsOperations;
    private readonly ICorePhysicalCardDrawCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICorePhysicalCardDrawPhysicalCardDrawOperations _physicalCardDrawOperations;
    internal CoreGameEnginePhysicalCardDrawRuntime(ICorePhysicalCardDrawCharacterGuoZhaoAnniversaryOperations characterGuoZhaoAnniversaryOperations, ICorePhysicalCardDrawCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICorePhysicalCardDrawModeRulesOperations modeRulesOperations, ICorePhysicalCardDrawModeRuntimeOperations modeRuntimeOperations, ICorePhysicalCardDrawCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePhysicalCardDrawCoreShanheCampaignsOperations coreShanheCampaignsOperations, ICorePhysicalCardDrawCoreMatchLogOperations coreMatchLogOperations, ICorePhysicalCardDrawPhysicalCardDrawOperations physicalCardDrawOperations)
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
