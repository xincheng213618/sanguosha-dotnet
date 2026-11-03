namespace CardGame.Core;
public sealed record MouHqOriginalUse(long UseId,long ActionId,bool Outgoing);
internal interface IMouHuangQuanFacts:IProgramSelectionQuery
{
    int Turn{get;}int TurnOwner{get;}long MovementSequence{get;}
    bool Alive(int seat);bool Human(int seat);string Name(int seat);string CardName(int id);
    IReadOnlyList<int> Living{get;}ContentSkillDefinition Skill(string id);
    IEnumerable<T> History<T>()where T:IGameEvent;
    bool ParentMatches(ProgramSkillFrame f);MouHqOriginalUse? OriginalUse(int owner,ProgramSkillWindowContext? context);
    IReadOnlyList<MouHqEntity> TransferEntities(ProgramSkillFrame f,int actor);
    double HelpValue(int actor,int target);bool MovementRecoveryChild(ProgramSkillFrame f,ResolutionFrame child);
}
internal interface IMouHuangQuanNative:IProgramRuntimeOperations
{
    void Give(ProgramSkillFrame f,int giver,int recipient,IReadOnlyList<int> ids,long before);
    bool Drain(ProgramSkillFrame f);void OriginalEffect(ProgramSkillFrame f);void Finish(ProgramSkillFrame f,bool accepted);void Publish();
}
