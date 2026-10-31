#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoXinOlRuntime
{
    internal bool IsIssuedBaoXinMutaoPile(PublicPersistentPileSource source) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.BaoXin is { Paid: true, Pile: { } pile } r && pile == source && r.Op == SkillProgramEffectOp.ResolveBaoXinMutao && r.Source == BaoXinSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && f.SkillId == "ol:mutao" && r.Original.Count > 0 && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((source.OwnerSeat, source.SkillId, source.SkillInstanceId)) == source && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Holder == r.Holder) == 1 && r.Original.Select((c, i) => (c, i)).All(x => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinOriginalEntityEvent>().Count(e => e.FrameId == f.Id && e.Index == x.i && e.CardId == x.c.CardId && e.Kind == x.c.Kind && e.EffectiveKind == x.c.EffectiveKind && e.From == x.c.From) == 1));
}
