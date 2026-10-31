#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongFriendRuntime
{
    internal int FriendManjuanMovedCount(CardMovementBatchContext batch, int seat, SkillProgramTrigger trigger, int fallback) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ExchangeFriendGainForDiscard) && batch.NativeDrawInvocation is { } draw && draw.OwnerSeat == seat && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw) ? draw.ActualCount : fallback;
    internal SkillProgramStepOutcome BeginFriendManjuan(ProgramSkillFrame frame)
    {
        var cards = FriendGainCardIds(frame.WindowContext!, frame.OwnerSeat).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { FriendPangTong = new(frame.InstructionIndex, SkillProgramEffectOp.ExchangeFriendGainForDiscard, FriendPangTongStage.Select, cards, [], []) });
        PublishFriendPangTongChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }
}
