using System.Globalization;
namespace CardGame.Core;
public sealed partial class GameEngine
{
}
internal interface IMouWenChouOlSelection : IProgramSelectionQuery { bool Target(int owner, int target); }
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 33100, SkillProgramEffectOp.UseMouWenChouLunzhan)]
internal sealed class MouWenChouTargetFilter : IProgramSelectionTargetFilter
{ public bool Allows(ProgramSelectionContext c, int target, IProgramSelectionQueries queries) => queries.Get<IMouWenChouOlSelection>().Target(c.OwnerSeat, target); }
