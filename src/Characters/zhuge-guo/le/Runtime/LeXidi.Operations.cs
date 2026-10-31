using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record LeXidiReorderStartedEvent(long FrameId, int OwnerSeat, int FluteCount, int RequestedCount) : IGameEvent;
internal sealed class ReorderLeZhugeGuoXidiDescriptor : LeZhugeGuoDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReorderLeZhugeGuoXidi; }
