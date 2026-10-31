#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativePlayerDeathEntries
{
    internal void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default) => _resolve().BeginPlayerDeath(parentFrameId, victim, killer, attack, dying, causingProgramSkillFrameId, causingCardDamageReplacementId);
    internal void FinalizePlayerDeath(global::CardGame.Core.DyingFrame dying) => _resolve().FinalizePlayerDeath(dying);
}
