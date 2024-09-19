using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������ ���ؼ� �ൿ �����͸� ������ �� �ִ�.
public interface ICommand
{
    void Execute();
    void Undo();
}
public class MoveCommand : ICommand
{
    private Transform ObjecToMove;
    private Vector3 displacement;
   
    public MoveCommand(Transform obj, Vector3 displacement)
    {
        this.ObjecToMove = obj;
        this.displacement = displacement;
    }
    public void Execute() { ObjecToMove.position += displacement; }
    public void Undo() { ObjecToMove.position -= displacement; }
}
public class CommandManager : MonoBehaviour
{
    private Stack<ICommand> commandHistory = new Stack<ICommand>(); //���� ���·� Ŀ�ǵ� ����

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }
    public void UndoLastCommand()
    {
        if(commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}

