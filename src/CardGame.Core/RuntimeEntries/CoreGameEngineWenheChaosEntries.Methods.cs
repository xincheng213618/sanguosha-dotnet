#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheChaosEntries
{
    internal void BeginWenheCardChaos(CardUseFrame use) => _resolve().BeginWenheCardChaos(use);
    internal void BeginWenheChaos(ProgramSkillFrame? roundParent, CardUseFrame? cardParent) => _resolve().BeginWenheChaos(roundParent, cardParent);
    internal bool IsExactWenheChaos(ProgramSkillFrame frame) => _resolve().IsExactWenheChaos(frame);
    internal bool TryCompleteWenheChaosProgram(ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteWenheChaosProgram(frame, completed);
    internal bool WenheChaosFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().WenheChaosFrameRidesOn(child, parent);
}
