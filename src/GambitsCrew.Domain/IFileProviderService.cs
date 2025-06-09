namespace GambitsCrew.Domain;

public interface IFileProviderService 
{
    public Stream GetCommand(string name);
    public Stream GetCondition(string name);
    public Stream GetCrewMember(string name);
    public Stream GetDeployment(string name);
    public Stream GetGambit(string name);
    public Stream GetOperator(string name);
    public Stream GetSelector(string name);
}
