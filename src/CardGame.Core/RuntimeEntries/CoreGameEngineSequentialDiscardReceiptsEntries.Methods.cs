#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardReceiptsEntries
{
    internal ProgramSkillFrame FreezeSequentialDiscardTopPayment(ProgramSkillFrame f, string bind, string? except, SkillProgramCardDestination destination) => _resolve().FreezeSequentialDiscardTopPayment(f, bind, except, destination);
    internal bool HasSequentialDiscardTopPayment(ProgramSkillFrame f, string bind) => _resolve().HasSequentialDiscardTopPayment(f, bind);
    internal bool ValidSequentialDiscard(ProgramSkillFrame f) => _resolve().ValidSequentialDiscard(f);
    internal bool ValidSequentialDiscardPayment(ProgramSkillFrame f, ProgramSequentialDiscardDraft d, ProgramSequentialDiscardPayment p) => _resolve().ValidSequentialDiscardPayment(f, d, p);
}
