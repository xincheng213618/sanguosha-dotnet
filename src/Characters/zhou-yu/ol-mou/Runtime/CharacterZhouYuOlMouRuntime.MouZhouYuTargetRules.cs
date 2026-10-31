#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.NativeUseMaterialQualification, 10)]
    internal bool MouGuquNativeUseMaterialQualified(ProgramNativeUseMaterialQuery query) => IsMouGuquUseAllowed(query.ActorSeat, query.Kind, query.PhysicalCardIds, query.Source, query.ExistingUseFrameId);
}
