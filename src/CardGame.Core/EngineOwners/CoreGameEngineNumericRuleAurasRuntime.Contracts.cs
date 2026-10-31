#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNumericRuleAurasCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNumericRuleAurasCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetLivingFactionCount();
}

internal interface ICoreNumericRuleAurasCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreNumericRuleAurasCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}
