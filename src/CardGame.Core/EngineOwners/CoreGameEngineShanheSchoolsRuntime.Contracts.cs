#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSchoolsShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface ICoreShanheSchoolsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player);
}
