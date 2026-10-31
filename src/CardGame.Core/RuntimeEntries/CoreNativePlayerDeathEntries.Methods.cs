#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativePlayerDeathEntries
{
    internal void BeginPlayerDeath(long parentFrameId, CharacterState victim, CharacterState? killer, GameEngine.IDamageAttempt? attack, DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default) => _resolve().BeginPlayerDeath(parentFrameId, victim, killer, attack, dying, causingProgramSkillFrameId, causingCardDamageReplacementId);
    internal void FinalizePlayerDeath(DyingFrame dying) => _resolve().FinalizePlayerDeath(dying);
}
