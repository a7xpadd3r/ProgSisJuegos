public interface IDeletableCommand : ICommand
{
    void Undo();
}
