#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 50)]
    internal bool GuanFirstBasicSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => GuanFirstBasicAvailable(query.Actor, query.Kind);
}
