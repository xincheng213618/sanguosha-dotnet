#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaiShiCiMobileShiRuntime
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(CharacterState owner, string skillId) => _shiTaiShiCiOperations.Get().PublicStates(owner.Seat, skillId);
}
