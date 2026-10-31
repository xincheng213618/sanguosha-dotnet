#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaiShiCiMobileShiRuntime
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(CharacterState owner, string skillId) => _shiTaiShiCiOperations.Get().PublicStates(owner.Seat, skillId);
}
