namespace CardGame.Core;

public sealed partial record EquipmentRecastFrame(long Id, int ActorSeat, int CardId, CardKind EffectiveKind,
    bool DrawApplied = false, int DrawCount = 0)
    : ResolutionFrame(Id, ResolutionFrameKind.EquipmentRecast, ResolutionFrameStep.ResolvingEffect);
